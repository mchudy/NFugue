using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NFugue.Staccato.Preprocessors
{
    public class ReplacementMapPreprocessor : IPreprocessor
    {
        private static readonly Regex replacementPatternWithBrackets = new Regex(@"<\S+>", RegexOptions.Compiled);
        private static readonly Regex replacementPatternWithoutBrackets = new Regex(@"\S+", RegexOptions.Compiled);

        public int Iterations { get; set; } = 1;
        public bool IsCaseSensitive { get; set; } = true;
        public bool RequiresAngleBrackets { get; set; } = true;
        public IDictionary<string, string> ReplacementMap { get; set; } = new Dictionary<string, string>();

        public string Preprocess(string s, StaccatoParserContext context)
        {
            string iteratingString = s;
            for (int i = 0; i < Iterations; i++)
            {
                StringBuilder sb = new StringBuilder();
                int posPrev = 0;
                foreach (Match match in GetReplacementRegex().Matches(iteratingString))
                {
                    string foundKey = RequiresAngleBrackets
                        ? match.Groups[0].Value.Substring(1, match.Groups[0].Length - 2)
                        : match.Groups[0].Value;
                    sb.Append(iteratingString.Substring(posPrev, match.Index - posPrev));
                    string lookupKey = IsCaseSensitive ? foundKey : foundKey.ToUpper();
                    string replacementValue;
                    if (ReplacementMap.TryGetValue(lookupKey, out replacementValue) && replacementValue != null)
                    {
                        sb.Append(ReplacementMap[lookupKey]);
                    }
                    else
                    {
                        sb.Append(foundKey); // If the key doesn't have a value, just put the key back - it might be intended for another parser or purpose
                    }
                    posPrev = match.Index + match.Length;
                }
                sb.Append(iteratingString.Substring(posPrev, iteratingString.Length - posPrev));
                iteratingString = sb.ToString();
            }
            return iteratingString;
        }

        private Regex GetReplacementRegex()
        {
            return RequiresAngleBrackets ? replacementPatternWithBrackets : replacementPatternWithoutBrackets;
        }
    }
}
