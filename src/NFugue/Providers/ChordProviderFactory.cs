using NFugue.Staccato.Subparsers.NoteSubparser;

namespace NFugue.Providers
{
    public class ChordProviderFactory
    {
        public static IChordProvider GetChordProvider()
        {
            return NoteSubparser.Instance;
        }
    }
}