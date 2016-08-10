using System;

namespace NFugue.Parser
{
    public class ParserException : Exception
    {
        public ParserException(string message) : base(message)
        {
        }

        public int Position { get; set; } = -1;
    }
}
