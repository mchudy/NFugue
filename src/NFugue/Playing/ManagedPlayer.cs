using NLog;
using Sanford.Multimedia.Midi;
using System;
#pragma warning disable 67

namespace NFugue.Playing
{
    public class ManagedPlayer
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public event EventHandler<SequenceEventArgs> Started;
        public event EventHandler Finished;
        public event EventHandler Paused;
        public event EventHandler Resumed;
        public event EventHandler Seek;
        public event EventHandler Reset;

        public bool IsStarted { get; private set; }
        public bool IsFinished { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsPlaying { get; private set; }

        public void Start(Sequence sequence)
        {
            OutputDevice outputDevice = new OutputDevice(0);
            Sequencer sequencer = new Sequencer { Sequence = sequence };
            log.Trace($"Playing sequence with division {sequence.Division}");
            sequencer.ChannelMessagePlayed += (s, e) =>
            {
                log.Trace($"Channel message played: {e.Message.MessageType.ToString()} " +
                          $"command:{e.Message.Command} data1: {e.Message.Data1} data2: {e.Message.Data2}");
                outputDevice.Send(e.Message);
            };
            sequencer.Chased += (s, e) =>
            {
                foreach (ChannelMessage message in e.Messages)
                {
                    log.Trace($"Message chased: {message.MessageType.ToString()} " +
                        $"command:{message.Command} data1: {message.Data1} data2: {message.Data2}");
                    outputDevice.Send(message);
                }
            };
            sequencer.SysExMessagePlayed += (s, e) =>
            {
                log.Trace($"SysEx message played: {e.Message.MessageType.ToString()} " +
                        $"status:{e.Message.Status.ToString()} data: {e.Message.GetBytes()}");
                outputDevice.Send(e.Message);
            };
            sequencer.PlayingCompleted += (s, e) =>
            {
                log.Trace("Playing completed. Closing device...");
                IsFinished = true;
                IsPlaying = false;
                outputDevice.Close();
            };
            IsPlaying = true;
            sequencer.Start();
        }
    }

    public class SequenceEventArgs : EventArgs
    {
        public Sequence Sequence { get; set; }
    }
}