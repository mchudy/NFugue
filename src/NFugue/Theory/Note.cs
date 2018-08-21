using NFugue.Extensions;
using NFugue.Midi;
using NFugue.Patterns;
using NFugue.Providers;
using NFugue.Staccato.Subparsers.NoteSubparser;
using System;
using System.Text;

namespace NFugue.Theory
{
    public class Note : IEquatable<Note>, IPatternProducer
    {
        private int value;
        private double duration;

        public Note()
        {
            OnVelocity = DefaultNoteSettings.DefaultOnVelocity;
            OffVelocity = DefaultNoteSettings.DefaultOffVelocity;
        }

        public Note(string note)
            : this(NoteProviderFactory.GetNoteProvider().CreateNote(note))
        {
        }

        public Note(Note note)
        {
            value = note.value;
            duration = note.duration;
            IsDurationExplicitlySet = note.IsDurationExplicitlySet;
            IsOctaveExplicitlySet = note.IsOctaveExplicitlySet;
            OnVelocity = note.OnVelocity;
            OffVelocity = note.OffVelocity;
            IsRest = note.IsRest;
            IsStartOfTie = note.IsStartOfTie;
            IsEndOfTie = note.IsEndOfTie;
            IsFirstNote = note.IsFirstNote;
            IsMelodicNote = note.IsMelodicNote;
            IsHarmonicNote = note.IsHarmonicNote;
            IsPercussionNote = note.IsPercussionNote;
            OriginalString = note.OriginalString;
        }

        public Note(int value) : this()
        {
            Value = value;
            IsOctaveExplicitlySet = false;
            duration = DefaultNoteSettings.DefaultDuration;
        }

        public Note(int value, double duration) : this()
        {
            this.value = value;
            Duration = duration;
        }

