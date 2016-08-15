using Staccato.Functions;
using System.Text;
using System.Text.RegularExpressions;

namespace Staccato.Preprocessors
{
    public class FunctionPreprocessor : IPreprocessor
    {
        private static readonly Regex functionPattern = new Regex(@":\S+\(\p{ASCII}*\)", RegexOptions.Compiled);
        private static readonly Regex namePattern = new Regex(@"\S+\(", RegexOptions.Compiled);
        private static readonly Regex paramPattern = new Regex(@"\(\p{ASCII}*\)", RegexOptions.Compiled);

        public string Preprocess(string musicString, StaccatoParserContext context)
        {
            var sb = new StringBuilder();
            int posPrev = 0;
            foreach (Match match in functionPattern.Matches(musicString))
            {
                string functionName = null;
                string parameters = null;

                foreach (Match nameMatch in namePattern.Matches(match.Groups[0].ToString()))
                {
                    functionName = nameMatch.Groups[0].ToString().Substring(1, nameMatch.Groups[0].Length - 2);
                }
                //TODO
                var function = FunctionManager.Instance.GetPreprocessorFunction(functionName);
                if (function == null)
                {
                    return musicString;
                }
                foreach (Match paramMatch in paramPattern.Matches(match.Groups[0].ToString()))
                {
                    parameters = paramMatch.Groups[0].ToString().Substring(1, paramMatch.Groups[0].Length - 2);
                }
                sb.Append(musicString.Substring(posPrev, match.Index - posPrev));
                sb.Append(function.Apply(parameters, context));
                posPrev = match.Index + match.Length;
            }
            sb.Append(musicString.Substring(posPrev, musicString.Length - posPrev));
            return sb.ToString();
        }
    }
}