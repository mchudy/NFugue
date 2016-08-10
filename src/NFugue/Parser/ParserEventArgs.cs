using NFugue.Theory;
using System;
using System.Collections.Generic;

namespace NFugue.Parser
{
    public class TrackChangedEventArgs : EventArgs
    {
        public byte Track { get; set; }
    }

    public class LayerChangedEventArgs : EventArgs
    {
        public byte Layer { get; set; }
    }

    public class InstrumentParsedEventArgs : EventArgs
    {
        public byte Instrument { get; set; }
    }

    public class TempoChangedEventArgs : EventArgs
    {
        public int TempoBPM { get; set; }
    }

    public class KeySignatureParsedEventArgs : EventArgs
    {
        public byte Key { get; set; }
        public byte Scale { get; set; }
    }

    public class TimeSignatureParsedEventArgs : EventArgs
    {
        public byte Numerator { get; set; }
        public byte PowerOfTwo { get; set; }
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
        public byte LSB { get; set; }
        public byte MSB { get; set; }
    }

    public class ChannelPressureParsedEventArgs : EventArgs
    {
        public byte Pressure { get; set; }
    }

    public class PolyphonicPressureParsedEventArgs : EventArgs
    {
        public byte Key { get; set; }
        public byte Pressure { get; set; }
    }

    public class SystemExclusiveParsedEventArgs : EventArgs
    {
        public IEnumerable<byte> Bytes { get; set; }
    }

    public class ControllerEventParsedEventArgs : EventArgs
    {
        public byte Controller { get; set; }
        public byte Value { get; set; }
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
