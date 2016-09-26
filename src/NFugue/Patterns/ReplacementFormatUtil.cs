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
        /// Current known users include ChordProgression and Intervals.
        /// </summary>
        public static Pattern ReplaceDollarsWithCandidates(string sequence,
            IList<IPatternProducer> candidates, IPatternProducer underscoreReplacement)
        {
            var sb = new StringBuilder();

            int posPrevDollar = -1;
            int posNextDollar = 0;

            while (posNextDollar < sequence.Length)
            {
                posNextDollar = sequence.FindNextOrEnd('$', posPrevDollar);
                if (posPrevDollar + 1 < sequence.Length)
                {
                    sb.Append(sequence.Substring(posPrevDollar + 1, posNextDollar - posPrevDollar - 1));
                }
                if (posNextDollar != sequence.Length)
                {
                    string selectionString = sequence.Substring(posNextDollar + 1, 1);
                    if (selectionString.Equals("_"))
                    {
                        // If the underscore replacement has tokens, then the stuff after $_ needs to be applied to each
                        // token in the underscore replacement!
                        string[] replacementTokens = underscoreReplacement.GetPattern().ToString().Split(' ');
                        int nextSpaceInSequence = sequence.FindNextOrEnd(' ', posNextDollar);
                        foreach (string token in replacementTokens)
                        {
                            sb.Append(token);
                            sb.Append(sequence.Substring(posNextDollar + 2, nextSpaceInSequence - posNextDollar - 2));
                            sb.Append(" ");
                        }
                        posNextDollar = nextSpaceInSequence - 1;
                    }
                    else
                    {
                        int selection = int.Parse(sequence.Substring(posNextDollar + 1, 1));
                        if (selection > candidates.Count)
                        {
                            throw new ArgumentException($"The selector ${selection} is greater than the number of items to choose from, which has {candidates.Count} items");
                        }
                        sb.Append(candidates[selection].GetPattern());
                    }
                }
                posPrevDollar = posNextDollar + 1;
            }

            return new Pattern(sb.ToString().Trim());
        }
    }
}