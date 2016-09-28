using System;
using Sanford.Multimedia.Midi;

namespace NFugue.Midi
{
    public class MidiEventArgs : EventArgs
    {
        public MidiEventArgs(MidiEvent midiEvent)
        {
            MidiEvent = midiEvent;
        }

        public MidiEvent MidiEvent { get; set; }
    }
}