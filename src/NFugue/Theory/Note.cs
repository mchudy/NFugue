namespace NFugue.Theory
{
    public class Note
    {
        public Note(Note root, Scale scale)
        {
            Root = root;
            Scale = scale;
        }

        public Note Root { get; }
        public Scale Scale { get; }
    }
}