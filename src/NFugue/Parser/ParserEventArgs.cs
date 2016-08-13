using NFugue.Theory;
using System;
using System.Collections.Generic;

namespace NFugue.Parser
{
    public class TrackChangedEventArgs : EventArgs
    {
        public sbyte Track { get; set; }
    }

    public class LayerChangedEventArgs : EventArgs
    {
        public sbyte Layer { get; set; }
    }

    public class InstrumentParsedEventArgs : EventArgs
    {
        public sbyte Instrument { get; set; }
    }

    public class TempoChangedEventArgs : EventArgs
    {
        public int TempoBPM { get; set; }
    }

    public class KeySignatureParsedEventArgs : EventArgs
    {
        public sbyte Key { get; set; }
        public sbyte Scale { get; set; }
    }

    public class TimeSignatureParsedEventArgs : EventArgs
    {
        public sbyte Numerator { get; set; }
        public sbyte PowerOfTwo { get; set; }
    }

    public class BarLineParsedEventArgs : EventArgs
    {
        public long Id { get; set; }
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
        public sbyte LSB { get; set; }
        public sbyte MSB { get; set; }
    }

    public class ChannelPressureParsedEventArgs : EventArgs
    {
        public sbyte Pressure { get; set; }
    }

    public class PolyphonicPressureParsedEventArgs : EventArgs
    {
        public sbyte Key { get; set; }
        public sbyte Pressure { get; set; }
    }

    public class SystemExclusiveParsedEventArgs : EventArgs
    {
        public IEnumerable<sbyte> Bytes { get; set; }
    }

    public class ControllerEventParsedEventArgs : EventArgs
    {
        public sbyte Controller { get; set; }
        public sbyte Value { get; set; }
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
