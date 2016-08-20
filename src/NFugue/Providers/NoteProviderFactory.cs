using NFugue.Providers;
using Staccato.Subparsers.NoteSubparser;
using System;

namespace Staccato
{
    public class NoteProviderFactory
    {
        private static readonly Lazy<INoteProvider> noteProvider = new Lazy<INoteProvider>(() => new NoteSubparser());

        public static INoteProvider GetNoteProvider()
        {
            return noteProvider.Value;
        }
    }
}