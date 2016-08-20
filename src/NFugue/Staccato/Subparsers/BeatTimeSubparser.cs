using System;
using System.Text.RegularExpressions;
using NFugue.Extensions;
using NFugue.Patterns;

namespace NFugue.Staccato.Subparsers
{
    public class BeatTimeSubparser : ISubparser
    {
        public const char BeatTimeChar = '@';
        public const char BeatTimeUseMarker = '#';

        public bool Matches(string music)
        {
            return music[0] == BeatTimeChar;
        }

        public TokenType GetTokenType(string tokenString)
        {
            if (tokenString[0] == BeatTimeChar)
            {
                if (tokenString[1] == BeatTimeUseMarker)
                {
                    return TokenType.TrackTimeBookmarkRequested;
                }
                return TokenType.TrackTimeBookmark;
            }
            return TokenType.UnknownToken;
        }

        public int Parse(string music, StaccatoParserContext context)
        {
            if (music[0] == BeatTimeChar)
            {
                int posNextSpace = music.FindNextOrEnd(' ');
                if (posNextSpace > 1)
                {
                    // ported substring
                    string timeTrackId = music.Substring(1, posNextSpace - 1);
                    if (Regex.IsMatch(timeTrackId, "([0-9]+(\\.[0-9]+)*)"))
                    {
                        double time = double.Parse(timeTrackId);
                        context.Parser.OnTrackBeatTimeRequested(time);
                    }
                    else if (timeTrackId[0] == BeatTimeUseMarker)
                    {
                        string timeBookmarkId = timeTrackId.Substring(1, timeTrackId.Length - 1);
                        context.Parser.OnTrackBeatTimeBookmarkRequested(timeBookmarkId);
                    }
                }
                return Math.Max(1, posNextSpace);
            }
            return 0;
        }
    }
}