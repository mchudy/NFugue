using NFugue.Patterns;
using Staccato.Extensions;
using System;
using System.Text.RegularExpressions;

namespace Staccato.Subparsers
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
                    string timeTrackId = music.Substring(1, posNextSpace);
                    if (Regex.IsMatch(timeTrackId, "([0-9]+(\\.[0-9]+)*)"))
                    {
                        double time = double.Parse(timeTrackId);
                        context.Parser.OnTrackBeatTimeRequested(time);
                    }
                    else if (timeTrackId[0] == BeatTimeUseMarker)
                    {
                        string timeBookmarkId = timeTrackId.Substring(1, timeTrackId.Length);
                        context.Parser.OnTrackBeatTimeBookmarkRequested(timeBookmarkId);
                    }
                }
                return Math.Max(1, posNextSpace);
            }
            return 0;
        }
    }
}