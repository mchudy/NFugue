using NFugue.Theory;

namespace NFugue.Midi
{
    public static class MidiDefaults
    {
        public static readonly float DefaultDivisionType = 120;
        public static readonly int DefaultResolutionTicksPerBeat = 128;
        public static readonly int DefaultTempoBeatsPerMinute = 120;
        public static readonly int DefaultTempoBeatsPerWhole = 4;
        public static readonly int DefaultMetronomePulse = 24;
        public static readonly int DefaultThirtysecondNotesPer24MidiClockSignals = 8;
        public static readonly int Tracks = 16;
        public static readonly int Layers = 16;
        public static readonly double MsPerMin = 60000.0d;
        public static readonly int DefaultMpq = 50; // Milliseconds per quarter note
        public static readonly int SetTempoMessageType = 0x51;
        public static readonly int PercussionTrack = 9;
        public static readonly int MinPercussionNote = 35;
        public static readonly int MaxPercussionNote = 81;
        public static readonly int MinOnVelocity = 0;
        public static readonly int MaxOnVelocity = 127;
        public static readonly int DefaultOnVelocity = 64; // See also DefaultNoteSettingsManager 
        public static readonly int MinOffVelocity = 0;
        public static readonly int MaxOffVelocity = 127;
        public static readonly int DefaultOffVelocity = 64; // See also DefaultNoteSettingsManager
        public static readonly int DefaultPatchBank = 0;
        public static readonly TimeSignature DefaultTimeSignature = new TimeSignature(4, 4);

        // Meta Message Type Values
        public static readonly int MetaSequenceNumber = 0x00;
        public static readonly int MetaTextEvent = 0x01;
        public static readonly int MetaCopyrightNotice = 0x02;
        public static readonly int MetaSequenceName = 0x03;
        public static readonly int MetaInstrumentName = 0x04;
        public static readonly int MetaLyric = 0x05;
        public static readonly int MetaMarker = 0x06;
        public static readonly int MetaCuePoint = 0x07;
        public static readonly int MetaMidiChannelPrefix = 0x20;
        public static readonly int MetaEndOfTrack = 0x2F;
        public static readonly int MetaTempo = 0x51;
        public static readonly int MetaSmtpeOffset = 0x54;
        public static readonly int MetaTimesig = 0x58;
        public static readonly int MetaKeysig = 0x59;
        public static readonly int MetaVendor = 0x7F;
    }
}