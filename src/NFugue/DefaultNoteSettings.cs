using NFugue.Theory;
using System;

namespace NFugue
{
    public static class DefaultNoteSettings
    {
        private static sbyte defaultOctave = 5;
        private static sbyte defaultBassOctave = 4;
        private static sbyte defaultOnVelocity = MidiDefaults.DefaultOnVelocity;
        private static sbyte defaultOffVelocity;

        public static sbyte DefaultOctave
        {
            get { return defaultOctave; }
            set
            {
                if (value < Note.MinOctave || value > Note.MaxOctave)
                {
                    throw new ArgumentOutOfRangeException("Octave out of range");
                }
                defaultOctave = value;
            }
        }

        public static sbyte DefaultBassOctave
        {
            get { return defaultBassOctave; }
            set
            {
                if (value < Note.MinOctave || value > Note.MaxOctave)
                {
                    throw new ArgumentOutOfRangeException("Octave out of range");
                }
                defaultBassOctave = value;
            }
        }

        public static sbyte DefaultOnVelocity
        {
            get { return defaultOnVelocity; }
            set
            {
                if (value < MidiDefaults.MinOnVelocity || value > MidiDefaults.MaxOnVelocity)
                {
                    throw new ArgumentOutOfRangeException("On velocity out of range");
                }
                defaultOnVelocity = value;
            }
        }

        public static sbyte DefaultOffVelocity
        {
            get { return defaultOffVelocity; }
            set
            {
                if (value < MidiDefaults.MinOnVelocity || value > MidiDefaults.MaxOnVelocity)
                {
                    throw new ArgumentOutOfRangeException("Off velocity out of range");
                }
                defaultOffVelocity = value;
            }
        }

        public static bool DefaultAdjustNotesByKeySignature { get; set; } = true;
        public static double DefaultDuration { get; set; } = 0.25;
    }
}
