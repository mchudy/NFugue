using NFugue.Providers;

namespace NFugue.Theory
{
    public class Key
    {
        public static readonly Key Default = new Key("C4maj");

        public Key(string keySignature)
            : this(KeyProviderFactory.GetKeyProvider().CreateKey(keySignature))
        {
        }

        public Key(Key key)
        {
            Root = key.Root;
            Scale = key.Scale;
        }

        public Key(Note root, Scale scale)
        {
            Root = root;
            Scale = scale;
        }

        public Key(Chord chord)
        {
            Root = chord.Root;
            if (chord.IsMajor)
            {
                Scale = Scale.Major;
            }
            else if (chord.IsMinor)
            {
                Scale = Scale.Minor;
            }
        }

        public string KeySignature => Root + Scale.ToString();
        public Note Root { get; }
        public Scale Scale { get; }
    }
}