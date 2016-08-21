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
        private readonly StaccatoParser parser = new StaccatoParser();
        private readonly MidiEventManager eventManager = new MidiEventManager();

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
            parser.Parse(s);
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
            parser.BeforeParsingStarted += (s, e) => eventManager.Reset();
            parser.AfterParsingFinished += (s, e) => eventManager.FinishSequence();
            parser.TrackChanged += (s, e) => eventManager.CurrentTrackNumber = e.Track;
            parser.LayerChanged += (s, e) => eventManager.CurrentLayerNumber = e.Layer;
            parser.InstrumentParsed += (s, e) => eventManager.AddEvent(ChannelCommand.ProgramChange, e.Instrument, 0);
            parser.TempoChanged += (s, e) => eventManager.SetTempo(e.TempoBPM);
            parser.KeySignatureParsed += (s, e) => eventManager.SetKeySignature((byte)e.Key, (byte)e.Scale);
            parser.TimeSignatureParsed += (s, e) => eventManager.SetTimeSignature((byte)e.Numerator, (byte)e.PowerOfTwo);
            parser.TrackBeatTimeBookmarked += (s, e) => eventManager.AddTrackTickTimeBookmark(e.TimeBookmarkId);
            parser.TrackBeatTimeBookmarkRequested +=
                (s, e) => eventManager.TrackBeatTime = eventManager.GetTrackBeatTimeBookmark(e.TimeBookmarkId);
            parser.TrackBeatTimeRequested += (s, e) => eventManager.TrackBeatTime = e.Time;
            parser.PitchWheelParsed += (s, e) => eventManager.AddEvent(ChannelCommand.PitchWheel, e.LSB, e.MSB);
            parser.ChannelPressureParsed += (s, e) => eventManager.AddEvent(ChannelCommand.ChannelPressure, e.Pressure);
            parser.PolyphonicPressureParsed +=
                (s, e) => eventManager.AddEvent(ChannelCommand.PolyPressure, e.Key, e.Pressure);
            parser.SystemExclusiveParsed += (s, e) => eventManager.AddSystemExclusiveEvent(e.Bytes.Cast<byte>().ToArray());
            parser.ControllerEventParsed +=
                (s, e) => eventManager.AddEvent(ChannelCommand.Controller, e.Controller, e.Value);
            parser.LyricParsed +=
                (s, e) => eventManager.AddMetaMessage(MetaType.Lyric, Encoding.ASCII.GetBytes(e.Lyric));
            parser.MarkerParsed +=
                (s, e) => eventManager.AddMetaMessage(MetaType.Marker, Encoding.ASCII.GetBytes(e.Marker));
            parser.NoteParsed += (s, e) => eventManager.AddNote(e.Note);
            parser.ChordParsed += (s, e) =>
            {
                foreach (var note in e.Chord.GetNotes())
                {
                    eventManager.AddNote(note);
                }
            };
        }
    }
}