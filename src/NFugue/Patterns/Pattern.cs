using NFugue.Theory;
using System;
using System.Collections.Generic;

namespace NFugue.Patterns
{
    public class Pattern : IPatternProducer, ITokenProducer
    {
        private string v;

        public Pattern(Chord[] candidateNotes)
        {
        }

        public Pattern(string v)
        {
            this.v = v;
        }

        public Pattern(Note[] candidateNotes)
        {
            throw new System.NotImplementedException();
        }

        public Pattern()
        {
        }

        public void Add(string toString)
        {
            throw new System.NotImplementedException();
        }

        internal void Add(Chord chord)
        {
            throw new NotImplementedException();
        }

        public void Add(Pattern pattern)
        {
            throw new NotImplementedException();
        }

        public Pattern GetPattern()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Token> GetTokens()
        {
            throw new NotImplementedException();
        }
    }
}