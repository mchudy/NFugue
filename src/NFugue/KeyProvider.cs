using NFugue.Providers;
using NFugue.Theory;

namespace NFugue
{
    public class KeyProvider : IKeyProvider
    {
        private static Key key = new Key("");
        public Key CreateKey(string keySignature)
        {
            return key;
        }

        public string CreateKeyString(sbyte notePositionInOctave, sbyte scale)
        {
            throw new System.NotImplementedException();
        }

        public sbyte ConvertAccidentalCountToKeyRootPositionInOctave(int accidentalCount, sbyte scale)
        {
            throw new System.NotImplementedException();
        }

        public sbyte ConvertKeyToByte(Key key)
        {
            throw new System.NotImplementedException();
        }
    }
}