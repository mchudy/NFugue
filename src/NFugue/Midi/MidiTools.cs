namespace NFugue.Midi
{
    public class MidiTools
    {
        public static int GetLSB(int value) => (value & 0x7F);
        public static int GetMSB(int value) => ((value >> 7) & 0x7F);
    }
}