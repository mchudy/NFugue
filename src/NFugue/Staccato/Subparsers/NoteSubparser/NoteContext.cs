using NFugue.Theory;
using System;

namespace NFugue.Staccato.Subparsers.NoteSubparser
{
    internal class NoteContext
    {
        public bool IsChord { get; set; }
        public bool IsThereAnother { get; set; }
        public bool IsNumericNote { get; set; }
        public int NoteNumber { get; set; }
        public int OctaveBias { get; set; }
        public bool IsNatural { get; set; }
        public string OriginalString { get; set; }
        public bool IsRest { get; set; }
        public string NoteValueAsString { get; set; }
        public string DurationValueAsString { get; set; }
        public bool IsOctaveExplicitlySet { get; set; }
        public int OctaveNumber { get; set; }
        public int InternalInterval { get; set; }
        public string ChordName { get; set; }
        public Intervals Intervals { get; set; }
        public string InversionBassNote { get; set; }
        public int InversionCount { get; set; }
        public bool IsDurationExplicitlySet { get; set; }
        public double DecimalDuration { get; set; }
        public bool IsEndOfTie { get; set; }
        public bool IsStartOfTie { get; set; }
        public int MostRecentDuration { get; set; }
        public int NoteOnVelocity { get; set; }
        public int NoteOffVelocity { get; set; }
        public bool HasNoteOnVelocity { get; set; }
        public bool HasNoteOffVelocity { get; set; }
        public string NoteOffVelocityValueAsString { get; set; }
        public string NoteOnVelocityValueAsString { get; set; }
        public bool AnotherNoteIsMelodic { get; set; }
        public bool AnotherNoteIsHarmonic { get; set; }
        public bool IsFirstNote { get; set; }
        public bool IsMelodicNote { get; set; }
        public bool IsHarmonicNote { get; set; }

        /// <summary>
        /// NoteContext should only be constructed when a note token is first being parsed.
        /// Subsequent parsings of notes within the same token must create a NoteContext using 
        /// CreateNextNoteContext()
        /// </summary>
        public NoteContext()
        {
            IsFirstNote = true;
        }

        /// <summary>
        /// Must be called (instead of the constructor) for notes other than the first note
        /// being parsed
        /// </summary>
        public NoteContext CreateNextNoteContext()
        {
            NoteContext noteContext = new NoteContext
            {
                IsFirstNote = false,
                IsMelodicNote = AnotherNoteIsMelodic,
                IsHarmonicNote = AnotherNoteIsHarmonic
            };
            return noteContext;
        }

        public NoteContext CreateChordNoteContext()
        {
            NoteContext noteContext = new NoteContext
            {
                IsFirstNote = false,
                IsMelodicNote = false,
                IsHarmonicNote = true,
                DecimalDuration = DecimalDuration
            };
            return noteContext;
        }

        public Note CreateNote(StaccatoParserContext parserContext)
        {
            if (NoteValueAsString != null)
            {
                object value;
                if (parserContext.Dictionary.TryGetValue(NoteValueAsString, out value))
                {
                    NoteNumber = (int)value;
                }
                else
                {
                    throw new ApplicationException("JFugue NoteSubparser: Could not find '" + NoteValueAsString + "' in dictionary.");
                }
            }
            if (DurationValueAsString != null)
            {
                object value;
                if (parserContext.Dictionary.TryGetValue(DurationValueAsString, out value))
                {
                    DecimalDuration = (double)value;
                }
                else
                {
                    throw new ApplicationException("JFugue NoteSubparser: Could not find '" + DurationValueAsString + "' in dictionary.");
                }
            }
            Note note = new Note(NoteNumber);
            note.IsOctaveExplicitlySet = IsOctaveExplicitlySet;
            if (IsDurationExplicitlySet)
            {
                note.Duration = DecimalDuration;
            }
            note.OriginalString = OriginalString;
            note.IsRest = IsRest;

            if (HasNoteOnVelocity)
            {
                if (NoteOnVelocityValueAsString != null)
                {
                    NoteOnVelocity = (int)parserContext.Dictionary[NoteOnVelocityValueAsString];
                }
                note.OnVelocity = NoteOnVelocity;
            }
            if (HasNoteOffVelocity)
            {
                if (NoteOffVelocityValueAsString != null)
                {
                    NoteOffVelocity = (int)parserContext.Dictionary[NoteOffVelocityValueAsString];
                }
                note.OffVelocity = NoteOffVelocity;
            }

            note.IsEndOfTie = IsEndOfTie;
            note.IsStartOfTie = IsStartOfTie;
            note.IsFirstNote = IsFirstNote;
            note.IsHarmonicNote = IsHarmonicNote;
            note.IsMelodicNote = IsMelodicNote;

            return note;
        }

        public Chord CreateChord(StaccatoParserContext parserContext)
        {
            if (NoteValueAsString != null)
            {
                NoteNumber = (int)parserContext.Dictionary[NoteValueAsString];
            }
            if (DurationValueAsString != null)
            {
                DecimalDuration = (double)parserContext.Dictionary[DurationValueAsString];
            }
            Note rootNote = CreateNote(parserContext);
            if (IsChord)
            {
                Chord chord = new Chord(rootNote, Intervals);
                if (InversionBassNote != null)
                {
                    chord.SetBassNote(InversionBassNote);
                }
                else if (InversionCount > 0)
                {
                    chord.Inversion = InversionCount;
                }
                return chord;
            }
            return null;
        }
    }
}