        public double Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                IsDurationExplicitlySet = true;
            }
        }


        public Note UseSameDurationAs(Note note2)
        {
            duration = note2.duration;
            IsDurationExplicitlySet = note2.IsDurationExplicitlySet;
            return this;
        }

        public void UseDefaultDuration()
        {
            duration = DefaultNoteSettings.DefaultDuration;
        }

        public Note UseSameExplicitOctaveSettingAs(Note note2)
        {
            IsOctaveExplicitlySet = note2.IsOctaveExplicitlySet;
            return this;
        }


        public int OnVelocity { get; set; }
        public int OffVelocity { get; set; }

        public bool IsRest { get; set; }
        public bool IsStartOfTie { get; set; }
        public bool IsEndOfTie { get; set; }

        public bool IsFirstNote { get; set; } = true;
        public bool IsMelodicNote { get; set; }
        public bool IsHarmonicNote { get; set; }
        public bool IsPercussionNote { get; set; }

        public bool IsDurationExplicitlySet { get; private set; }
        public bool IsOctaveExplicitlySet { get; set; }

        public string OriginalString { get; set; }

        public int Value
        {
            get
            {
                return IsRest ? 0 : value;
            }
            set { this.value = value; }
        }

        public static Note CreateRest(double duration)
        {
            return new Note
            {
                IsRest = true,
                Duration = duration
            };
        }

        public double MicrosecondDuration(double mpq) => duration * 4.0f * mpq;

        public int Octave => (IsRest ? 0 : (value / SemitonesInOctave));

        public int PositionInOctave => (IsRest ? 0 : (Value % SemitonesInOctave));

        public string GetToneString()
        {
            if (IsRest)
            {
                return "R";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(ToneStringWithoutOctave(Value));
            if (IsOctaveExplicitlySet)
            {
                sb.Append(Octave);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a MusicString representation of the given MIDI note value,
        /// which indicates a note and an octave.
        /// </summary>
        public static string GetToneString(int noteValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ToneStringWithoutOctave(noteValue));
            sb.Append(noteValue / SemitonesInOctave);
            return sb.ToString();
        }

        /// <summary>
        /// Returns a MusicString representation of the given MIDI note value,
        /// but just the note - not the octave.This means that the value returned
        /// can not be used to accurately recalculate the noteValue, since information
        /// will be missing.But this is useful for knowing what note within any octave
        /// the corresponding value belongs to.
        /// </summary>
        /// <param name="noteValue">MIDI numeric note value</param>
        /// <returns>Music string</returns>
        public static string ToneStringWithoutOctave(int noteValue)
        {
            return NoteNamesCommon[noteValue % SemitonesInOctave];
        }

        public static bool IsSameNote(string note1, string note2)
        {
            if (note1.Equals(note2, StringComparison.OrdinalIgnoreCase)) return true;
            for (int i = 0; i < NoteNamesCommon.Length; i++)
            {
                if (note1.Equals(NoteNamesFlat[i], StringComparison.OrdinalIgnoreCase) &&
                    note2.Equals(NoteNamesSharp[i], StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (note1.Equals(NoteNamesSharp[i], StringComparison.OrdinalIgnoreCase) &&
                    note2.Equals(NoteNamesFlat[i], StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a MusicString representation of the given MIDI note value,
        /// just the note(not the octave), disposed to use either flats or sharps.
        /// Pass -1 to get a flat name and +1 to get a sharp name for any notes
        /// that are accidentals.
        /// </summary>
        /// <param name="dispose">-1 to get a flat value, +1 to get a sharp value</param>
        /// <param name="noteValue">The MIDI note value</param>
        /// <returns>Music string representation of the note</returns>
        public static string DispositionedToneStringWithoutOctave(int dispose, int noteValue)
        {
            if (dispose == -1)
            {
                return NoteNamesFlat[noteValue % SemitonesInOctave];
            }
            return NoteNamesSharp[noteValue % SemitonesInOctave];
        }

        /// <summary>
        /// Returns a music string representations of the given note using the
        /// name of a percussion instrument
        /// </summary>
        /// <param name="noteValue">The MIDI note value</param>
        /// <returns>Music string representation of the note</returns>
        public static string PercussionString(int noteValue)
        {
            StringBuilder buddy = new StringBuilder();
            buddy.Append("[");
            buddy.Append(((PercussionInstrument)noteValue).GetDescription());
            buddy.Append("]");
            return buddy.ToString();
        }

        /// <summary>
        /// Return the frequency in Hertz for the given note.
        /// </summary>
        /// <param name="note">Music string representing the note</param>
        /// <returns>The note frequency in Hz</returns>
        public static double FrequencyForNote(string note)
        {
            return note.ToUpper().StartsWith("R") ? 0.0d :
                FrequencyForNote(NoteProviderFactory.GetNoteProvider().CreateNote(note).Value);
        }

        /// <summary>
        /// Return the frequency in Hertz for the given note.
        /// </summary>
        /// <param name="noteValue">MIDI numeric note value</param>
        /// <returns>The note frequency in Hz</returns>
        public static double FrequencyForNote(int noteValue)
        {
            return PreciseFrequencyForNote(noteValue).Truncate(3);
        }

        private static double PreciseFrequencyForNote(int noteValue)
        {
            return FrequencyAboveBase(8.1757989156, noteValue / 12.0);
        }

        private static double FrequencyAboveBase(double baseFrequency, double octavesAboveBase)
        {
            return baseFrequency * Math.Pow(2.0, octavesAboveBase);
        }

        public static bool IsValidNote(string candidateNote)
        {
            return NoteSubparser.Instance.Matches(candidateNote);
        }

        public static bool IsValidQualifier(string candidateQualifier) => true;

        /// <summary>
        /// Returns a MusicString representation of a decimal duration.  This code
        /// currently only converts single duration values representing whole, half,
        /// quarter, eighth, etc. durations; and dotted durations associated with those
        /// durations(such as "h.", equal to 0.75).  This method does not convert
        /// combined durations(for example, "hi" for 0.625). For these values,
        /// the original decimal duration is returned in a string, prepended with a "/"
        /// to make the returned value a valid MusicString duration indicator.
        /// It does handle durations greater than 1.0 (for example, "wwww" for 4.0).  
        /// </summary>
        /// <param name="decimalDuration">The decimal value of the duration to convert</param>
        /// <returns>A MusicString fragment representing the duration</returns>
        public static string DurationString(double decimalDuration)
        {
            double originalDecimalDuration = decimalDuration;
            StringBuilder sb = new StringBuilder();
            if (decimalDuration >= 1.0)
            {
                int numWholeDurations = (int)Math.Floor(decimalDuration);
                sb.Append("w");
                if (numWholeDurations > 1)
                {
                    sb.Append(numWholeDurations);
                }
                decimalDuration -= numWholeDurations;
            }
            //TODO: extract string constants to a static class
            if (decimalDuration == 0.75) sb.Append("h.");
            else if (decimalDuration == 0.5) sb.Append("h");
            else if (decimalDuration == 0.375) sb.Append("q.");
            else if (decimalDuration == 0.25) sb.Append("q");
            else if (decimalDuration == 0.1875) sb.Append("i.");
            else if (decimalDuration == 0.125) sb.Append("i");
            else if (decimalDuration == 0.09375) sb.Append("s.");
            else if (decimalDuration == 0.0625) sb.Append("s");
            else if (decimalDuration == 0.046875) sb.Append("t.");
            else if (decimalDuration == 0.03125) sb.Append("t");
            else if (decimalDuration == 0.0234375) sb.Append("x.");
            else if (decimalDuration == 0.015625) sb.Append("x");
            else if (decimalDuration == 0.01171875) sb.Append("o.");
            else if (decimalDuration == 0.0078125) sb.Append("o");
            else if (decimalDuration == 0.0) { }
            else
            {
                return "/" + originalDecimalDuration;
            }
            return sb.ToString();
        }

        public static string DurationStringForBeat(int beat)
        {
            switch (beat)
            {
                case 2: return "h";
                case 4: return "q";
                case 8: return "i";
                case 16: return "s";
                default: return "/" + (1.0 / beat);
            }
        }

        public string VelocityString()
        {
            StringBuilder buddy = new StringBuilder();
            if (OnVelocity != DefaultNoteSettings.DefaultOnVelocity)
            {
                buddy.Append("a" + OnVelocity);
            }
            if (OffVelocity != DefaultNoteSettings.DefaultOffVelocity)
            {
                buddy.Append("d" + OffVelocity);
            }
            return buddy.ToString();
        }

        public Pattern GetPattern()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ToStringWithoutDuration());
            sb.Append(DecoratorString());
            return new Pattern(sb.ToString());
        }

        public Pattern GetPercussionPattern()
        {
            if (Value < MidiDefaults.MinPercussionNote || Value > MidiDefaults.MaxPercussionNote) return GetPattern();
            StringBuilder buddy = new StringBuilder();
            buddy.Append(PercussionString(Value));
            buddy.Append(DecoratorString());
            return new Pattern(buddy.ToString());
        }

        public override string ToString()
        {
            return GetPattern().ToString();
        }

        public string ToStringWithoutDuration()
        {
            if (IsRest)
            {
                return "R";
            }
            if (IsPercussionNote)
            {
                return PercussionString(Value);
            }
            return OriginalString ?? GetToneString(Value);
        }

        public string ToneString
        {
            get
            {
                if (IsRest)
                {
                    return "R";
                }
                StringBuilder sb = new StringBuilder();
                sb.Append(ToneStringWithoutOctave(Value));
                if (IsOctaveExplicitlySet)
                {
                    sb.Append(Octave);
                }
                return sb.ToString();
            }
        }

        public string DecoratorString()
        {
            StringBuilder sb = new StringBuilder();
            if (IsDurationExplicitlySet)
            {
                sb.Append(DurationString(duration));
            }
            sb.Append(VelocityString());
            return sb.ToString();
        }

        public static readonly Note Rest = new Note(0) { IsRest = true };
        public static readonly string[] NoteNamesCommon = { "C", "C#", "D", "Eb", "E", "F", "F#", "G", "G#", "A", "Bb", "B" };
        public static readonly string[] NoteNamesSharp = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        public static readonly string[] NoteNamesFlat = { "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B" };

        public static readonly int SemitonesInOctave = 12;
        public static readonly int MinOctave = 0;
        public static readonly int MaxOctave = 10;

        #region Equality members

        public bool Equals(Note other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            bool originalStringsMatchSufficientlyWell = (other.OriginalString == null) || (OriginalString == null) || other.OriginalString.Equals(OriginalString, StringComparison.OrdinalIgnoreCase);
            return value == other.value && duration.Equals(other.duration) &&
                   OnVelocity == other.OnVelocity &&
                   OffVelocity == other.OffVelocity &&
                   IsPercussionNote == other.IsPercussionNote &&
                   IsRest == other.IsRest &&
                   IsStartOfTie == other.IsStartOfTie &&
                   IsEndOfTie == other.IsEndOfTie &&
                   IsFirstNote == other.IsFirstNote &&
                   IsMelodicNote == other.IsMelodicNote &&
                   IsHarmonicNote == other.IsHarmonicNote &&
                   IsDurationExplicitlySet == other.IsDurationExplicitlySet &&
                   IsOctaveExplicitlySet == other.IsOctaveExplicitlySet &&
                   originalStringsMatchSufficientlyWell;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Note)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = value.GetHashCode();
                hashCode = (hashCode * 397) ^ duration.GetHashCode();
                hashCode = (hashCode * 397) ^ OnVelocity.GetHashCode();
                hashCode = (hashCode * 397) ^ OffVelocity.GetHashCode();
                hashCode = (hashCode * 397) ^ IsPercussionNote.GetHashCode();
                hashCode = (hashCode * 397) ^ IsRest.GetHashCode();
                hashCode = (hashCode * 397) ^ IsStartOfTie.GetHashCode();
                hashCode = (hashCode * 397) ^ IsEndOfTie.GetHashCode();
                hashCode = (hashCode * 397) ^ IsFirstNote.GetHashCode();
                hashCode = (hashCode * 397) ^ IsMelodicNote.GetHashCode();
                hashCode = (hashCode * 397) ^ IsHarmonicNote.GetHashCode();
                hashCode = (hashCode * 397) ^ IsDurationExplicitlySet.GetHashCode();
                hashCode = (hashCode * 397) ^ IsOctaveExplicitlySet.GetHashCode();
                hashCode = (hashCode * 397) ^ OriginalString.GetHashCode();
                return hashCode;
            }
        }
        #endregion
    }
}