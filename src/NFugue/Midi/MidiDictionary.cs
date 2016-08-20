using System.Collections.Generic;

namespace NFugue.Midi
{
    public class MidiDictionary
    {
        public static IDictionary<string, byte> InstrumentStringToByte { get; } = new Dictionary<string, byte>();
        public static IDictionary<string, int> TempoStringToInt { get; } = new Dictionary<string, int>();
    }
}