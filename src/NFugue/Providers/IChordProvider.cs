using NFugue.Theory;

namespace NFugue.Providers
{
    /// <summary>
    /// This interface must be implemented by the parser responsible for Staccato strings
    /// </summary>
    public interface IChordProvider
    {
        Chord CreateChord(string chordString);
    }
}
