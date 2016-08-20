namespace NFugue.Staccato
{
    public interface IPreprocessorFunction
    {
        /// <summary>
        ///  Returns the names of this function, which are the same names that would
        /// be seen in the Staccato function call, e.g., "TRILL" for a trill function.
        /// The name of the function must be expressed in all capital letters.
        /// This method may return multiple names of a function to allow for abbreviations
        /// (e.g., TR or TRILL could both be legal names for the trill function)
        /// </summary>
        string[] GetNames();

        /// <summary>
        /// Returns a string that should be inserted into the Staccato string
        /// instead of the function which is being pre-processed.
        /// </summary>
        string Apply(string parameters, StaccatoParserContext context);
    }
}