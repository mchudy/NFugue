using NFugue.Theory;
using System;

namespace NFugue.Parsing
{
    public class TrackChangedEventArgs : EventArgs
    {
        public int Track { get; set; }
    }

    public class LayerChangedEventArgs : EventArgs
    {
        public int Layer { get; set; }
    }

    public class InstrumentParsedEventArgs : EventArgs
    {
        public int Instrument { get; set; }
    }

    public class TempoChangedEventArgs : EventArgs
    {
        public int TempoBPM { get; set; }
    }

    public class KeySignatureParsedEventArgs : EventArgs
    {
        public int Key { get; set; }
        public int Scale { get; set; }
    }

    public class TimeSignatureParsedEventArgs : EventArgs
    {
        public int Numerator { get; set; }
        public int PowerOfTwo { get; set; }
    }

    public class BarLineParsedEventArgs : EventArgs
    {
        public long Time { get; set; }
    }

    public class TrackBeatTimeBookmarkEventArgs : EventArgs
    {
        public string TimeBookmarkId { get; set; }
    }

    public class TrackBeatTimeRequestedEventArgs : EventArgs
    {
        public double Time { get; set; }
    }

    public class PitchWheelParsedEventArgs : EventArgs
    {
        public int LSB { get; set; }
        public int MSB { get; set; }
    }

    public class ChannelPressureParsedEventArgs : EventArgs
    {
        public int Pressure { get; set; }
    }

    public class PolyphonicPressureParsedEventArgs : EventArgs
    {
        public int Key { get; set; }
        public int Pressure { get; set; }
    }

    public class SystemExclusiveParsedEventArgs : EventArgs
    {
        public byte[] Bytes { get; set; }
    }

    public class ControllerEventParsedEventArgs : EventArgs
    {
        public int Controller { get; set; }
        public int Value { get; set; }
    }

    public class LyricParsedEventArgs : EventArgs
    {
        public string Lyric { get; set; }
    }

    public class MarkerParsedEventArgs : EventArgs
    {
        public string Marker { get; set; }
    }

    public class FunctionParsedEventArgs : EventArgs
    {
        public string Id { get; set; }
        public object Message { get; set; }
    }

    public class NoteEventArgs : EventArgs
    {
        public Note Note { get; set; }
    }

    public class ChordParsedEventArgs : EventArgs
    {
        public Chord Chord { get; set; }
    }

}
