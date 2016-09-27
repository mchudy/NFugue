using NFugue.Theory;
using Sanford.Multimedia.Midi;
using System.Collections.Generic;
using System.Linq;

namespace NFugue.Midi
{
    /// <summary>
    /// Places musical data into the a MIDI sequence
    /// </summary>
    public class MidiEventManager : TrackTimeManager
    {
        private readonly Track[] tracks = new Track[MidiDefaults.Tracks];
        private readonly List<MessageToInsert> messagesToInsert = new List<MessageToInsert>();

        public float DivisionType { get; set; } = MidiDefaults.DefaultDivisionType;
        public int ResolutionTicksPerBeat { get; set; } = MidiDefaults.DefaultResolutionTicksPerBeat;
        public byte MetronomePulse { get; set; } = (byte)MidiDefaults.DefaultMetronomePulse;

        /// <summary>
        /// Returns the current sequence, which is a collection of tracks
        /// </summary>
        /// <remarks>
        /// If your goal is to add events to the sequence, you don't want to use this method to
        /// get the sequence; instead, use the AddEvent methods to add your events.
        /// </remarks>
        public Sequence Sequence { get; private set; }

        public byte ThirtySecondNotesPerQuarterNote { get; set; } =
            (byte)MidiDefaults.DefaultThirtysecondNotesPer24MidiClockSignals;

        /// <summary>
        /// Returns the track indicated by <code>CurrentTrackNumber</code> and creates
        /// it if it does not already exist.
        /// </summary>
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

        /// <summary>
        /// Adds a MIDI event to the current track
        /// </summary>
        /// <param name="command">MIDI command represented by the message</param>
        /// <param name="data">The first data byte</param>
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

        /// <summary>
        /// Adds a MIDI event to the current track
        /// </summary>
        /// <param name="command">MIDI command represented by the message</param>
        /// <param name="data1">The first data byte</param>
        /// <param name="data2">The second data byte</param>
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

        /// <summary>
        /// Adds a MetaMessage to the current track
        /// </summary>
        /// <param name="type">Type of the message</param>
        /// <param name="bytes">Message data</param>
        public void AddMetaMessage(MetaType type, byte[] bytes)
        {
            MetaMessage message = new MetaMessage(type, bytes);
            QueueMessage(message);
        }

        /// <summary>
        /// Adds a SysexMessage to the current track
        /// </summary>
        /// <param name="bytes">Message data</param>
        public void AddSystemExclusiveEvent(byte[] bytes)
        {
            var message = new SysExMessage(bytes);
            QueueMessage(message);
        }

        /// <summary>
        /// Adds a MIDI message which sets the tempo
        /// </summary>
        /// <param name="tempoBPM">Tempo in BPM</param>
        public void SetTempo(int tempoBPM)
        {
            TempoChangeBuilder builder = new TempoChangeBuilder
            {
                Tempo = 60000000 / tempoBPM // convert to PPQ
            };
            builder.Build();
            QueueMessage(builder.Result);
        }

        /// <summary>
        /// Adds a MIDI message which sets the time signature
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
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

        /// <summary>
        /// Adds MIDI messages representing the given note (NoteOn and NoteOff)
        /// </summary>
        /// <param name="note"></param>
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

        /// <summary>
        /// Adds a MIDI messages which sets the key signature
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scale"></param>
        public void SetKeySignature(byte key, byte scale)
        {
            MetaMessage message = new MetaMessage(MetaType.KeySignature, new[] { key, scale });
            QueueMessage(message);
        }

        /// <summary>
        /// Finishes the sequence by adding an End of Track meta message (0x2F)
        /// that has been used in this sequence.
        /// </summary>
        public void FinishSequence()
        {
            InsertChannelMessages();
            MetaMessage message = new MetaMessage(MetaType.EndOfTrack, new byte[] { });
            double latestTick = ConvertBeatsToTicks(Enumerable.Range(0, tracks.Length)
                .Select(GetLatestTrackBeatTime)
                .Max());
            for (int i = 0; i < LastCreatedTrackNumber; i++)
            {
                if (tracks[i] != null)
                {
                    tracks[i].Insert((int)latestTick, message);
                }
            }
        }

        /// <summary>
        /// Clears all the tracks and messages
        /// </summary>
        public void Reset()
        {
            messagesToInsert.Clear();
            Sequence = new Sequence((int)MidiDefaults.DefaultDivisionType);
            CreateTrack(0);
        }

        private int ConvertBeatsToTicks(double beats)
        {
            return (int)(ResolutionTicksPerBeat * beats * MidiDefaults.DefaultTempoBeatsPerWhole);
        }

        /*
         * This kind of lazy track creation is necessary because we need to make sure that all NoteOff events
         * will be inserted before all NoteOn events at the same position (Track.Insert inserts at the first
         * available position in the list). 
        */
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

        protected override void CreateTrack(int track)
        {
            base.CreateTrack(track);
            tracks[track] = new Track();
            Sequence.Add(tracks[track]);
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