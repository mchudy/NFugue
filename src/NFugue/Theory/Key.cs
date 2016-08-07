namespace NFugue.Theory
{
    public class Key
    {
        public Key(Note root, Scale scale)
        {
            Root = root;
            Scale = scale;
        }

        public Note Root { get; }
        public Scale Scale { get; }
    }
}