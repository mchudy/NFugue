using System.Collections.Generic;

namespace NFugue.Integration.MusicXml.Internals
{
    public static class Mappings
    {
        public static IDictionary<string, string> XmlToNFugueChordMap = new SortedDictionary<string, string>()
        {
            {"major", "MAJ"},
            {"major-sixth", "MAJ6"},
            {"major-seventh", "MAJ7"},
            {"major-ninth", "MAJ9"},
            {"major-13th", "MAJ13"},
            {"minor", "MIN"},
            {"minor-sixth", "MIN6"},
            {"minor-seventh", "MIN7"},
            {"minor-ninth", "MIN9"},
            {"minor-11th", "MIN11"},
            {"major-minor", "MINMAJ7"},
            {"dominant", "DOM7"},
            {"dominant-11th", "DOM7%11"},
            {"dominant-ninth", "DOM9"},
            {"dominant-13th", "DOM13"},
            {"augmented", "AUG"},
            {"augmented-seventh", "AUG7"},
            {"diminished", "DIM"},
            {"diminished-seventh", "DIM7"},
            {"suspended-fourth", "SUS4"},
            {"suspended-second", "SUS2"},
        };
    }
}