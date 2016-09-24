#pragma warning disable 659
namespace NFugue.Integration.MusicXml.Internals
{
    internal class KeySignature
    {
        public KeySignature(sbyte key, sbyte scale)
        {
            Key = key;
            Scale = scale;
        }

        public sbyte Key { get; }
        public sbyte Scale { get; }

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