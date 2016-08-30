namespace NFugue.Rhythm
{
    public interface IRhythmAltLayerProvider
    {
        /// <summary>
        /// Override this method to provide Rhythm with an alternate layer
        /// based on the current segment. For example, if you would like to
        /// return a new layer for every 5th segment, you might say:
        ///     <code>if (segment % 5 == 0) return "S...O...S..oO..."</code>
        /// If there is no alt layer to provide, return null. 
        /// </summary>
        /// <param name="segment">The index into rhythm's length</param>
        /// <returns>A new alt layer, or null if no alt layer is to be provided</returns>
        /// <seealso cref="Rhythm.Length"/>
        ///
        string ProvideAltLayer(int segment);
    }
}