using NFugue.Patterns;
using Staccato.Extensions;
using System.Text.RegularExpressions;

namespace Staccato.Subparsers
{
    public class TempoSubparser : ISubparser
    {
        public const char TempoChar = 'T';

        public bool Matches(string music)
        {
            return music[0] == TempoChar;
        }

        public TokenType GetTokenType(string tokenString)
        {
            if (tokenString[0] == TempoChar)
            {
                return TokenType.Tempo;
            }
            return TokenType.UnknownToken;
        }

        public int Parse(string music, StaccatoParserContext context)
        {
            if (!Matches(music)) return 0;
            int posNextSpace = music.FindNextOrEnd(' ');
            int tempo = -1;
            if (posNextSpace > 1)
            {
                string tempoId = music.Substring(1, posNextSpace - 1);
                if (Regex.IsMatch(tempoId, @"\d+"))
                {
                    tempo = int.Parse(tempoId);
                }
                else
                {
                    if (tempoId[0] == '[')
                    {
                        tempoId = tempoId.Substring(1, tempoId.Length - 2);
                    }
                    tempo = (int)context.Dictionary[tempoId];
                }
            }
            context.Parser.OnTempoChanged(tempo);
            return posNextSpace + 1;
        }

        public static void PopulateContext(StaccatoParserContext context)
        {
            //TODO
        }
    }
}