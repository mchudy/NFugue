using NFugue.Patterns;

namespace Staccato
{
    public interface ISubparser
    {
        /// <summary>
        /// Indicates whether the subparser should be responsible for parsing the given music string
        /// </summary>
        /// <param name="music">The Staccato music string to consider </param>
        /// <returns>true if this subparser will accept the music string, false otherwise</returns>
        bool Matches(string music);

        /// <summary>
        /// Asks the subparser to provide a <see cref="TokenType"/> for the given token
        /// </summary>
        /// <param name="tokenString">The Staccato token to map to a type</param>
        TokenType GetTokenType(string tokenString);

        /// <summary>
        /// Parses the given music string
        /// </summary>
        /// <param name="music"></param>
        /// <param name="context"></param>
        /// <returns>Updated parsing index into the Staccato music string</returns>
        int Parse(string music, StaccatoParserContext context);
    }
}