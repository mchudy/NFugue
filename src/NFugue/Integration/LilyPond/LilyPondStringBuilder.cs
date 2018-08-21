using NFugue.Extensions;
using NFugue.Midi;
using NFugue.Parsing;
using System.Globalization;
using System.Text;

namespace NFugue.Integration.LilyPond
{
    public class LilyPondStringBuilder
    {
        private bool closeChord;
        private bool handlePolyphony;
        private bool closePolyphony;
        private readonly NoteWindow noteWindow = new NoteWindow();
        private StringBuilder lilyPondString = new StringBuilder(" ");

        public LilyPondStringBuilder(Parser parser)
        {
            parser.TrackChanged += OnTrackChanged;
            parser.InstrumentParsed += OnInstrumentParsed;
            parser.NoteParsed += OnNoteParsed;
            parser.ChordParsed += OnChordParsed;
        }

        public string GetLilyPondString()
        {
            Save(true);
            HandleLastNote();
            lilyPondString.Append("}");
            return lilyPondString.ToString();
        }

        private void OnChordParsed(object sender, ChordParsedEventArgs e)
        {
            noteWindow.AddChordOctave(e.Chord.Root);
            string musicString = e.Chord.GetPatternWithNotes().ToString();
            string duration =
                LilyPondNoteDurationHelper.GetDuration(e.Chord.Root.Duration.ToString(CultureInfo.InvariantCulture));
            ParallelNoteEvent(musicString, duration, e.Chord.Root.OriginalString);
            lilyPondString.Append($">{duration} ");
        }

        private void ParallelNoteEvent(string musicString, string duration, string rootOriginalString)
        {
            bool isFirst = true;
            lilyPondString.Append("<");
            foreach (string note in musicString.Split('+'))
            {
                string firstLetter = note.Substring(0, 1).ToLower();
                lilyPondString.Append(firstLetter);
                if (isFirst)
                {
                    int octaveChange = noteWindow.GetOctaveChange(firstLetter[0]);
                    if (octaveChange > 0)
                    {
                        for (int i = 0; i < octaveChange; i++)
                        {
                            lilyPondString.Append("'");
                        }
                    }
                    else if (octaveChange < 0)
                    {
                        for (int i = 0; i > octaveChange; i--)
                        {
                            lilyPondString.Append(",");
                        }
                    }
                    isFirst = false;
                    noteWindow.LastNote = firstLetter[0];
                }
                lilyPondString.Append(" ");
            }
            lilyPondString.Remove(lilyPondString.Length - 1, 1);
        }

        private void OnNoteParsed(object sender, NoteEventArgs e)
        {
            noteWindow.AddNote(e.Note);
            if (e.Note.IsFirstNote)
            {
                if (handlePolyphony)
                {
                    handlePolyphony = false;
                }
            }
            else
            {
                handlePolyphony = !e.Note.IsHarmonicNote;
            }
            if (noteWindow.SecondPreviousNote != null)
            {
                Save(false);
            }
        }

        private void OnInstrumentParsed(object sender, InstrumentParsedEventArgs e)
        {
            string setInstrumentString =
                $"\\set Staff.instrumentName = \"{((Instrument) e.Instrument).GetDescription()}\" ";
            lilyPondString.Append(setInstrumentString);
        }

        private void OnTrackChanged(object sender, TrackChangedEventArgs e)
        {
            if (lilyPondString.Length > 1)
            {
                Save(true);
                HandleLastNote();
                noteWindow.EmptyAll();
                lilyPondString.Append("}\n");
            }
            else
            {
                lilyPondString = new StringBuilder();
            }
            lilyPondString.Append("\\new Staff { ");
        }

        private void HandleLastNote()
        {
            if (noteWindow.CurrentNoteLy.Length > 0 && !closeChord)
            {
                lilyPondString.Append(noteWindow.CurrentNoteLy);
            }
            if (noteWindow.CurrentNoteDuration != null && !closeChord)
            {
                lilyPondString.Append(noteWindow.CurrentNoteDuration);
            }
            if (noteWindow.CurrentNote != null && !closeChord)
            {
                lilyPondString.Append(" ");
            }

            if (!lilyPondString.ToString().Contains("new Staff"))
            {
                var lyBuffer = new StringBuilder();
                lyBuffer.Append("\\new Staff {");
                lyBuffer.Append(lilyPondString);
                lilyPondString = lyBuffer;
            }
            if (closeChord)
            {
                closeChord = false;
                lilyPondString.Append(noteWindow.CurrentNoteLy);
                lilyPondString.Append(">");
                lilyPondString.Append(noteWindow.CurrentNoteDuration);
                lilyPondString.Append(" ");
            }
            if (closePolyphony)
            {
                lilyPondString.Append("} >> ");
            }
        }

