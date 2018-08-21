using NFugue.Midi;
using NFugue.Theory;
using System;

namespace NFugue
{
    public static class DefaultNoteSettings
    {
        private static int defaultOctave = 5;
        private static int defaultBassOctave = 4;
        private static int defaultOnVelocity = MidiDefaults.DefaultOnVelocity;
        private static int defaultOffVelocity;

        public static int DefaultOctave
        {
            get => defaultOctave;
            set
            {
                if (value < Note.MinOctave || value > Note.MaxOctave)
                {
                    throw new ArgumentOutOfRangeException(nameof(DefaultOctave));
                }
                defaultOctave = value;
            }
        }

        public static int DefaultBassOctave
        {
            get => defaultBassOctave;
            set
            {
                if (value < Note.MinOctave || value > Note.MaxOctave)
                {
                    throw new ArgumentOutOfRangeException(nameof(DefaultBassOctave));
                }
                defaultBassOctave = value;
            }
        }

        public static int DefaultOnVelocity
        {
            get => defaultOnVelocity;
            set
            {
                if (value < MidiDefaults.MinOnVelocity || value > MidiDefaults.MaxOnVelocity)
                {
                    throw new ArgumentOutOfRangeException(nameof(DefaultOnVelocity));
                }
                defaultOnVelocity = value;
            }
        }

        public static int DefaultOffVelocity
        {
            get => defaultOffVelocity;
            set
            {
                if (value < MidiDefaults.MinOnVelocity || value > MidiDefaults.MaxOnVelocity)
                {
                    throw new ArgumentOutOfRangeException(nameof(DefaultOffVelocity));
                }
                defaultOffVelocity = value;
            }
        }

        public static bool AdjustNotesByKeySignature { get; set; } = true;
        public static double DefaultDuration { get; set; } = 0.25;
    }
}
