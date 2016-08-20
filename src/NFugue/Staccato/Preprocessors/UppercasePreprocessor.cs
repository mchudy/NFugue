using Staccato.Extensions;
using System;
using System.Text;

namespace Staccato.Preprocessors
{
    /// <summary>
    /// Turns to uppercase all tokens that are not lyrics, markers, or functions
    /// </summary>
    public class UppercasePreprocessor : IPreprocessor
    {
        // The characters indicating a lyric ('), tag (#), or instruction ({)
        // prevent this UppercaseProcessor from uppercasing the following token. Note that these
        // characters must start at the beginning of the token, otherwise they could
        // indicate a sharp (#) or the colon in a tuplet (*2:3).
        private static readonly char[] SafeChars = { '\'', '@', '#', '{' };

        public string Preprocess(string s, StaccatoParserContext context)
        {
            StringBuilder buddy = new StringBuilder();
            int pos = 0;
            while (pos < s.Length)
            {
                int upperUntil = s.FindNextOrEnd(SafeChars, pos);
                if ((upperUntil == 0) || (s[upperUntil - 1] == ' '))
                {
                    buddy.Append(s.Substring(pos, upperUntil - pos).ToUpper());
                    if (upperUntil < s.Length)
                    {
                        int lowerUntil = upperUntil;
                        if ((s[upperUntil + 1] == '(') || (s[upperUntil] == ':'))
                        {
                            lowerUntil = s.IndexOf(')', upperUntil + 1);
                            buddy.Append(s.Substring(upperUntil, lowerUntil - upperUntil));
                        }
                        else
                        {
                            lowerUntil = s.FindNextOrEnd(' ', upperUntil);
                            buddy.Append(s.Substring(upperUntil, lowerUntil - upperUntil));
                        }
                        upperUntil = lowerUntil;
                    }
                    pos = upperUntil;
                }
                else
                {
                    int min = Math.Min(s.Length, upperUntil + 1);
                    buddy.Append(s.Substring(pos, min - pos).ToUpper());
                    pos = min;
                }
            }
            return buddy.ToString();
        }
    }
}
