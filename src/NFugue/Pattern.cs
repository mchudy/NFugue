using NFugue.Theory;

namespace NFugue
{
    public class Pattern
    {
        private string v;

        public Pattern(string v)
        {
            this.v = v;
        }

        public Pattern(Note[] candidateNotes)
        {
            throw new System.NotImplementedException();
        }
    }
}