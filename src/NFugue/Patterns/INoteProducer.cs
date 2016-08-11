using NFugue.Theory;
using System.Collections.Generic;

namespace NFugue.Patterns
{
    public interface INoteProducer
    {
        IEnumerable<Note> GetNotes();
    }
}