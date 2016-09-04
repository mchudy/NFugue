using NFugue.Midi;
using NFugue.Patterns;
using NFugue.Staccato;
using Sanford.Multimedia.Midi;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NFugue.Playing
{
    public class Player
    {
        private readonly ManagedPlayer managedPlayer = new ManagedPlayer();
        private readonly MidiEventManager eventManager = new MidiEventManager();

        public StaccatoParser Parser { get; } = new StaccatoParser();

        public Player()
        {
            SubscribeToParserEvents();
        }

        public Sequence GetSequence(params IPatternProducer[] patternProducers)
        {
            return GetSequence(new Pattern(patternProducers));
        }

        public Sequence GetSequence(IPatternProducer patternProducer)
        {
            return GetSequence(patternProducer.GetPattern().ToString());
        }

        public Sequence GetSequence(params string[] strings)
        {
            return GetSequence(new Pattern(strings));
        }

        public Sequence GetSequence(string s)
        {
            Parser.Parse(s);
            return eventManager.Sequence;
        }

        public void Play(params IPatternProducer[] patternProducers)
        {
            Play(new Pattern(patternProducers));
        }

        public void Play(IPatternProducer patternProducer)
        {
            Play(patternProducer.GetPattern().ToString());
        }

        public void Play(params string[] strings)
        {
            Play(new Pattern(strings));
        }

        public void Play(string musicString)
        {
            Play(GetSequence(musicString));
        }

        public void Play(Sequence sequence)
        {
            managedPlayer.Start(sequence);
            while (!managedPlayer.IsFinished)
            {
                Thread.Sleep(20);
            }
        }

        public void DelayPlay(long millisToDelay, params IPatternProducer[] patternProducers)
        {
            DelayPlay(millisToDelay, new Pattern(patternProducers));
        }

        public void DelayPlay(long millisToDelay, IPatternProducer patternProducer)
        {
            DelayPlay(millisToDelay, patternProducer.GetPattern().ToString());
        }

        public void DelayPlay(long millisToDelay, params string[] strings)
        {
            DelayPlay(millisToDelay, new Pattern(strings));
        }

        public void DelayPlay(long millisToDelay, string s)
        {
            DelayPlay(millisToDelay, GetSequence(s));
        }

        public void DelayPlay(long millisToDelay, Sequence sequence)
        {
            Task.Run(() =>
            {
                Thread.Sleep((int)millisToDelay);
                Play(sequence);
            });
        }

        private void SubscribeToParserEvents()
        {
            Parser.BeforeParsingStarted += (s, e) => eventManager.Reset();
            Parser.AfterParsingFinished += (s, e) => eventManager.FinishSequence();
            Parser.TrackChanged += (s, e) => eventManager.CurrentTrackNumber = e.Track;
            Parser.LayerChanged += (s, e) => eventManager.CurrentLayerNumber = e.Layer;
            Parser.InstrumentParsed += (s, e) => eventManager.AddEvent(ChannelCommand.ProgramChange, e.Instrument, 0);
            Parser.TempoChanged += (s, e) => eventManager.SetTempo(e.TempoBPM);
            Parser.KeySignatureParsed += (s, e) => eventManager.SetKeySignature((byte)e.Key, (byte)e.Scale);
            Parser.TimeSignatureParsed += (s, e) => eventManager.SetTimeSignature((byte)e.Numerator, (byte)e.PowerOfTwo);
            Parser.TrackBeatTimeBookmarked += (s, e) => eventManager.AddTrackTickTimeBookmark(e.TimeBookmarkId);
            Parser.TrackBeatTimeBookmarkRequested +=
                (s, e) => eventManager.TrackBeatTime = eventManager.GetTrackBeatTimeBookmark(e.TimeBookmarkId);
            Parser.TrackBeatTimeRequested += (s, e) => eventManager.TrackBeatTime = e.Time;
            Parser.PitchWheelParsed += (s, e) => eventManager.AddEvent(ChannelCommand.PitchWheel, e.LSB, e.MSB);
            Parser.ChannelPressureParsed += (s, e) => eventManager.AddEvent(ChannelCommand.ChannelPressure, e.Pressure);
            Parser.PolyphonicPressureParsed +=
                (s, e) => eventManager.AddEvent(ChannelCommand.PolyPressure, e.Key, e.Pressure);
            Parser.SystemExclusiveParsed += (s, e) => eventManager.AddSystemExclusiveEvent(e.Bytes.Cast<byte>().ToArray());
            Parser.ControllerEventParsed +=
                (s, e) => eventManager.AddEvent(ChannelCommand.Controller, e.Controller, e.Value);
            Parser.LyricParsed +=
                (s, e) => eventManager.AddMetaMessage(MetaType.Lyric, Encoding.ASCII.GetBytes(e.Lyric));
            Parser.MarkerParsed +=
                (s, e) => eventManager.AddMetaMessage(MetaType.Marker, Encoding.ASCII.GetBytes(e.Marker));
            Parser.NoteParsed += (s, e) => eventManager.AddNote(e.Note);
            Parser.ChordParsed += (s, e) =>
            {
                foreach (var note in e.Chord.GetNotes())
                {
                    eventManager.AddNote(note);
                }
            };
        }
    }
}