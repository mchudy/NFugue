using NFugue.Theory;
using System;
using System.Text;

namespace Staccato.Functions
{
    /// <summary>
    /// Replaces the given note with multiple 32nd notes of the given note and the note one interval higher.
    /// For example, ":trill(Cq)" will become "Ct Dt Ct Dt Ct Dt Ct Dt"   
    /// </summary>
    public class TrillFunction : IPreprocessorFunction
    {
        private const double ThirtySecondDuration = 1 / 32.0;

        public string[] GetNames()
        {
            return new[] { "TRILL", "TR" };
        }

        public string Apply(string parameters, StaccatoParserContext context)
        {
            StringBuilder buddy = new StringBuilder();
            foreach (string noteString in parameters.Split(' '))
            {
                try
                {
                    //TODO:
                    //Note note = NoteProviderFactory.getNoteProvider().createNote(noteString);
                    var note = new Note(noteString);
                    int n = (int)(note.Duration / ThirtySecondDuration);
                    for (int i = 0; i < n / 2; i++)
                    {
                        buddy.Append(Note.GetToneString(note.Value));
                        buddy.Append("t ");
                        // This function could really be more intelligent. For example, 
                        // in the following line, the value of the trill note should actually
                        // be consistent with the scale that is being used, and the note that
                        // is being played. In a C-Major scale with an E note, F would be the
                        // trill note, and that is only +1 from E. Also, the trill could become
                        // increasingly quick. 
                        buddy.Append(Note.GetToneString((sbyte)(note.Value + 2)));
                        buddy.Append("t ");
                    }
                }
                catch (Exception)
                {
                    // Nothing to do
                }
            }
            return buddy.ToString().Trim();
        }
    }
}