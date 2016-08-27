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
        private sbyte value;
        private double duration;

        public Note()
        {
            OnVelocity = DefaultNoteSettings.DefaultOnVelocity;
            OffVelocity = DefaultNoteSettings.DefaultOffVelocity;
        }

        public Note(string note)
        {
            NoteProviderFactory.GetNoteProvider().CreateNote(note);
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
            Value = (sbyte)value;
            IsOctaveExplicitlySet = true;
            duration = DefaultNoteSettings.DefaultDuration;
        }

        public Note(int value, double duration) : this()
        {
            this.value = (sbyte)value;
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


        public sbyte OnVelocity { get; set; }
        public sbyte OffVelocity { get; set; }

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

        public sbyte Value
        {
            get
            {
                return IsRest ? (sbyte)0 : value;
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

        public sbyte GetOctave()
        {
            return IsRest ? (sbyte)0 : (sbyte)(value / Octave);
        }

        public sbyte PositionInOctave() => IsRest ? (sbyte)0 : (sbyte)(Value % Octave);


        public string GetToneString()
        {
            if (IsRest)
            {
                return "R";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(GetToneStringWithoutOctave(Value));
            if (IsOctaveExplicitlySet)
            {
                sb.Append(GetOctave());
            }
            return sb.ToString();
        }

        public static string GetToneString(sbyte noteValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetToneStringWithoutOctave(noteValue));
            sb.Append(noteValue / Octave);
            return sb.ToString();
        }

        public static string GetToneStringWithoutOctave(sbyte noteValue)
        {
            return NoteNamesCommon[noteValue % Octave];
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

        public static string DispositionedToneStringWithoutOctave(int dispose, sbyte noteValue)
        {
            if (dispose == -1)
            {
                return NoteNamesFlat[noteValue % Octave];
            }
            return NoteNamesSharp[noteValue % Octave];
        }

        public static string PercussionString(sbyte noteValue)
        {
            StringBuilder buddy = new StringBuilder();
            buddy.Append("[");
            buddy.Append(PercussionNames[noteValue - 35]);
            buddy.Append("]");
            return buddy.ToString();
        }

        public static double FrequencyForNote(string note)
        {
            return note.ToUpper().StartsWith("R") ? 0.0d :
                FrequencyForNote(NoteProviderFactory.GetNoteProvider().CreateNote(note).Value.ToString());
        }

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
                default: return "/" + (1.0 / (double)beat);
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

        public string ToneString()
        {
            if (IsRest)
            {
                return "R";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(GetToneStringWithoutOctave(Value));
            if (IsOctaveExplicitlySet)
            {
                sb.Append(GetOctave());
            }
            return sb.ToString();
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

        //TODO: drop in favour of OrderBy?
        public static void SortNotesBy(Note[] notes, Func<Note, int> callback)
        {
            for (int i = 0; i < notes.Length - 1; i++)
            {
                for (int j = 1; j < notes.Length - i; j++)
                {
                    if (callback(notes[j - 1]) > callback(notes[j]))
                    {
                        var temp = notes[j - 1];
                        notes[j - 1] = notes[j];
                        notes[j] = temp;
                    }
                }
            }
        }
        public static readonly Note Rest = new Note(0) { IsRest = true };
        public static readonly string[] NoteNamesCommon = { "C", "C#", "D", "Eb", "E", "F", "F#", "G", "G#", "A", "Bb", "B" };
        public static readonly string[] NoteNamesSharp = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        public static readonly string[] NoteNamesFlat = { "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B" };

        public static readonly sbyte Octave = 12;
        public static readonly sbyte MinOctave = 0;
        public static readonly sbyte MaxOctave = 10;

        //TODO: enum?
        public static readonly string[] PercussionNames = {
    	    // Percussion Name		// MIDI Note Value
    	    "ACOUSTIC_BASS_DRUM", 	//       35
    	    "BASS_DRUM", 			//       36
    	    "SIDE_STICK", 			//       37
    	    "ACOUSTIC_SNARE",		//       38
    	    "HAND_CLAP", 			//       39
    	    "ELECTRIC_SNARE", 		//       40
    	    "LO_FLOOR_TOM", 		//       41
    	    "CLOSED_HI_HAT",		//       42
    	    "HIGH_FLOOR_TOM", 		//       43
    	    "PEDAL_HI_HAT", 		//       44
    	    "LO_TOM", 				//       45
    	    "OPEN_HI_HAT", 		    //       46
    	    "LO_MID_TOM", 			//       47
    	    "HI_MID_TOM", 			//       48
    	    "CRASH_CYMBAL_1", 		//       49
    	    "HI_TOM",				//       50
    	    "RIDE_CYMBAL_1", 		//       51
    	    "CHINESE_CYMBAL", 		//       52
    	    "RIDE_BELL", 			//       53
    	    "TAMBOURINE",			//       54
    	    "SPLASH_CYMBAL", 		//       55
    	    "COWBELL", 				//       56
    	    "CRASH_CYMBAL_2", 		//       57
    	    "VIBRASLAP",			//       58
    	    "RIDE_CYMBAL_2", 		//       59
    	    "HI_BONGO", 			//       60
    	    "LO_BONGO", 			//       61
    	    "MUTE_HI_CONGA",		//       62
    	    "OPEN_HI_CONGA", 		//       63
    	    "LO_CONGA", 			//       64
    	    "HI_TIMBALE", 			//       65
    	    "LO_TIMBALE",			//       66
    	    "HI_AGOGO", 			//       67
    	    "LO_AGOGO", 			//       68
    	    "CABASA", 				//       69
    	    "MARACAS", 				//       70
    	    "SHORT_WHISTLE", 		//       71
    	    "LONG_WHISTLE", 		//       72
    	    "SHORT_GUIRO", 			//       73
    	    "LONG_GUIRO",			//       74
    	    "CLAVES", 				//       75
    	    "HI_WOOD_BLOCK", 		//       76
    	    "LO_WOOD_BLOCK", 		//       77
    	    "MUTE_CUICA",			//       78
    	    "OPEN_CUICA", 			//       79
    	    "MUTE_TRIANGLE", 		//       80
    	    "OPEN_TRIANGLE"			//       81
        };

        #region Equality members

        public bool Equals(Note other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            bool originalStringsMatchSufficientlyWell = ((other.OriginalString == null) || (OriginalString == null)) ||
                other.OriginalString.Equals(OriginalString, StringComparison.OrdinalIgnoreCase);
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