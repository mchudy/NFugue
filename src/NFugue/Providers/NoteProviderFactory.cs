using NFugue.Staccato.Subparsers.NoteSubparser;

namespace NFugue.Providers
{
    public class NoteProviderFactory
    {
        public static INoteProvider GetNoteProvider()
        {
            return NoteSubparser.Instance;
        }
    }
}