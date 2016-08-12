using System.Collections.Generic;

namespace NFugue.Patterns
{
    public class TrackTable : IPatternProducer
    {
        public const int NumTracks = 16;
        public const int RhythmTrack = 9;

        private IList<IList<IPatternProducer>> tracks = new List<IList<IPatternProducer>>();
        private IList<IPatternProducer> trackSettings = new List<IPatternProducer>();

        public Pattern GetPattern()
        {
            var pattern = new Pattern();
            int trackCounter = 0;
            foreach (var setting in trackSettings)
            {
                if (!string.IsNullOrEmpty(setting.ToString()))
                {
                    pattern.Add(new Pattern(setting).SetVoice(trackCounter));
                }
                trackCounter++;
            }
            for (int i = 0; i < tracks.Count; i++)
            {
                var track = tracks[i];
                foreach (var producer in track)
                {
                    pattern.Add(new Pattern(producer).SetVoice(i));
                }
            }
            return pattern;
        }

        public override string ToString()
        {
            return GetPattern().ToString();
        }
    }
}