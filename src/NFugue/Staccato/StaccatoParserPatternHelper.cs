using NFugue.Patterns;
using System.Collections.Generic;
using System.Linq;

namespace NFugue.Staccato
{
    /// <summary>
    /// Certain functionality of Patterns depend on being able to know what each token
    /// parses as. This class provides a bridge between Patterns and Tokens and the
    /// StaccatoParser.While the methods below could be a part of either StaccatoParser
    /// or Pattern, placing them here helps keep those two classes cleaner.
    /// </summary>
    public class StaccatoParserPatternHelper
    {
        private readonly StaccatoParser parser = new StaccatoParser();

        public List<Token> GetTokens(IPatternProducer p)
        {
            var tokenStrings = parser.PreprocessAndSplit(p.ToString());
            return tokenStrings.Select(tokenString => new Token(tokenString, GetTokenType(tokenString)))
                .ToList();
        }

        public TokenType GetTokenType(string tokenString)
        {
            foreach (ISubparser sub in parser.Subparsers)
            {
                if (sub.Matches(tokenString))
                {
                    return sub.GetTokenType(tokenString);
                }
            }
            return TokenType.UnknownToken;
        }
    }
}