        private void Save(bool isLastSave)
        {
            if (!isLastSave)
            {
                if (noteWindow.SecondPreviousNote.IsFirstNote && noteWindow.PreviousNote.IsHarmonicNote && !noteWindow.CurrentNote.IsMelodicNote)
                {
                    lilyPondString.Append("<");
                    lilyPondString.Append(noteWindow.SecondPreviousNoteLy);
                    lilyPondString.Append(" ");
                    closeChord = true;
                }
                else if (noteWindow.SecondPreviousNote.IsFirstNote && handlePolyphony)
                {
                    if (closePolyphony)
                    {
                        closePolyphony = false;
                        lilyPondString.Append("} >>");
                    }
                    lilyPondString.Append("<< { ");
                    closePolyphony = true;
                    lilyPondString.Append(noteWindow.SecondPreviousNoteLy);
                    lilyPondString.Append(noteWindow.SecondPreviousNoteDuration);
                    if (noteWindow.PreviousNote.IsHarmonicNote)
                    {
                        lilyPondString.Append(" } \\\\ { ");
                    }
                }
                else if (noteWindow.SecondPreviousNote.IsHarmonicNote && noteWindow.PreviousNote.IsFirstNote)
                {
                    // close parallel
                    lilyPondString.Append(noteWindow.SecondPreviousNoteLy);
                    lilyPondString.Append(">");
                    lilyPondString.Append(noteWindow.PreviousNoteDuration);
                    lilyPondString.Append(" ");
                    closeChord = false;
                }
                else
                {
                    lilyPondString.Append(noteWindow.SecondPreviousNoteLy);
                    if (!noteWindow.SecondPreviousNote.IsHarmonicNote)
                    {
                        lilyPondString.Append(noteWindow.SecondPreviousNoteDuration);
                    }
                    lilyPondString.Append(" ");
                }
                if (noteWindow.SecondPreviousNote.IsStartOfTie)
                {
                    lilyPondString.Append("~ ");
                }
            }

            if (!isLastSave && noteWindow.PreviousNote != null && noteWindow.PreviousNote.IsFirstNote && noteWindow.CurrentNote.IsHarmonicNote)
            {
                return;
            }

            if (noteWindow.SecondPreviousNote != null && !noteWindow.CurrentNote.IsHarmonicNote && (noteWindow.PreviousNote != null || noteWindow.CurrentNote != null))
            {
                lilyPondString.Append(noteWindow.PreviousNoteLy);
            }

            if (noteWindow.PreviousNote != null)
            {
                if (isLastSave && noteWindow.PreviousNote.IsFirstNote && noteWindow.CurrentNote.IsHarmonicNote)
                {

                    lilyPondString.Append("<");
                    lilyPondString.Append(noteWindow.PreviousNoteLy);
                    lilyPondString.Append(" ");
                    closeChord = true;
                }
                if (noteWindow.PreviousNote.IsHarmonicNote && noteWindow.CurrentNote.IsHarmonicNote)
                {
                    lilyPondString.Append(noteWindow.PreviousNoteLy);
                    lilyPondString.Append(" ");
                }

                if (noteWindow.PreviousNote.IsHarmonicNote && noteWindow.CurrentNote.IsFirstNote)
                {
                    // close parallel
                    lilyPondString.Append(">");
                    lilyPondString.Append(noteWindow.PreviousNoteDuration);
                    lilyPondString.Append(" ");
                    closeChord = false;
                }
                else if (!noteWindow.CurrentNote.IsHarmonicNote)
                {
                    if (noteWindow.SecondPreviousNote == null)
                    {
                        lilyPondString.Append(noteWindow.PreviousNoteLy);
                    }
                    lilyPondString.Append(noteWindow.PreviousNoteDuration);
                    lilyPondString.Append(" ");
                }

                if (noteWindow.PreviousNote.IsStartOfTie)
                {
                    lilyPondString.Append("~ ");
                }
            }
            noteWindow.Empty();
        }
    }
}