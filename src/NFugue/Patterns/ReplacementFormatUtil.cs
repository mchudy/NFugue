using NFugue.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFugue.Patterns
{
    public static class ReplacementFormatUtil
    {
        /// <summary>
        /// This util takes replacement strings with dollar signs, like "$0q $1h $2w", and replaces each $ index with a
        /// value from the array of candidates. $_ is replaced with the underscoreReplacement. Returns the resulting Pattern.
        /// Current known users include ChordProgression and Intervals and BrokenChordPreprocessor.
        /// </summary>
        public static Pattern ReplaceDollarsWithCandidates(string sequence,
            IList<IPatternProducer> candidates, IPatternProducer underscoreReplacement)
        {
            return ReplaceDollarsWithCandidates(sequence, candidates, underscoreReplacement, null, " ", " ", null);
        }

        public static Pattern ReplaceDollarsWithCandidates(string sequence, IList<IPatternProducer> candidates, 
            IPatternProducer underscoreReplacement, IDictionary<string, IPatternProducer> specialReplacers, 
            string inputSeparator, string outputSeparator, string finalThingToAppend)
        {
            var buddy = new StringBuilder();

            // Understand the contents of the special replacer map. 
            // Specifically, find the longest-length item in the map.
            int maxSpecialReplacerKeyLength = 0;
            if (specialReplacers != null)
            {
                foreach (string replacerKey in specialReplacers.Keys)
                {
                    if (replacerKey.Length > maxSpecialReplacerKeyLength)
                    {
                        maxSpecialReplacerKeyLength = replacerKey.Length;
                    }
                }
            }

            // Split the input into individual elements and parse each one.
            foreach (var elementWithSeparator in SplitElementsWithSeparators(sequence))
            {
                string element = elementWithSeparator.Element;
                string innerSeparator = elementWithSeparator.Separator.ToString();
                string distributionNeeded = null;
                string appender = "";
                bool replacementFound = false;
                if (element.StartsWith("$"))
                {
                    int indexForAppender = element.Length;
                    for (int len = Math.Min(maxSpecialReplacerKeyLength + 1, element.Length); len > 1 && !replacementFound; len--)
                    {
                        string selectionString = element.Substring(1, len - 1);
                        if (specialReplacers.ContainsKey(selectionString))
                        {
                            distributionNeeded = specialReplacers[selectionString].ToString();
                            indexForAppender = len;
                            replacementFound = true;
                        }
                    }
                    if (!replacementFound)
                    {
                        if (element[1] >= '0' && element[1] <= '9')
                        {
                            int index = int.Parse(element.Substring(1, 1));
                            distributionNeeded = candidates[index].ToString();
                            indexForAppender = 2;
                            replacementFound = true;
                        }
                        else if (element[1] == '!')
                        {
                            distributionNeeded = underscoreReplacement.ToString();
                            indexForAppender = 2;
                            replacementFound = true;
                        }
                    }
                    if (indexForAppender < element.Length)
                    {
                        appender = element.Substring(indexForAppender, element.Length - indexForAppender);
                        replacementFound = true;
                    }
                }
                else
                {
                    distributionNeeded = element;
                    replacementFound = true;
                }

                if (replacementFound)
                {
                    if (finalThingToAppend != null)
                    {
                        appender = appender + finalThingToAppend;
                    }
                    Distribute(buddy, distributionNeeded, appender);
                }

                buddy.Append(innerSeparator.Equals(",") ? " " : innerSeparator);
            }

            buddy.Remove(buddy.Length - 1, 1);

            return new Pattern(buddy.ToString());
        }

        private static void Distribute(StringBuilder builder, string elements, string appender)
        {
            foreach (string element in elements.Split(' '))
            {
                builder.Append(element);
                builder.Append(appender);
                builder.Append(" ");
            }

            builder.Remove(builder.Length - 1, 1);
        }

        private static List<ElementWithSeparator> SplitElementsWithSeparators(string sequence)
        {
            char[] separators = { ' ', '+', '_', ',' };
            var retVal = new List<ElementWithSeparator>();
            int startingPos = 0;
            int cursor = 0;
            int which = -1;
            while (cursor < sequence.Length)
            {
                cursor++;
                if (cursor < sequence.Length)
                {
                    if ((which = InWhich(sequence[cursor], separators)) != -1)
                    {
                        string element = sequence.Substring(startingPos, cursor - startingPos);
                        char separator = separators[which];
                        retVal.Add(new ElementWithSeparator(element, separator));
                        startingPos = cursor + 1;
                    }
                }
                else if (startingPos < sequence.Length)
                {
                    string element = sequence.Substring(startingPos, sequence.Length - startingPos);
                    char separator = ' ';
                    retVal.Add(new ElementWithSeparator(element, separator));
                }
            }
            return retVal;
        }


        private static int InWhich(char ch, char[] chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                if (ch == chars[i])
                {
                    return i;
                }
            }
            return -1;
        }


        private struct ElementWithSeparator
        {
            public readonly string Element;
            public readonly char Separator;

            public ElementWithSeparator(string element, char separator)
            {
                Separator = separator;
                Element = element;
            }
        }
    }
}