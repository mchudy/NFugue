using NFugue.Parsing;
using NFugue.Theory;

namespace NFugue.Temporal
{
    public class TemporalEvents
    {
        public class TrackEvent : ITemporalEvent
        {
            private readonly int track;

            public TrackEvent(int track)
            {
                this.track = track;
            }

            public void Execute(Parser parser)
            {
                parser.OnTrackChanged(track);
            }
        }

        public class LayerEvent : ITemporalEvent
        {
            private readonly int layer;

            public LayerEvent(int layer)
            {
                this.layer = layer;
            }

            public void Execute(Parser parser)
            {
                parser.OnLayerChanged(layer);
            }
        }

        public class InstrumentEvent : ITemporalEvent
        {
            private readonly int instrument;

            public InstrumentEvent(int instrument)
            {
                this.instrument = instrument;
            }

            public void Execute(Parser parser)
            {
                parser.OnInstrumentParsed(instrument);
            }
        }

        public class TempoEvent : ITemporalEvent
        {
            private readonly int tempoBPM;

            public TempoEvent(int tempoBPM)
            {
                this.tempoBPM = tempoBPM;
            }

            public void Execute(Parser parser)
            {
                parser.OnTempoChanged(tempoBPM);
            }
        }

        public class KeySignatureEvent : ITemporalEvent
        {
            private readonly int key;
            private readonly int scale;

            public KeySignatureEvent(int key, int scale)
            {
                this.key = key;
                this.scale = scale;
            }

            public void Execute(Parser parser)
            {
                parser.OnKeySignatureParsed(key, scale);
            }
        }

        public class TimeSignatureEvent : ITemporalEvent
        {
            private readonly int numerator;
            private readonly int powerOfTwo;

            public TimeSignatureEvent(int numerator, int powerOfTwo)
            {
                this.numerator = numerator;
                this.powerOfTwo = powerOfTwo;
            }

            public void Execute(Parser parser)
            {
                parser.OnTimeSignatureParsed(numerator, powerOfTwo);
            }
        }

        public class BarEvent : ITemporalEvent
        {
            private readonly long barId;

            public BarEvent(long barId)
            {
                this.barId = barId;
            }

            public void Execute(Parser parser)
            {
                parser.OnBarLineParsed(barId);
            }
        }

        //     public void trackBeatTimeBookmarked(string timeBookmarkId);
        //     public void trackBeatTimeBookmarkRequested(string timeBookmarkId);
        //     public void trackBeatTimeRequested(double time); 

        public class PitchWheelEvent : ITemporalEvent
        {
            private readonly int lsb;
            private readonly int msb;

            public PitchWheelEvent(int lsb, int msb)
            {
                this.lsb = lsb;
                this.msb = msb;
            }

            public void Execute(Parser parser)
            {
                parser.OnKeySignatureParsed(lsb, msb);
            }
        }

        public class ChannelPressureEvent : ITemporalEvent
        {
            private readonly int pressure;

            public ChannelPressureEvent(int pressure)
            {
                this.pressure = pressure;
            }

            public void Execute(Parser parser)
            {
                parser.OnChannelPressureParsed(pressure);
            }
        }

        public class PolyphonicPressureEvent : ITemporalEvent
        {
            private readonly int key;
            private readonly int pressure;

            public PolyphonicPressureEvent(int key, int pressure)
            {
                this.key = key;
                this.pressure = pressure;
            }

            public void Execute(Parser parser)
            {
                parser.OnPolyphonicPressureParsed(key, pressure);
            }
        }

        public class SystemExclusiveEvent : ITemporalEvent
        {
            private readonly byte[] bytes;

            public SystemExclusiveEvent(params byte[] bytes)
            {
                this.bytes = bytes;
            }

            public void Execute(Parser parser)
            {
                parser.OnSystemExclusiveParsed(bytes);
            }
        }

        public class ControllerEvent : ITemporalEvent
        {
            private readonly int controller;
            private readonly int value;

            public ControllerEvent(int controller, int value)
            {
                this.controller = controller;
                this.value = value;
            }

            public void Execute(Parser parser)
            {
                parser.OnControllerEventParsed(controller, value);
            }
        }

        public class LyricEvent : ITemporalEvent
        {
            private readonly string lyric;

            public LyricEvent(string lyric)
            {
                this.lyric = lyric;
            }

            public void Execute(Parser parser)
            {
                parser.OnLyricParsed(lyric);
            }
        }

        public class MarkerEvent : ITemporalEvent
        {
            private readonly string marker;

            public MarkerEvent(string marker)
            {
                this.marker = marker;
            }

            public void Execute(Parser parser)
            {
                parser.OnMarkerParsed(marker);
            }
        }

        public class UserEvent : ITemporalEvent
        {
            private readonly string id;
            private readonly object message;

            public UserEvent(string id, object message)
            {
                this.id = id;
                this.message = message;
            }

            public void Execute(Parser parser)
            {
                parser.OnFunctionParsed(id, message);
            }
        }

        public class NoteEvent : IDurationTemporalEvent
        {
            private Note note;

            public NoteEvent(Note note)
            {
                this.note = note;
            }

            public void Execute(Parser parser)
            {
                parser.OnNoteParsed(note);
            }

            public double Duration
            {
                get => note.Duration;
            }
        }

        public class ChordEvent : IDurationTemporalEvent
        {
            private readonly Chord chord;

            public ChordEvent(Chord chord)
            {
                this.chord = chord;
            }

            public void Execute(Parser parser)
            {
                parser.OnChordParsed(chord);
            }

            public double Duration
            {
                get => chord.GetNotes()[0].Duration;
            }
        }
    }
}
