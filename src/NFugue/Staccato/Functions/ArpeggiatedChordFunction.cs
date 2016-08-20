using System.Text;
using NFugue.Theory;

namespace NFugue.Staccato.Functions
{
    public class ArpeggiatedChordFunction : IPreprocessorFunction
    {
        public string[] GetNames()
        {
            return new[] { "ARPEGGIATED", "AR" };
        }

        public string Apply(string parameters, StaccatoParserContext context)
        {
            var chord = new Chord(parameters);
            var notes = chord.GetNotes();
            double duration = chord.Root.Duration;
            double durationPerNote = duration / notes.Length;
            var sb = new StringBuilder();
            foreach (var note in notes)
            {
                sb.Append(Note.GetToneString(note.Value));
                sb.Append("/");
                sb.Append(durationPerNote);
                sb.Append(" ");
            }
            return sb.ToString().Trim();
        }
    }
}