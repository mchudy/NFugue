using NFugue.Patterns;
using NFugue.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFugue.Theory
{
    public class Chord : IPatternProducer
    {
        static Chord()
        {
            ChordMap = new SortedDictionary<string, Intervals>(Comparer<string>.Create((s1, s2) =>
            {
                int result = CompareLength(s1, s2);
                if (result == 0)
                {
                    result = string.Compare(s1, s2, StringComparison.Ordinal);
                }
                return result;
            }))
            {
                // Major Chords
                ["MAJ"] = new Intervals("1 3 5"),
                ["MAJ6"] = new Intervals("1 3 5 6"),
                ["MAJ7"] = new Intervals("1 3 5 7"),
                ["MAJ9"] = new Intervals("1 3 5 7 9"),
                ["ADD9"] = new Intervals("1 3 5 9"),
                ["MAJ6%9"] = new Intervals("1 3 5 6 9"),
                ["MAJ7%6"] = new Intervals("1 3 5 6 7"),
                ["MAJ13"] = new Intervals("1 3 5 7 9 13"),
                // Minor Chords
                ["MIN"] = new Intervals("1 b3 5"),
                ["MIN6"] = new Intervals("1 b3 5 6"),
                ["MIN7"] = new Intervals("1 b3 5 b7"),
                ["MIN9"] = new Intervals("1 b3 5 b7 9"),
                ["MIN11"] = new Intervals("1 b3 5 b7 9 11"),
                ["MIN7%11"] = new Intervals("1 b3 5 b7 11"),
                ["MINADD9"] = new Intervals("1 b3 5 9"),
                ["MIN6%9"] = new Intervals("1 b3 5 6"),
                ["MINMAJ7"] = new Intervals("1 b3 5 7"),
                ["MINMAJ9"] = new Intervals("1 b3 5 7 9"),
                //Dominant Chords
                ["DOM7"] = new Intervals("1 3 5 b7"),
                ["DOM7%6"] = new Intervals("1 3 5 6 b7"),
                ["DOM7%11"] = new Intervals("1 3 5 b7 11"),
                ["DOM7SUS"] = new Intervals("1 4 5 b7"),
                ["DOM7%6SUS"] = new Intervals("1 4 5 6 b7"),
                ["DOM9"] = new Intervals("1 3 5 b7 9"),
                ["DOM11"] = new Intervals("1 3 5 b7 9 11"),
                ["DOM13"] = new Intervals("1 3 5 b7 9 13"),
                ["DOM13SUS"] = new Intervals("1 3 5 b7 11 13"),
                ["DOM7%6%11"] = new Intervals("1 3 5 b7 9 11 13"),
                // Augmented chords
                ["AUG"] = new Intervals("1 3 #5"),
                ["AUG7"] = new Intervals("1 3 #5 b7"),
                // Diminished chords
                ["DIM"] = new Intervals("1 b3 b5"),
                ["DIM7"] = new Intervals("1 b3 b5 6"),
                // Suspended Chords
                ["SUS4"] = new Intervals("1 4 5"),
                ["SUS2"] = new Intervals("1 2 5")
            };
            // Human readable names for some of the more cryptic chord strings
            humanReadableMap = new Dictionary<string, string>
            {
                ["MAJ6%9"] = "6/9",
                ["MAJ7%6"] = "7/6"
            };
        }

        private readonly Intervals intervals;

        public Chord(string s)
            : this(ChordProviderFactory.GetChordProvider().CreateChord(s))
        {
        }

        public Chord(Chord chord)
        {
            Root = chord.Root;
            intervals = chord.GetIntervals();
            Inversion = chord.Inversion;
        }

        public Chord(Note root, Intervals intervals)
        {
            Root = root;
            this.intervals = intervals;
        }

        public Chord(Key key)
        {
            Root = key.Root;
            intervals = key.Scale.Intervals;
        }

        public Note Root { get; set; }
        public int Inversion { get; set; }
        public bool IsMinor => intervals.Equals(MINOR_INTERVALS);
        public bool IsMajor => intervals.Equals(MAJOR_INTERVALS);

        public static IDictionary<string, Intervals> ChordMap;
        public static IDictionary<string, string> humanReadableMap;

        public static bool IsValid(string candidateChordMusicString)
        {
            string musicString = candidateChordMusicString.ToUpper();
            foreach (string chordName in ChordMap.Keys)
            {
                if (musicString.Contains(chordName))
                {
                    int index = musicString.IndexOf(chordName);
                    string possibleNote = musicString.Substring(0, index);
                    string qualifiers = musicString.Substring(index + chordName.Length - 1, musicString.Length - index - chordName.Length);
                    if (Note.IsValidNote(possibleNote) && Note.IsValidQualifier(qualifiers))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static int CompareLength(string s1, string s2)
        {
            if (s1.Length < s2.Length)
            {
                return 1;
            }
            if (s1.Length > s2.Length)
            {
                return -1;
            }
            return 0;
        }

        public static string[] GetChordNames()
        {
            return ChordMap.Keys.ToArray();
        }

        public static void AddChord(string name, string intervalPattern)
        {
            AddChord(name, new Intervals(intervalPattern));
        }

        public static void AddChord(string name, Intervals intervalPattern)
        {
            ChordMap.Add(name, intervalPattern);
        }

        public static Intervals GetIntervals(string name)
        {
            return ChordMap[name];
        }

        public static void RemoveChord(string name)
        {
            ChordMap.Remove(name);
        }

        public static string GetChordType(Intervals intervals)
        {
            foreach (var entry in ChordMap)
            {
                if (intervals.Equals(entry.Value))
                {
                    return entry.Key;
                }
            }
            return null;
        }

        public static void PutHumanReadable(string chordName, string humanReadableName)
        {
            humanReadableMap.Add(chordName, humanReadableName);
        }

        public static string GetHumanReadableName(string chordName)
        {
            if (humanReadableMap.ContainsKey(chordName))
            {
                return humanReadableMap[chordName];
            }
            return chordName;
        }

        public static Chord FromNotes(string noteString)
        {
            return FromNotes(noteString.Split(' '));
        }

        public static Chord FromNotes(string[] noteStrings)
        {
            var notes = new List<Note>();
            foreach (var noteString in noteStrings)
            {
                notes.Add(new Note(noteString));
            }
            return FromNotes(notes.ToArray());
        }

        public static Chord FromNotes(Note[] notes)
        {
            return new Chord(GetChordFromNotes(notes));
        }

        private static Note[] FlattenNotesByPositionInOctave(Note[] notes)
        {
            var noteMap = new Dictionary<int, Note>();
            var noteOrder = new List<int>();
            foreach (var note in notes)
            {
                int positionInOctave = note.PositionInOctave;
                if (!noteMap.ContainsKey(positionInOctave))
                {
                    noteMap.Add(positionInOctave, note);
                    noteOrder.Add(positionInOctave);
                }
            }

            Note[] retVal = new Note[noteMap.Count];
            int counter = 0;
            foreach (var positionInOctave in noteOrder)
            {
                retVal[counter++] = noteMap[positionInOctave];
            }
            return retVal;
        }

        private static string GetChordFromNotes(Note[] notes)
        {
            bool returnNonOctaveNotes = false;

            // Sorting notes by their value will let us know which is the bass note
            notes = notes.OrderBy(n => n.Value).ToArray();

            // If the distance between the lowest note and the highest note is greater than 12, 
            // we have a chord that spans octaves and we should return a chord in which the
            // notes have no octave.
            if (notes[notes.Length - 1].Value - notes[0].Value > Note.SemitonesInOctave)
            {
                returnNonOctaveNotes = true;
            }
            Note bassNote = notes[0];

            // Sorting notes by position in octave will let us know which chord we have
            notes = notes.OrderBy(n => n.PositionInOctave).ToArray();
            notes = FlattenNotesByPositionInOctave(notes);

            string[] possibleChords = new string[notes.Length];
            for (int i = 0; i < notes.Length; i++)
            {
                Note[] notesToCheck = new Note[notes.Length];
                for (int u = 0; u < notes.Length; u++)
                {
                    notesToCheck[u] = notes[(i + u) % notes.Length];
                }
                possibleChords[i] = GetChordType(Intervals.CreateIntervalsFromNotes(notesToCheck));
            }

            // Now, return the first non-null string
            for (int i = 0; i < possibleChords.Length; i++)
            {
                if (possibleChords[i] != null)
                {
                    StringBuilder sb = new StringBuilder();
                    if (returnNonOctaveNotes)
                    {
                        sb.Append(Note.ToneStringWithoutOctave(notes[i].Value));
                    }
                    else
                    {
                        sb.Append(notes[i]);
                    }
                    sb.Append(possibleChords[i]);
                    if (!bassNote.Equals(notes[i]))
                    {
                        sb.Append("^");
                        sb.Append(bassNote);
                    }
                    return sb.ToString();
                }
            }

            return null;
        }

        public Intervals GetIntervals()
        {
            return intervals;
        }

        public Chord SetBassNote(string newBass)
        {
            return SetBassNote(new Note(newBass));
        }

        public Chord SetBassNote(Note newBass)
        {
            if (Root == null)
            {
                return this;
            }
            for (int i = 0; i < intervals.Size; i++)
            {
                if (newBass.Value % 12 == (Root.Value + Theory.Intervals.GetHalfsteps(intervals.GetNthInterval(i))) % 12)
                {
                    Inversion = i;
                }
            }
            return this;
        }

        public Note GetBassNote()
        {
            int bassNoteValue = Root.Value - Note.SemitonesInOctave +
                                Theory.Intervals.GetHalfsteps(intervals.GetNthInterval(Inversion));
            return new Note(Note.NoteNamesCommon[bassNoteValue % Note.SemitonesInOctave]).UseSameExplicitOctaveSettingAs(Root);
        }

        public Chord SetOctave(int octave)
        {
            Root.Value = (Root.PositionInOctave + octave * Note.SemitonesInOctave);
            return this;
        }

        public Note[] GetNotes()
        {
            int[] halfsteps = intervals.ToHalfstepArray();
            Note[] retVal = new Note[halfsteps.Length];
            retVal[0] = new Note(Root);
            for (int i = 0; i < halfsteps.Length - 1; i++)
            {
                retVal[i + 1] = new Note(retVal[i].Value + halfsteps[i + 1] - halfsteps[i])
                { IsFirstNote = false, IsMelodicNote = false, IsHarmonicNote = true };
                retVal[i + 1].UseSameExplicitOctaveSettingAs(Root);
                if (!Root.IsOctaveExplicitlySet)
                {
                    retVal[i + 1].OriginalString = (Note.ToneStringWithoutOctave((retVal[i].Value + halfsteps[i + 1] - halfsteps[i])));
                }
            }

            // Now calculate inversion
            // 2017-02-17: It looks like this is putting notes up, instead of moving other notes down
            for (int i = 0; i < Inversion; i++)
            {
                if (i < retVal.Length)
                {
                    retVal[i].Value = (byte)(retVal[i].Value + Note.SemitonesInOctave);
                }
            }

            // Rotate the returned notes based on the inversion
            // Cmaj should return C E G, but Cmaj^^ should return G C E
            Note[] retVal2 = new Note[retVal.Length];
            for (int i = 0; i < retVal.Length; i++)
            {
                retVal2[i] = retVal[(i + Inversion) % retVal.Length];
            }

            return retVal2;
        }

        private string InsertChordNameIntoNote(Note note, string chordName)
        {
            StringBuilder buddy = new StringBuilder();
            //buddy.Append(Note.getToneString(note.getValue()));
            buddy.Append(note.ToneString);
            buddy.Append(chordName);
            if (note.IsDurationExplicitlySet)
            {
                buddy.Append(Note.DurationString(note.Duration));
            }
            buddy.Append(note.VelocityString());
            return buddy.ToString();
        }

        public string GetChordType()
        {
            foreach (var entry in ChordMap)
            {
                if (GetIntervals().Equals(entry.Value))
                {
                    return entry.Key;
                }
            }
            return null;
        }

        public static int GetInversionFromChordString(string chordString)
        {
            return chordString.Count(c => c == '^');
        }

        public Pattern GetPattern()
        {
            Pattern pattern = new Pattern();
            bool foundChord = false;
            string chordName = GetChordType();
            if (chordName != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(InsertChordNameIntoNote(Root, chordName));
                for (int i = 0; i < Inversion; i++)
                {
                    sb.Append("^");
                }
                pattern.Add(sb.ToString());
                foundChord = true;
            }
            if (!foundChord)
            {
                return GetPatternWithNotes();
            }
            return pattern;
        }

        public Pattern GetPatternWithNotes()
        {
            // A better way of creating a Chord: Check to see if the intervals are in the map; if so, use the associated name. 
            // (Then you'd need to check for inversions, too)
            StringBuilder sb = new StringBuilder();
            Note[] notes = GetNotes();
            for (int i = 0; i < notes.Length - 1; i++)
            {
                sb.Append(notes[i].GetPattern());
                sb.Append("+");
            }
            sb.Append(notes[notes.Length - 1]);
            return new Pattern(sb.ToString());
        }


        public Pattern GetPatternWithNotesExceptRoot()
        {
            var builder = new StringBuilder();
            var notes = GetNotes();
            for (int i = 0; i < notes.Length; i++)
            {
                if (notes[i].PositionInOctave != Root.PositionInOctave)
                {
                    builder.Append(notes[i].GetPattern());
                    builder.Append("+");
                }
            }
            builder.Remove(builder.Length - 1, 1);
            return new Pattern(builder.ToString());
        }

        public Pattern GetPatternWithNotesExceptBass()
        {
            var builder = new StringBuilder();
            var notes = GetNotes();
            for (int i = 0; i < notes.Length - 1; i++)
            {
                if (notes[i].Value % Note.SemitonesInOctave != GetBassNote().Value % Note.SemitonesInOctave)
                {
                    builder.Append(notes[i].GetPattern());
                    builder.Append("+");
                }
            }
            builder.Append(notes[notes.Length - 1]);
            return new Pattern(builder.ToString());
        }

        public override string ToString()
        {
            return GetPattern().ToString();
        }

        public string ToHumanReadableString()
        {
            return Root + GetHumanReadableName(GetChordType());
        }


        /// <summary>
        /// Returns a string consisting of the notes in the chord.
        /// For example, new Chord("Cmaj").toNoteString() returns "(C+E+G)"
        /// </summary> 
        public string ToNoteString()
        {
            var builder = new StringBuilder();
            builder.Append("(");
            foreach (var note in GetNotes())
            {
                builder.Append(note);
                builder.Append("+");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append(")");
            return builder.ToString();
        }


        public static readonly Intervals MAJOR_INTERVALS = new Intervals("1 3 5");
        public static readonly Intervals MINOR_INTERVALS = new Intervals("1 b3 5");
        public static readonly Intervals DIMINISHED_INTERVALS = new Intervals("1 b3 b5");
        public static readonly Intervals MAJOR_SEVENTH_INTERVALS = new Intervals("1 3 5 7");
        public static readonly Intervals MINOR_SEVENTH_INTERVALS = new Intervals("1 b3 5 b7");
        public static readonly Intervals DIMINISHED_SEVENTH_INTERVALS = new Intervals("1 b3 b5 6");
        public static readonly Intervals MAJOR_SEVENTH_SIXTH_INTERVALS = new Intervals("1 3 5 6 7");
        public static readonly Intervals MINOR_SEVENTH_SIXTH_INTERVALS = new Intervals("1 3 5 6 7");
        public static readonly int OCTAVE = 12;

        #region Equality members
        protected bool Equals(Chord other)
        {
            return Equals(Root, other.Root) && Equals(intervals, other.intervals) && Inversion == other.Inversion;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Chord)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Root?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (intervals?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Inversion;
                return hashCode;
            }
        }
        #endregion
    }
}