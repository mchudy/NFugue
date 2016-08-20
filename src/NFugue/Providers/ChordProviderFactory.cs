using NFugue.Providers;
using Staccato.Subparsers.NoteSubparser;
using System;

namespace Staccato
{
    public class ChordProviderFactory
    {
        private static readonly Lazy<IChordProvider> chordProvider = new Lazy<IChordProvider>(() => new NoteSubparser());

        public static IChordProvider GetChordProvider()
        {
            return chordProvider.Value;
        }
    }
}