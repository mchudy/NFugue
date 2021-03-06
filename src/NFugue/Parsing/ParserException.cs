﻿using System;

namespace NFugue.Parsing
{
    public class ParserException : Exception
    {
        public ParserException()
        {
        }

        public ParserException(string message) : base(message)
        {
        }

        public int Position { get; set; } = -1;
    }
}
