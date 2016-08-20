using System.Text.RegularExpressions;
using NFugue.Patterns;

namespace NFugue.Staccato.Subparsers
{
    public class WhitespaceConsumer : ISubparser
    {
        private static readonly Regex WhiteSpaceRegex = new Regex(@"^\s+", RegexOptions.Compiled);


        public bool Matches(string music)
        {
            return WhiteSpaceRegex.IsMatch(music);
        }

        public TokenType GetTokenType(string tokenString)
        {
            if (Matches(tokenString))
            {
                return TokenType.Whitespace;
            }
            return TokenType.UnknownToken;
        }

        public int Parse(string music, StaccatoParserContext context)
        {
            var match = WhiteSpaceRegex.Match(music);
            return match.Success ? match.Length : 0;
        }
    }
}