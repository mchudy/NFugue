namespace NFugue.Theory
{
    public class KeyProvider
    {
        private static Key key = new Key("");
        public Key CreateKey(string keySignature)
        {
            return key;
        }
    }
}