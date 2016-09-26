namespace NFugue.Rhythms
{
    public class AltLayer
    {
        public string RhythmString { get; set; }
        public RhythmAltLayerProvider AltLayerProvider { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int Recurrence { get; set; }
        public int ZOrder { get; set; }

        public AltLayer(int startIndex, int endIndex, int recurrence, string rhythmString, RhythmAltLayerProvider altLayerProvider, int zOrder)
        {
            RhythmString = rhythmString;
            AltLayerProvider = altLayerProvider;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Recurrence = recurrence;
            ZOrder = zOrder;
        }

        /// <summary>
        /// Provides Rhythm with an alternate layer based on the current segment. For example,
        /// if you would like to return a new layer for every 5th segment, you might say:
        ///     <code>if (segment % 5 == 0) return "S...O...S..oO..."</code>
        /// If there is no alt layer to provide, it should return null. 
        /// </summary>
        /// <param name="segment">The index into rhythm's length</param>
        /// <returns>A new alt layer, or null if no alt layer is to be provided</returns>
        /// <seealso cref="Rhythm.Length"/>
        public delegate string RhythmAltLayerProvider(int segment);

        /// <summary>
        /// Indicates whether this alt layer should be provided for the given segment
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public bool ShouldProvideAltLayer(int segment)
        {
            // ALways return true if there is an AltLayerProvider
            if (AltLayerProvider != null)
            {
                return true;
            }

            // Check if we're in the right range of start and end indexes, and check the recurrence
            if ((segment >= StartIndex) && (segment <= EndIndex))
            {
                if (Recurrence == -1) return true;
                if ((Recurrence != -1) && (segment % (Recurrence) == StartIndex)) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns this alt layer, assuming that shouldProvideAltLayer is true
        /// </summary> 
        public string GetAltLayer(int segment)
        {
            if (AltLayerProvider != null)
            {
                return AltLayerProvider(segment);
            }
            return RhythmString;
        }
    }
}