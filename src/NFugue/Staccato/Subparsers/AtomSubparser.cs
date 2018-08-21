using NFugue.Extensions;
using NFugue.Patterns;

namespace NFugue.Staccato.Subparsers
{
    /// <summary>
    /// Parses Instrument, Voice, and Layer tokens. Each has values that are parsed as bytes.
    /// </summary>
    public class AtomSubparser : ISubparser
    {
        public const char AtomChar = '&';
        public const char QuarkSeparator = ',';

        public bool Matches(string music) => music[0] == AtomChar;

        public TokenType GetTokenType(string tokenString)
        {
            if (tokenString[0] == AtomChar)
            {
                return TokenType.Atom;
            }

            return TokenType.UnknownToken;
        }

        public int Parse(string music, StaccatoParserContext context)
        {
            if (!Matches(music))
            {
                return 0;
            }

            int posNextSpace = music.FindNextOrEnd(' ');
            music = music.Substring(1, posNextSpace - 1); // Remove the initial character
            var quarks = music.Split(QuarkSeparator);
            var ivlSubparser = new IVLSubparser();

            foreach (string quark in quarks)
            {
                if (ivlSubparser.Matches(quark))
                {
                    ivlSubparser.Parse(quark, context);
                }
                else if (NoteSubparser.NoteSubparser.Instance.Matches(quark))
                {
                    NoteSubparser.NoteSubparser.Instance.Parse(quark, context);
                }
            }

            return posNextSpace + 1;
        }
    }
}
