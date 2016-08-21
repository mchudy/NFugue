using NFugue.Theory;

namespace NFugue.Midi
{
    public static class MidiDefaults
    {
        public static readonly float DefaultDivisionType = 120f;
        public static readonly int DefaultResolutionTicksPerBeat = 128;
        public static readonly int DefaultTempoBeatsPerMinute = 120;
        public static readonly int DefaultTempoBeatsPerWhole = 4;
        public static readonly int DefaultMetronomePulse = 24;
        public static readonly int DefaultThirtysecondNotesPer24MidiClockSignals = 8;
        public static readonly int Tracks = 16;
        public static readonly int Layers = 16;
        public static readonly double MsPerMin = 60000.0d;
        public static readonly int DefaultMpq = 50; // Milliseconds per quarter note
        public static readonly sbyte SetTempoMessageType = 0x51;
        public static readonly sbyte PercussionTrack = 9;
        public static readonly sbyte MinPercussionNote = 35;
        public static readonly sbyte MaxPercussionNote = 81;
        public static readonly sbyte MinOnVelocity = 0;
        public static readonly sbyte MaxOnVelocity = 127;
        public static readonly sbyte DefaultOnVelocity = 64; // See also DefaultNoteSettingsManager 
        public static readonly sbyte MinOffVelocity = 0;
        public static readonly sbyte MaxOffVelocity = 127;
        public static readonly sbyte DefaultOffVelocity = 64; // See also DefaultNoteSettingsManager
        public static readonly int DefaultPatchBank = 0;
        public static readonly TimeSignature DefaultTimeSignature = new TimeSignature(4, 4);

        // Meta Message Type Values
        public static readonly sbyte MetaSequenceNumber = 0x00;
        public static readonly sbyte MetaTextEvent = 0x01;
        public static readonly sbyte MetaCopyrightNotice = 0x02;
        public static readonly sbyte MetaSequenceName = 0x03;
        public static readonly sbyte MetaInstrumentName = 0x04;
        public static readonly sbyte MetaLyric = 0x05;
        public static readonly sbyte MetaMarker = 0x06;
        public static readonly sbyte MetaCuePoint = 0x07;
        public static readonly sbyte MetaMidiChannelPrefix = 0x20;
        public static readonly sbyte MetaEndOfTrack = 0x2F;
        public static readonly sbyte MetaTempo = 0x51;
        public static readonly sbyte MetaSmtpeOffset = 0x54;
        public static readonly sbyte MetaTimesig = 0x58;
        public static readonly sbyte MetaKeysig = 0x59;
        public static readonly sbyte MetaVendor = 0x7F;
    }
}