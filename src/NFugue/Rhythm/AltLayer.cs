namespace NFugue.Rhythm
{
    public class AltLayer
    {
        public string RhythmString { get; set; }
        public IRhythmAltLayerProvider AltLayerProvider { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int Recurrence { get; set; }
        public int ZOrder { get; set; }

        public AltLayer(int startIndex, int endIndex, int recurrence, string rhythmString, IRhythmAltLayerProvider altLayerProvider, int zOrder)
        {
            RhythmString = rhythmString;
            AltLayerProvider = altLayerProvider;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Recurrence = recurrence;
            ZOrder = zOrder;
        }

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
                return AltLayerProvider.ProvideAltLayer(segment);
            }
            return RhythmString;
        }
    }
}