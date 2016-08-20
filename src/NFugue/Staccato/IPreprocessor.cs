namespace Staccato
{
    /// <summary>
    /// A Preprocessor takes a token from the Staccato string, does some computation on the string,
    /// and returns the String results of the computation so it may be included back into the
    /// Staccato string. This is used for functionality that can be expressed in a String but must
    /// be expanded to actual Staccato instructions. 
    /// </summary>
    /// <remarks>
    /// The MicrotonePreprocess is an example of this. The user is allowed to express a microtone
    /// using 'm' followed by the frequency - e.g., m440. The MicrotonePreprocessor takes this String,
    /// parses the frequency value, figures out what Pitch Wheel and Note events need to be called to
    /// generate this frequency in MIDI, and returns the full set of Staccato Pitch Wheel and Note 
    /// events.
    /// </remarks>
    public interface IPreprocessor
    {
        string Preprocess(string musicString, StaccatoParserContext context);
    }
}