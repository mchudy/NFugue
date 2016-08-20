using System;
using NFugue.Extensions;
using NFugue.Patterns;
using NFugue.Staccato.Preprocessors;

namespace NFugue.Staccato.Subparsers
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
                lyricOrMarker = ParenSpacesPreprocessor.Unprocess(lyricOrMarker);
                if (music[0] == LyricChar)
                {
                    context.Parser.OnLyricParsed(lyricOrMarker);
                }
                else
                {
                    context.Parser.OnTrackBeatTimeBookmarked(lyricOrMarker);
                    context.Parser.OnMarkerParsed(lyricOrMarker);
                }
                return Math.Max(1, Math.Min(posNext + 1, music.Length));
            }
            return 0;
        }
    }
}