using NFugue.Theory;
using Sanford.Multimedia.Midi;
using System.Collections.Generic;
using System.Linq;

namespace NFugue.Midi
{
    public class MidiEventManager : TrackTimeManager
    {
        private readonly Track[] tracks = new Track[MidiDefaults.Tracks];
        private readonly List<MessageToInsert> messagesToInsert = new List<MessageToInsert>();

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

        public void AddEvent(ChannelCommand command, int data)
        {
            var shortMessageBuilder = new ChannelMessageBuilder
            {
                Command = command,
                Data1 = CurrentTrackNumber,
                Data2 = data,
            };
            shortMessageBuilder.Build();
            QueueMessage(shortMessageBuilder.Result);
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
            QueueMessage(shortMessageBuilder.Result);
        }

        public void AddMetaMessage(MetaType type, byte[] bytes)
        {
            MetaMessage message = new MetaMessage(type, bytes);
            QueueMessage(message);
        }

        public void AddSystemExclusiveEvent(byte[] bytes)
        {
            var message = new SysExMessage(bytes);
            QueueMessage(message);
        }

        public void SetTempo(int tempoBPM)
        {
            TempoChangeBuilder builder = new TempoChangeBuilder
            {
                Tempo = 60000000 / tempoBPM // convert to PPQ
            };
            builder.Build();
            QueueMessage(builder.Result);
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
            QueueMessage(builder.Result);
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

        public void SetKeySignature(byte key, byte scale)
        {
            MetaMessage message = new MetaMessage(MetaType.KeySignature, new[] { key, scale });
            QueueMessage(message);
        }

        public void FinishSequence()
        {
            InsertChannelMessages();
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

        public void Reset()
        {
            messagesToInsert.Clear();
            Sequence = new Sequence((int)MidiDefaults.DefaultDivisionType);
            CreateTrack(0);
        }

        protected override void CreateTrack(sbyte track)
        {
            base.CreateTrack(track);
            tracks[track] = new Track();
            Sequence.Add(tracks[track]);
        }

        private int ConvertBeatsToTicks(double beats)
        {
            return (int)(ResolutionTicksPerBeat * beats * MidiDefaults.DefaultTempoBeatsPerWhole);
        }

        // This kind of lazy track creation is necessary because we need to make sure that all NoteOff events
        // will be inserted before all NoteOn events at the same position (Track.Insert inserts at the first
        // available position in the list).
        private void QueueMessage(IMidiMessage message)
        {
            messagesToInsert.Add(new MessageToInsert(ConvertBeatsToTicks(TrackBeatTime),
                message, CurrentTrack));
        }

        private void InsertChannelMessages()
        {
            foreach (var msg in messagesToInsert.OrderBy(m => m.Position))
            {
                msg.Track.Insert(msg.Position, msg.Message);
            }
        }

        private class MessageToInsert
        {
            public int Position { get; }
            public IMidiMessage Message { get; }
            public Track Track { get; }

            public MessageToInsert(int position, IMidiMessage message, Track track)
            {
                Position = position;
                Message = message;
                Track = track;
            }
        }
    }
}