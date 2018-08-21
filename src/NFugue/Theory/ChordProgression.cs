using System.Linq;
using NFugue.Patterns;
using NFugue.Providers;
using System.Text.RegularExpressions;

namespace NFugue.Theory
{
    public class ChordProgression : IPatternProducer
    {
        private string[] progressionElements;
        private Chord[] knownChords;
        private Key key;
        private string allSequence;
        private string eachSequence;

        private ChordProgression()
        {
        }

        /// <summary>
        /// Creates a chord progression given a Progression String, like "I vi ii V" - case is important!
        /// Chords can be separated with spaces("I vi ii V") or dashes("I-vi-ii-V").
        /// </summary>
        /// <param name="progression"></param>
        public ChordProgression(string progression)
        {
            Create(Regex.Split(progression, "[- ]")); // Split on either spaces or dashes
        }

        /// <summary>
        /// Creates a chord progression given an array of Progression Strings, 
        /// like { "I", "vi", "ii", "V" } - case is important!
        /// </summary>
        /// <param name="progressionElements"></param>
        public ChordProgression(string[] progressionElements)
        {
            Create(progressionElements);
        }

        private void Create(string[] progressionElements)
        {
            this.progressionElements = progressionElements;
            this.key = Key.Default;
        }

        public static ChordProgression FromChords(string knownChords)
        {
            string[] knownChordStrings = Regex.Split(knownChords, " +");
            ChordProgression cp = new ChordProgression {knownChords = new Chord[knownChordStrings.Length]};
            for (int i = 0; i < knownChordStrings.Length; i++)
            {
                cp.knownChords[i] = new Chord(knownChordStrings[i]);
            }

            return cp;
        }

        public static ChordProgression FromChords(params Chord[] chords)
        {
            return new ChordProgression {knownChords = chords};
        }

        /** The key usually identifies the tonic note and/or chord [Wikipedia] */
        public ChordProgression SetKey(string key)
        {
            return SetKey(KeyProviderFactory.GetKeyProvider().CreateKey(key));
        }

        public ChordProgression SetKey(Key key)
        {
            this.key = key;
            return this;
        }

        public Pattern GetPattern()
        {
            Pattern pattern = new Pattern();
            foreach (Chord chord in GetChords())
            {
                pattern.Add(chord);
            }

            if (allSequence != null)
            {
                pattern = ReplacementFormatUtil.ReplaceDollarsWithCandidates(allSequence, GetChords(),
                    new Pattern(GetChords()));
            }

            if (eachSequence != null)
            {
                Pattern p2 = new Pattern();
                foreach (var chordString in pattern.ToString().Split(' '))
                {
                    Chord chord = new Chord(chordString);
                    p2.Add(ReplacementFormatUtil.ReplaceDollarsWithCandidates(eachSequence, chord.GetNotes(), chord));
                }

                pattern = p2;
            }

            return pattern;
        }

        /// <summary>
        /// Returns a list of chords represented by this chord progression
        /// </summary>
        /// <returns></returns>
        public Chord[] GetChords()
        {
            if (knownChords != null)
            {
                return knownChords;
            }

            Chord[] chords = new Chord[progressionElements.Length];
            Pattern scalePattern = key.Scale.Intervals.SetRoot(key.Root).GetPattern();
            string[] scaleNotes = scalePattern.ToString().Split(' ');
            int counter = 0;
            foreach (string progressionElement in progressionElements)
            {
                Note rootNote = NoteProviderFactory.GetNoteProvider()
                    .CreateNote(scaleNotes[RomanNumeralToIndex(progressionElement)]);
                rootNote.UseSameDurationAs(key.Root);
                Intervals intervals = Chord.MAJOR_INTERVALS;
                if ((progressionElement[0] == 'i') || (progressionElement[0] == 'v'))
                {
                    // Checking to see if the progression element is lowercase 
                    intervals = Chord.MINOR_INTERVALS;
                }

                if ((progressionElement.ToLower().IndexOf("o") > 0) || (progressionElement.ToLower().IndexOf("d") > 0))
                {
                    // Checking to see if the progression element is diminished
                    intervals = Chord.DIMINISHED_INTERVALS;
                }

                if (progressionElement.EndsWith("7"))
                {
                    if (intervals.Equals(Chord.MAJOR_INTERVALS))
                    {
                        intervals = Chord.MAJOR_SEVENTH_INTERVALS;
                    }
                    else if (intervals.Equals(Chord.MINOR_INTERVALS))
                    {
                        intervals = Chord.MINOR_SEVENTH_INTERVALS;
                    }
                    else if (intervals.Equals(Chord.DIMINISHED_INTERVALS))
                    {
                        intervals = Chord.DIMINISHED_SEVENTH_INTERVALS;
                    }
                }

                if (progressionElement.EndsWith("7%6"))
                {
                    if (intervals.Equals(Chord.MAJOR_INTERVALS))
                    {
                        intervals = Chord.MAJOR_SEVENTH_SIXTH_INTERVALS;
                    }
                    else if (intervals.Equals(Chord.MINOR_INTERVALS))
                    {
                        intervals = Chord.MINOR_SEVENTH_SIXTH_INTERVALS;
                    }
                }

                // Check for inversions
                int inversions = CountInversions(progressionElement);

                chords[counter] = new Chord(rootNote, intervals);
                if (inversions > 0)
                {
                    chords[counter].Inversion = inversions;
                }

                counter++;
            }

            return chords;
        }

        private int RomanNumeralToIndex(string romanNumeral)
        {
            string s = romanNumeral.ToLower();
            if (s.StartsWith("vii"))
            {
                return 6;
            }

            if (s.StartsWith("vi"))
            {
                return 5;
            }

            if (s.StartsWith("v"))
            {
                return 4;
            }

            if (s.StartsWith("iv"))
            {
                return 3;
            }

            if (s.StartsWith("iii"))
            {
                return 2;
            }

            if (s.StartsWith("ii"))
            {
                return 1;
            }

            if (s.StartsWith("i"))
            {
                return 0;
            }

            return 0;
        }

        public override string ToString()
        {
            return GetPattern().ToString();
        }

        public string[] ToStringArray()
        {
            return GetPattern().ToString().Split(' ');
        }

        public ChordProgression EachChordAs(string sequence)
        {
            this.eachSequence = sequence;
            return this;
        }

        public ChordProgression AllChordsAs(string sequence)
        {
            this.allSequence = sequence;
            return this;
        }

        public ChordProgression Distribute(string distribute)
        {
            for (int i = 0; i < progressionElements.Length; i++)
            {
                progressionElements[i] = progressionElements[i] + distribute;
            }

            return this;
        }

        private int CountInversions(string s) => s.Count(t => t == '^');
    }
}