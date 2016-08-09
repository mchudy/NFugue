using System;
using NFugue.Theory;

namespace NFugue
{
    public class Pattern
    {
        private string v;

        public Pattern()
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

        public void Add(string toString)
        {
            throw new System.NotImplementedException();
        }

        internal void Add(Chord chord)
        {
            throw new NotImplementedException();
        }
    }
}