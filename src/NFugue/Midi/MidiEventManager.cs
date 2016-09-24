using NFugue.Theory;
using NLog;
using Sanford.Multimedia.Midi;
using System.Linq;

namespace NFugue.Midi
{
    public class MidiEventManager : TrackTimeManager
    {
        private readonly Track[] tracks = new Track[MidiDefaults.Tracks];

        public float DivisionType { get; set; } = MidiDefaults.DefaultDivisionType;
        public int ResolutionTicksPerBeat { get; set; } = MidiDefaults.DefaultResolutionTicksPerBeat;
        public byte MetronomePulse { get; set; } = (byte)MidiDefaults.DefaultMetronomePulse;
        public Sequence Sequence { get; private set; }

        public byte ThirtySecondNotesPerQuarterNote { get; set; } =
            (byte)MidiDefaults.DefaultThirtysecondNotesPer24MidiClockSignals;

        public Track CurrentTrack
        {
            get
            {
                if (tracks[CurrentTrackNumber] == null)
                {
                    tracks[CurrentTrackNumber] = new Track();
                    Sequence.Add(tracks[CurrentTrackNumber]);
                }
                return tracks[CurrentTrackNumber];
            }
        }

        public void Reset()
        {
            Sequence = new Sequence((int)MidiDefaults.DefaultDivisionType);
            CreateTrack(0);
        }

        public void AddEvent(ChannelCommand command, int data)
        {
            var shortMessageBuilder = new ChannelMessageBuilder
            {
                Command = command,
                Data1 = CurrentTrackNumber,
                Data2 = data,
            };
            shortMessageBuilder.Build();
            CurrentTrack.Insert(ConvertBeatsToTicks(TrackBeatTime), shortMessageBuilder.Result);
        }

        public void AddEvent(ChannelCommand command, int data1, int data2)
        {
            LogManager.GetCurrentClassLogger().Trace($"New message {command} {data1} {data2} \nticks: {ConvertBeatsToTicks(TrackBeatTime)} layer:{CurrentLayerNumber} track: {CurrentTrackNumber} beats:{TrackBeatTime}");
            var shortMessageBuilder = new ChannelMessageBuilder
            {
                Command = command,
                Data1 = data1,
                Data2 = data2,
                MidiChannel = CurrentTrackNumber
            };
            shortMessageBuilder.Build();
            CurrentTrack.Insert(ConvertBeatsToTicks(TrackBeatTime), shortMessageBuilder.Result);
        }

        private int ConvertBeatsToTicks(double beats)
        {
            return (int)(ResolutionTicksPerBeat * beats * MidiDefaults.DefaultTempoBeatsPerWhole);
        }

        public void FinishSequence()
        {
            MetaMessage message = new MetaMessage(MetaType.EndOfTrack, new byte[] { });
            double latestTick = ConvertBeatsToTicks(Enumerable.Range(0, tracks.Length)
                .Select(i => GetLatestTrackBeatTime((sbyte)i))
                .Max());
            for (int i = 0; i < LastCreatedTrackNumber; i++)
            {
                if (tracks[i] != null)
                {
                    tracks[i].Insert((int)latestTick, message);
                }
            }
        }

        public void AddSystemExclusiveEvent(byte[] bytes)
        {
            var message = new SysExMessage(bytes);
            CurrentTrack.Insert(ConvertBeatsToTicks(TrackBeatTime), message);
        }

        public void SetTempo(int tempoBPM)
        {
            TempoChangeBuilder builder = new TempoChangeBuilder
            {
                Tempo = 60000000 / tempoBPM // convert to PPQ
            };
            builder.Build();
            CurrentTrack.Insert(ConvertBeatsToTicks(TrackBeatTime), builder.Result);
        }

        public void SetTimeSignature(byte numerator, byte denominator)
        {
            TimeSignatureBuilder builder = new TimeSignatureBuilder
            {
                Numerator = numerator,
                Denominator = denominator,
                ClocksPerMetronomeClick = MetronomePulse,
                ThirtySecondNotesPerQuarterNote = ThirtySecondNotesPerQuarterNote
            };
            builder.Build();
            CurrentTrack.Insert(ConvertBeatsToTicks(TrackBeatTime), builder.Result);
        }

        public void AddNote(Note note)
        {
            if (note.Duration == 0.0)
            {
                note.UseDefaultDuration();
            }
            if (note.IsFirstNote)
            {
                InitialNoteBeatTimeForHarmonicNotes = TrackBeatTime;
            }
            if (note.IsHarmonicNote)
            {
                TrackBeatTime = InitialNoteBeatTimeForHarmonicNotes;
            }
            if (note.IsRest)
            {
                AdvanceTrackBeatTime(note.Duration);
                return;
            }
            if (!note.IsEndOfTie)
            {
                AddEvent(ChannelCommand.NoteOn, note.Value, note.OnVelocity);
            }
            AdvanceTrackBeatTime(note.Duration);
            if (!note.IsStartOfTie)
            {
                AddEvent(ChannelCommand.NoteOff, note.Value, note.OffVelocity);
            }
        }

        protected override void CreateTrack(sbyte track)
        {
            base.CreateTrack(track);
            tracks[track] = new Track();
            Sequence.Add(tracks[track]);
        }

        public void SetKeySignature(byte key, byte scale)
        {
            MetaMessage message = new MetaMessage(MetaType.KeySignature, new[] { key, scale });
            CurrentTrack.Insert(ConvertBeatsToTicks(TrackBeatTime), message);
        }

        public void AddMetaMessage(MetaType type, byte[] bytes)
        {
            MetaMessage message = new MetaMessage(type, bytes);
            CurrentTrack.Insert(ConvertBeatsToTicks(TrackBeatTime), message);
        }
    }
}