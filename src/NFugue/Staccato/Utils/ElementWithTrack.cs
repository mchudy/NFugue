namespace NFugue.Staccato.Utils
{
    /// <summary>
    /// This class wraps an element with the track and layer in which the element appears.
    /// For example, a Note element (like "C5") is associated with information about its channel.
    /// Layers are frequently used for notes in the MIDI percussion track (10th track, known in Staccato as V9) 
    /// </summary>
    public class ElementWithTrack
    {
        public ElementWithTrack(int track, int layer, string element)
        {
            Track = track;
            Layer = layer;
            Element = element;
        }

        public int Track { get; }
        public int Layer { get; }
        public string Element { get; }
    }
}
