namespace NFugue.Midi
{
    public class MidiTools
    {
        public static sbyte GetLSB(int value) => (sbyte)(value & 0x7F);
        public static sbyte GetMSB(int value) => (sbyte)((value >> 7) & 0x7F);
    }
}