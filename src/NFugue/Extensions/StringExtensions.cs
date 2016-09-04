using System.Collections.Generic;

namespace NFugue.Extensions
{
    public static class StringExtensions
    {
        public static int FindNextOrEnd(this string s, char charToFind, int startIndex = 0)
        {
            return s.FindNextOrEnd(new[] { charToFind }, startIndex);
        }

        public static int FindNextOrEnd(this string s, IEnumerable<char> charsToFind, int startIndex = 0)
        {
            if (startIndex < 0) startIndex = 0;
            int position = int.MaxValue;
            foreach (var c in charsToFind)
            {
                int x = s.IndexOf(c, startIndex);
                if (x != -1 && x < position)
                {
                    position = x;
                }
            }
            if (position == int.MaxValue)
            {
                position = s.Length;
            }
            return position;
        }
    }
}