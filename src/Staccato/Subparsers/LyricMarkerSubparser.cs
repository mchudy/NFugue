using NFugue.Patterns;
using Staccato.Extensions;

namespace Staccato.Subparsers
{
    public class LyricMarkerSubparser : ISubparser
    {
        public const char LyricChar = '\'';
        public const char MarkerChar = '#';

        public bool Matches(string music)
        {
            return music[0] == LyricChar || music[0] == MarkerChar;
        }

        public TokenType GetTokenType(string tokenString)
        {
            if (tokenString[0] == LyricChar)
            {
                return TokenType.Lyric;
            }
            if (tokenString[0] == MarkerChar)
            {
                return TokenType.Marker;
            }
            return TokenType.UnknownToken;
        }

        public int Parse(string music, StaccatoParserContext context)
        {
            if (music[0] == LyricChar || music[0] == MarkerChar)
            {
                string lyricOrMarker = null;
                int posNext = 0;
                if (music[1] == '(')
                {
                    posNext = music.FindNextOrEnd(')');
                }
                else
                {
                    posNext = music.FindNextOrEnd(' ');
                }
                int startPos = music[1] == '(' ? 2 : 1;
                lyricOrMarker = music.Substring(startPos, posNext - startPos);
                //lyricOrMarker
            }
            return 0;
        }
    }
}