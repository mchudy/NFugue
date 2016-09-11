using NFugue.Theory;
using System;
using System.Globalization;
using System.Text;

namespace NFugue.Integration.LilyPond
{
    internal class NoteWindow
    {
        public char LastNote { get; set; } = 'c';
        public int LastOctave { get; set; } = 4;
        public int CurrentOctave { get; set; } = 4;
        public Note CurrentNote { get; private set; }
        public Note PreviousNote { get; private set; }
        public Note SecondPreviousNote { get; private set; }
        public StringBuilder CurrentNoteLy { get; private set; } = new StringBuilder();
        public string PreviousNoteLy { get; private set; }
        public string SecondPreviousNoteLy { get; private set; }
        public string CurrentNoteDuration { get; private set; }
        public string PreviousNoteDuration { get; private set; }
        public string SecondPreviousNoteDuration { get; private set; }

        public void Empty()
        {
            SecondPreviousNote = null;
            PreviousNote = null;
        }

        public void EmptyAll()
        {
            SecondPreviousNote = null;
            PreviousNote = null;
            CurrentNote = null;
            CurrentNoteLy = new StringBuilder();
            CurrentNoteDuration = null;
        }

        public void AddNote(Note note)
        {
            SecondPreviousNote = PreviousNote;
            SecondPreviousNoteLy = PreviousNoteLy;
            PreviousNote = CurrentNote;
            PreviousNoteLy = CurrentNoteLy.ToString();
            CurrentNoteLy = new StringBuilder();
            CurrentNote = note;
            if (!note.IsRest)
            {
                string firstLetter = note.OriginalString.Substring(0, 1).ToLower();
                CurrentNoteLy.Append(firstLetter);
                CurrentOctave = note.Octave;
                if (note.OriginalString.Length > 1)
                {
                    string secondLetter = note.OriginalString.Substring(1, 1).ToLower();
                    if (secondLetter == "b")
                    {
                        CurrentNoteLy.Append("es");
                    }
                    else if (secondLetter == "#")
                    {
                        CurrentNoteLy.Append("is");
                    }
                }
                int octaveChange = GetOctaveChange(firstLetter[0]);
                if (octaveChange > 0)
                {
                    for (int i = 0; i < octaveChange; i++)
                    {
                        CurrentNoteLy.Append("'");
                    }
                }
                else if (octaveChange < 0)
                {
                    for (int i = 0; i > octaveChange; i--)
                    {
                        CurrentNoteLy.Append(",");
                    }
                }
                LastNote = firstLetter[0];
            }
            else
            {
                CurrentNoteLy.Append("r");
            }
            SecondPreviousNoteDuration = PreviousNoteDuration;
            PreviousNoteDuration = CurrentNoteDuration;
            CurrentNoteDuration = LilyPondNoteDurationHelper.GetDuration(note.Duration.ToString(CultureInfo.InvariantCulture));
        }

        public void AddChordOctave(Note root)
        {
            CurrentOctave = root.Octave;
        }

        internal int GetOctaveChange(char currentNoteChar)
        {
            int octaveChange = CurrentOctave - LastOctave;
            int lilypondChange = 0;
            if (PreviousNote != null && !PreviousNote.IsRest)
            {
                lilypondChange = LilyPondRelativeDirection(PreviousNote.OriginalString.ToLower()[0], currentNoteChar);
            }
            else
            {
                lilypondChange = LilyPondRelativeDirection(LastNote, currentNoteChar);
            }

            int jfugueChange = 0;
            if (PreviousNote != null && !PreviousNote.IsRest)
            {
                jfugueChange = NFugueOctaveChange(PreviousNote.OriginalString.ToLower()[0], currentNoteChar, lilypondChange);
            }
            else
            {
                jfugueChange = NFugueOctaveChange(LastNote, currentNoteChar, lilypondChange);
            }
            octaveChange += jfugueChange;
            LastOctave = CurrentOctave;
            return octaveChange;
        }

        private int LilyPondRelativeDirection(char firstNote, char secondNote)
        {
            char curChar = firstNote;
            if (firstNote == secondNote)
            {
                return 0;
            }
            for (int i = 1; i < 4; i++)
            {
                curChar++;
                if (curChar > 'g')
                {
                    curChar = 'a';
                }

                if (curChar == secondNote)
                {
                    return i;
                }
            }
            curChar = firstNote;
            for (int i = 1; i < 4; i++)
            {
                curChar--;
                if (curChar < 'a')
                {
                    curChar = 'g';
                }

                if (curChar == secondNote)
                {
                    return -i;
                }
            }
            return 0;
        }

        private int NFugueOctaveChange(char firstNote, char secondNote, int lilypondDirection)
        {
            char curChar = firstNote;
            int steps = Math.Abs(lilypondDirection) + 1;
            for (int i = 1; Math.Abs(i) < steps; i += 1)
            {
                if (lilypondDirection > 0)
                {
                    curChar++;
                    if (curChar > 'g')
                    {
                        curChar = 'a';
                    }
                    if ((firstNote < 'c' || firstNote >= 'g') && curChar > 'b')
                    {
                        return -1;
                    }
                }
                else
                {
                    curChar--;
                    if (curChar < 'a')
                    {
                        curChar = 'g';
                    }
                    if (firstNote >= 'c' && firstNote < 'f' && curChar < 'c')
                    {
                        return 1;
                    }
                }
            }
            return 0;
        }
    }
}
