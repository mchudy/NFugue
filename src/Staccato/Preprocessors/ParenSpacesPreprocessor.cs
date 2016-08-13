using Staccato.Extensions;
using System.Text;

namespace Staccato.Preprocessors
{
    /// <summary>
    /// Changes spaces in parentheses to underscores, since the Staccato string is split on parentheses
    /// </summary>
    public class ParenSpacesPreprocessor : IPreprocessor
    {
        public string Preprocess(string s, StaccatoParserContext context)
        {
            StringBuilder sb = new StringBuilder();
            int pos = 0;
            while (pos < s.Length)
            {
                int keepSpacesUntil = s.FindNextOrEnd('(', pos);
                int endParen = s.FindNextOrEnd(')', keepSpacesUntil);
                sb.Append(s.Substring(pos, keepSpacesUntil - pos));
                for (int i = keepSpacesUntil; i < endParen; i++)
                {
                    if (s[i] == ' ')
                    {
                        sb.Append('_');
                    }
                    else
                    {
                        sb.Append(s[i]);
                    }
                }
                pos = endParen;
            }
            return sb.ToString();
        }

        public static string Unprocess(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in s)
            {
                if (ch == '_')
                {
                    sb.Append(' ');
                }
                else
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString();
        }
    }
}