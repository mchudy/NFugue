using System;
using System.Text.RegularExpressions;
using NFugue.Extensions;
using NFugue.Patterns;

namespace NFugue.Staccato.Subparsers
{
    public class BarLineSubparser : ISubparser
    {
        public const char Barline = '|';

        public bool Matches(string music)
        {
            return music[0] == Barline;
        }

        public TokenType GetTokenType(string tokenString)
        {
            if (tokenString[0] == Barline)
            {
                return TokenType.BarLine;
            }
            return TokenType.UnknownToken;
        }

        public int Parse(string music, StaccatoParserContext context)
        {
            if (music[0] == Barline)
            {
                int posNextSpace = music.FindNextOrEnd(' ');
                long measure = -1;
                if (posNextSpace > 1)
                {
                    string barId = music.Substring(1, posNextSpace - 1);
                    if (Regex.IsMatch(barId, @"\d+"))
                    {
                        measure = long.Parse(barId);
                    }
                    else
                    {
                        measure = (long)context.Dictionary[barId];
                    }
                }
                context.Parser.OnBarLineParsed(measure);
                return Math.Max(1, posNextSpace);
            }
            return 0;
        }
    }
}
