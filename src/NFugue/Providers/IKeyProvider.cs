using NFugue.Theory;

namespace NFugue.Providers
{
    /// <summary>
    /// This interface must be implemented by the parser responsible for Staccato strings
    /// </summary>
    public interface IKeyProvider
    {
        /// <summary>
        /// Given a key signature, like "Cmaj" or "Kbbbb", return the corresponding Key
        /// </summary>
        Key CreateKey(string keySignature);

        /// <summary>
        /// Creates a key name, like Cmaj, given the root note's position in an octave (e.g., 0 for C)
        /// and a major or minor indicator - @see Scale MAJOR_SCALE_INDICATOR and MINOR_SCALE_INDICATOR
        /// </summary>
        string CreateKeyString(int notePositionInOctave, int scale);

        /// <summary>
        /// Turns number of accidentals (negative for flats, positive for sharps) to a key and returns the
        /// key's root note's position in the octave 
        /// </summary>
        int ConvertAccidentalCountToKeyRootPositionInOctave(int accidentalCount, int scale);

        /// <summary>
        /// Converts the given Key to an integer value, from -7 for Cb major or Ab major to +7 for C# 
        /// minor or A# minor, with 0 being C major or A minor
        /// </summary>
        int ConvertKeyToInt(Key key);
    }
}