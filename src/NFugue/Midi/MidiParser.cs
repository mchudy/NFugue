using NFugue.Extensions;
using NFugue.Parsing;
using NFugue.Providers;
using NFugue.Theory;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFugue.Midi
{
    public class MidiParser : Parser
    {
        private List<Dictionary<byte, TempNote>> noteCache;
        private int resolutionTicksPerBeat = MidiDefaults.DefaultResolutionTicksPerBeat;
        private int tempoBPM = MidiDefaults.DefaultTempoBeatsPerMinute;
        private int currentChannel = -1;
        private double[] currentTimeInBeats;
        private double[] expectedTimeInBeats;

        public void Parse(Sequence sequence)
        {
            Start();
            foreach (var track in sequence)
            {
                foreach (var midiEvent in track.Iterator())
                {
                    ParseEvent(midiEvent);
                }
            }
            Stop();
        }

        public void Start()
        {
            OnBeforeParsingStarted();
            InitNoteCache();
            resolutionTicksPerBeat = MidiDefaults.DefaultResolutionTicksPerBeat;
        }

        public void Stop()
        {
            OnAfterParsingFinished();
        }

        public event EventHandler<MidiEventArgs> HandledMidiEvent;
        public event EventHandler<MidiEventArgs> UnhandledMidiEvent;

        public void ParseEvent(MidiEvent @event)
        {
            IMidiMessage message = @event.MidiMessage;
            if (message.MessageType == MessageType.Channel)
            {
                ParseChannelMessage((ChannelMessage)message, @event);
            }
            else if (message.MessageType == MessageType.Meta)
            {
                ParseMetaMessage((MetaMessage)message, @event);
            }
            else if (message.MessageType == MessageType.SystemExclusive)
            {
                ParseSysExMessage((SysExMessage)message, @event);
            }
            else
            {
                UnhandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
            }
        }

        private void ParseChannelMessage(ChannelMessage message, MidiEvent @event)
        {
            // For any message that isn't a NoteOn event, update the current time and channel.
            // (We don't do this for NoteOn events because NoteOn aren't written until the NoteOff event)
            if (!IsNoteOnEvent(message.Command, message.MidiChannel, @event))
            {
                CheckChannel(message.MidiChannel);
            }
            switch (message.Command)
            {
                case ChannelCommand.NoteOff:
                    OnNoteOff(message.MidiChannel, @event);
                    HandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
                    break;
                case ChannelCommand.NoteOn:
                    OnNoteOn(message.MidiChannel, @event);
                    HandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
                    break;
                case ChannelCommand.PolyPressure:
                    OnPolyphonicPressureParsed(message.Data1, message.Data2);
                    HandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
                    break;
                case ChannelCommand.Controller:
                    OnControllerEventParsed(message.Data1, message.Data2);
                    HandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
                    break;
                case ChannelCommand.ProgramChange:
                    OnInstrumentParsed(message.Data1);
                    HandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
                    break;
                case ChannelCommand.ChannelPressure:
                    OnChannelPressureParsed(message.Data1);
                    HandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
                    break;
                case ChannelCommand.PitchWheel:
                    OnPitchWheelParsed(message.Data1, message.Data2);
                    HandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
                    break;
                default:
                    UnhandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
                    break;
            }
        }

        private void ParseMetaMessage(MetaMessage message, MidiEvent @event)
        {
            switch (message.MetaType)
            {
                case MetaType.Lyric:
                    OnLyricParsed(Encoding.ASCII.GetString(message.GetBytes()));
                    HandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
                    break;
                case MetaType.Marker:
                    OnMarkerParsed(Encoding.ASCII.GetString(message.GetBytes()));
                    HandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
                    break;
                case MetaType.Tempo:
                    WhenTempoChanged(message);
                    HandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
                    break;
                case MetaType.TimeSignature:
                    OnTimeSignatureParsed(message.GetBytes()[0], message.GetBytes()[1]);
                    HandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
                    break;
                case MetaType.KeySignature:
                    KeySigParsed(message);
                    HandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
                    break;
                default:
                    UnhandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
                    break;
            }
        }

        private void ParseSysExMessage(IMidiMessage message, MidiEvent @event)
        {
            OnSystemExclusiveParsed(message.GetBytes());
            HandledMidiEvent?.Invoke(this, new MidiEventArgs(@event));
        }

        private bool IsNoteOnEvent(ChannelCommand command, int channel, MidiEvent @event)
        {
            return command == ChannelCommand.NoteOn && noteCache?[channel]?.GetValueOrDefault(@event.MidiMessage.GetBytes()[1]) == null &&
                    (@event.MidiMessage.GetBytes()[2] == 0);
        }

        private bool IsNoteOffEvent(ChannelCommand command, int channel, MidiEvent @event)
        {
            // An event is a NoteOff event if it is actually a NoteOff event, 
            // or if it is a NoteOn event where the note has already been played and the attack velocity is 0. 
            return (command == ChannelCommand.NoteOff) ||
                   ((command == ChannelCommand.NoteOn) &&
                    (noteCache?[channel]?.GetValueOrDefault(@event.MidiMessage.GetBytes()[1]) != null) &&
                    (@event.MidiMessage.GetBytes()[2] == 0));
        }

        private void OnNoteOff(int channel, MidiEvent @event)
        {
            byte note = @event.MidiMessage.GetBytes()[1];
            TempNote tempNote = noteCache?[channel]?[note];
            if (tempNote == null)
            {
                // A note was turned off when that note was never indicated as having been turned on
                return;
            }
            noteCache[channel].Remove(note);
            CheckTime(tempNote.StartTick);

            long durationInTicks = @event.AbsoluteTicks - tempNote.StartTick;
            double durationInBeats = GetDurationInBeats(durationInTicks);
            byte noteOffVelocity = @event.MidiMessage.GetBytes()[2];
            expectedTimeInBeats[currentChannel] = currentTimeInBeats[currentChannel] + durationInBeats;

            Note noteObject = new Note(note)
            {
                Duration = GetDurationInBeats(durationInTicks),
                OnVelocity = tempNote.NoteOnVelocity,
                OffVelocity = noteOffVelocity
            };
            OnNoteReleased(new Note(note) { OffVelocity = noteOffVelocity });
            OnNoteParsed(noteObject);
        }

        private void OnNoteOn(int channel, MidiEvent @event)
        {
            if (IsNoteOffEvent(ChannelCommand.NoteOn, channel, @event))
            {
                // Some MIDI files use the Note On event with 0 velocity to indicate Note Off
                OnNoteOff(channel, @event);
                return;
            }

            byte note = @event.MidiMessage.GetBytes()[1];
            byte noteOnVelocity = @event.MidiMessage.GetBytes()[2];
            if (noteCache?[channel]?.GetValueOrDefault(note) != null)
            {
                // The note already existed in the cache! Nothing to do about it now. This shouldn't happen.
            }
            else
            {
                noteCache?[channel]?.Add(note, new TempNote(@event.AbsoluteTicks, noteOnVelocity));
            }
            OnNotePressed(new Note(note) { OnVelocity = noteOnVelocity });
        }

        private void WhenTempoChanged(MetaMessage meta)
        {
            int newTempoMSPQ = (meta.GetBytes()[2] & 0xFF) |
                ((meta.GetBytes()[1] & 0xFF) << 8) |
                ((meta.GetBytes()[0] & 0xFF) << 16);
            this.tempoBPM = newTempoMSPQ = 60000000 / newTempoMSPQ;
            OnTempoChanged(tempoBPM);
        }

        private void KeySigParsed(MetaMessage meta)
        {
            byte scale = (byte)(meta.GetBytes()[1] == 0 ? ScaleType.Major : ScaleType.Minor);
            OnKeySignatureParsed(KeyProviderFactory.GetKeyProvider().ConvertAccidentalCountToKeyRootPositionInOctave(
                meta.GetBytes()[0], scale), scale);
        }

        private void CheckTime(long tick)
        {
            double newTimeInBeats = GetDurationInBeats(tick);
            if (expectedTimeInBeats[currentChannel] != newTimeInBeats)
            {
                if (newTimeInBeats > expectedTimeInBeats[this.currentChannel])
                {
                    OnNoteParsed(Note.CreateRest(newTimeInBeats - expectedTimeInBeats[currentChannel]));
                }
                else
                {
                    OnTrackBeatTimeRequested(newTimeInBeats);
                }
            }
            currentTimeInBeats[currentChannel] = newTimeInBeats;
        }

        private void CheckChannel(int channel)
        {
            if (currentChannel != channel)
            {
                OnTrackChanged(channel);
                currentChannel = channel;
            }
        }

        private double GetDurationInBeats(long durationInTicks)
        {
            return durationInTicks / (double)resolutionTicksPerBeat / 4.0d;
        }

        private void InitNoteCache()
        {
            noteCache = new List<Dictionary<byte, TempNote>>();
            currentTimeInBeats = new double[MidiDefaults.Tracks];
            expectedTimeInBeats = new double[MidiDefaults.Tracks];

            for (int i = 0; i < MidiDefaults.Tracks; i++)
            {
                noteCache.Add(new Dictionary<byte, TempNote>());
                currentTimeInBeats[i] = 0.0d;
                expectedTimeInBeats[i] = 0.0d;
            }
        }

        internal class TempNote
        {
            public long StartTick { get; }
            public byte NoteOnVelocity { get; }

            public TempNote(long startTick, byte noteOnVelocity)
            {
                StartTick = startTick;
                NoteOnVelocity = noteOnVelocity;
            }
        }
    }
}