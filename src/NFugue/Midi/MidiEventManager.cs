using NFugue.Extensions;
using NFugue.Theory;
using Sanford.Multimedia.Midi;

namespace NFugue.Midi
{
    public class MidiEventManager : TrackTimeManager
    {
        public Sequence Sequence;
        private readonly Track[] tracks = new Track[MidiDefaults.Tracks];

        public float DivisionType { get; set; } = MidiDefaults.DefaultDivisionType;
        public int ResolutionTicksPerBeat { get; set; } = MidiDefaults.DefaultResolutionTicksPerBeat;
        public byte MetronomePulse { get; set; } = (byte)MidiDefaults.DefaultMetronomePulse;

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
            Sequence = new Sequence((int)DivisionType);
            CreateTrack(0);
        }

        public void AddEvent(int command, int data)
        {
            var shortMessageBuilder = new ChannelMessageBuilder
            {
                Command = (ChannelCommand)command,
                Data1 = CurrentTrackNumber,
                Data2 = data,
            };
            shortMessageBuilder.Build();
            CurrentTrack.Add(shortMessageBuilder.Result);
        }

        public void AddEvent(ChannelCommand command, int data1, int data2)
        {
            var shortMessageBuilder = new ChannelMessageBuilder
            {
                Command = command,
                Data1 = data1,
                Data2 = data2,
                MidiChannel = CurrentTrackNumber
            };
            shortMessageBuilder.Build();
            CurrentTrack.Add(shortMessageBuilder.Result);
        }

        public void FinishSequence()
        {
            MetaMessage message = new MetaMessage(MetaType.EndOfTrack, null);
            for (int i = 0; i < LastCreatedTrackNumber; i++)
            {
                if (tracks[i] != null)
                {
                    tracks[i].Add(message);
                }
            }
        }

        public void AddSystemExclusiveEvent(byte[] bytes)
        {
            var message = new SysExMessage(bytes);
            CurrentTrack.Add(message);
        }

        public void SetTempo(int tempoBPM)
        {
            TempoChangeBuilder builder = new TempoChangeBuilder
            {
                Tempo = tempoBPM
            };
            builder.Build();
            CurrentTrack.Add(builder.Result);
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
            CurrentTrack.Add(builder.Result);
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
    }
}