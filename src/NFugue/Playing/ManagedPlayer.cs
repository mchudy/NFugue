using Sanford.Multimedia.Midi;
using System;

namespace NFugue.Playing
{
    public class ManagedPlayer
    {
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
            Sequencer sequencer = new Sequencer();
            sequencer.Sequence = sequence;
            sequencer.Start();
            //sequencer.MetaMessagePlayed += (s, e) =>
            //{
            //    if (e.Message.MetaType == MetaType.EndOfTrack)
            //    {
            //        this.IsFinished = true;
            //        sequencer.Stop();
            //    }
            //};
            sequencer.Stopped += (s, e) => IsFinished = true;
        }
    }

    public class SequenceEventArgs : EventArgs
    {
        public Sequence Sequence { get; set; }
    }
}