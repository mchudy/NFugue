#pragma warning disable 659
namespace NFugue.Integration.MusicXml.Internals
{
    internal class KeySignature
    {
        public KeySignature(int key, int scale)
        {
            Key = key;
            Scale = scale;
        }

        public int Key { get; }
        public int Scale { get; }

        public override bool Equals(object obj)
        {
            KeySignature other = obj as KeySignature;
            if (other != null)
            {
                return other.Key == Key && other.Scale == Scale;
            }
            return false;
        }
    }
}