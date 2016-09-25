using NLog;
using Sanford.Multimedia.Midi;
using System;

namespace NFugue.Playing
{
    public class ManagedPlayer : IDisposable
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly Sequencer sequencer = new Sequencer();
        private readonly OutputDevice outputDevice;

        public ManagedPlayer()
        {
            outputDevice = new OutputDevice(0);
            sequencer.PlayingCompleted += (s, e) => Finish();
            SubscribeMessageEvents();
        }

        public bool IsStarted { get; private set; }
        public bool IsFinished { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsPlaying { get; private set; }

        public event EventHandler<SequenceEventArgs> Started;
        public event EventHandler Finished;
        public event EventHandler Paused;
        public event EventHandler Resumed;
        public event EventHandler Sought;
        public event EventHandler OnReset;

        public void Start(Sequence sequence)
        {
            sequencer.Sequence = sequence;
            log.Trace($"Playing sequence with division {sequence.Division}");
            foreach (var track in sequence)
            {
                foreach (var msg in track.Iterator())
                {
                    log.Trace($"absolute ticks: {msg.AbsoluteTicks} {msg.MidiMessage.MessageType} {msg.MidiMessage.GetBytes()} {(msg.MidiMessage as ChannelMessage)?.Command}");
                }
            }
            IsPlaying = true;
            IsFinished = false;
            IsPaused = false;
            IsStarted = true;
            sequencer.Start();
            Started?.Invoke(this, new SequenceEventArgs { Sequence = sequence });
        }

        public void Pause()
        {
            sequencer.Stop();
            IsPaused = true;
            Paused?.Invoke(this, EventArgs.Empty);
        }

        public void Resume()
        {
            sequencer.Start();
            IsPaused = false;
            Resumed?.Invoke(this, EventArgs.Empty);
        }

        public void Seek(int tick)
        {
            sequencer.Position = tick;
            Sought?.Invoke(this, EventArgs.Empty);
        }

        public void Reset()
        {
            IsStarted = false;
            IsPaused = false;
            IsFinished = false;
            OnReset?.Invoke(this, EventArgs.Empty);
        }

        public void Finish()
        {
            log.Trace("Playing completed. Closing device...");
            Finished?.Invoke(this, EventArgs.Empty);
            IsFinished = true;
        }

        private void SubscribeMessageEvents()
        {
            sequencer.ChannelMessagePlayed += (s, e) =>
            {
                log.Trace($"Channel message played: {e.Message.MessageType.ToString()} " +
                          $"command:{e.Message.Command} data1: {e.Message.Data1} data2: {e.Message.Data2}");
                outputDevice?.Send(e.Message);
            };
            sequencer.Chased += (s, e) =>
            {
                foreach (ChannelMessage message in e.Messages)
                {
                    log.Trace($"Message chased: {message.MessageType.ToString()} " +
                              $"command:{message.Command} data1: {message.Data1} data2: {message.Data2}");
                    outputDevice?.Send(message);
                }
            };
            sequencer.SysExMessagePlayed += (s, e) =>
            {
                log.Trace($"SysEx message played: {e.Message.MessageType.ToString()} " +
                          $"status:{e.Message.Status.ToString()} data: {e.Message.GetBytes()}");
                outputDevice?.Send(e.Message);
            };
        }

        public void Dispose()
        {
            outputDevice.Dispose();
        }
    }

    public class SequenceEventArgs : EventArgs
    {
        public Sequence Sequence { get; set; }
    }
}