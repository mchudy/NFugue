using NFugue.Rhythms;
using System;
using System.Collections.Generic;

namespace NFugue.Patterns
{
    public class TrackTable : IPatternProducer
    {
        public const int NumTracks = 16;
        public const int RhythmTrack = 9;

        private readonly IList<IList<IPatternProducer>> tracks = new List<IList<IPatternProducer>>();
        private readonly IList<IPatternProducer> trackSettings = new List<IPatternProducer>();
        private readonly double cellDuration;

        public TrackTable(int length, double cellDuration)
        {
            this.Length = length;
            this.cellDuration = cellDuration;
            for (int i = 0; i < TrackTable.NumTracks; i++)
            {
                trackSettings.Add(new Pattern(""));
                var list = new List<IPatternProducer>();
                for (int u = 0; u < Length; u++)
                {
                    list.Add(new Pattern($"R/{cellDuration:.0#############}"));
                }
                tracks.Add(list);
            }
        }

        public int Length { get; private set; }

        public IList<IPatternProducer> GetTrack(int track) => tracks[track];

        public TrackTable Add(int track, int position, IPatternProducer producer)
        {
            var trackList = tracks[track];
            if (trackList == null)
            {
                trackList = new List<IPatternProducer>();
                tracks.Insert(track, trackList);
            }
            trackList.Insert(position, producer.GetPattern());
            return this;
        }

        public TrackTable Add(int track, int start, params IPatternProducer[] producers)
        {
            int counter = 0;
            foreach (var producer in producers)
            {
                Add(track, start + counter, new[] { producer });
                counter++;
            }
            return this;
        }

        /// <summary>
        /// Puts the given pattern in the track table at every 'nth' position
        /// </summary>
        public TrackTable AddAtIntervals(int track, int nth, IPatternProducer producer)
        {
            for (int position = 0; position < Length; position += nth)
            {
                Add(track, position, producer);
            }
            return this;
        }

        /// <summary>
        /// Puts the given pattern in the track table at every 'nth' position, starting with 
        /// position 'first' and ending with 'end'
        /// </summary>
        public TrackTable AddAtIntervals(int track, int first, int nth, int end, IPatternProducer producer)
        {
            for (int position = first; position < Math.Min(Length, end); position += nth)
            {
                Add(track, position, producer);
            }
            return this;
        }

        public TrackTable Add(int track, int start, int end, IPatternProducer producer)
        {
            for (int i = start; i <= end; i++)
            {
                Add(track, i, producer);
            }
            return this;
        }

        /// <summary>
        /// Lets you specify which cells in the TrackTable should be populated with the given PatternProducer by using a String 
        /// in which a period means "not in this cell" and any other character means "in this cell".
        /// Example: put(1, pattern, "...XXXX..XX....XXXX..XX....");
        /// </summary>
        public TrackTable Add(int track, string periodSelector, IPatternProducer producer)
        {
            for (int i = 0; i < periodSelector.Length; i++)
            {
                if (periodSelector[i] == '.')
                {
                    // No op
                }
                else if (periodSelector[i] == '-')
                {
                    Add(track, i, new Pattern(""));
                }
                else
                {
                    Add(track, i, producer);
                }
            }
            return this;
        }

        public TrackTable Add(Rhythm rhythm)
        {
            for (int i = 0; i < rhythm.Length; i++)
            {
                Add(9, i, rhythm.GetPatternAt(i));
            }
            return this;
        }

        public IPatternProducer this[int track, int position] => tracks[track][position];
        public IPatternProducer Get(int track, int position) => tracks[track][position];

        public TrackTable Clear(int track, int position)
        {
            Add(track, position, new Pattern(""));
            return this;
        }

        public TrackTable Reset(int track, int position)
        {
            Add(track, position, new Pattern($"R/{cellDuration}"));
            return this;
        }

        public TrackTable SetTrackSettings(int track, IPatternProducer producer)
        {
            trackSettings.Insert(track, producer);
            return this;
        }

        public TrackTable SetTrackSettings(int track, string s)
        {
            trackSettings.Insert(track, new Pattern(s));
            return this;
        }

        public IPatternProducer GetTrackSettings(int track) => trackSettings[track];

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

        public Pattern GetPatternAt(int column)
        {
            var columnPattern = new Pattern();
            foreach (var track in tracks)
            {
                IPatternProducer producer = track[column];
                columnPattern.Add(new Pattern(producer).SetVoice(tracks.IndexOf(track)));
            }
            return columnPattern;
        }

        public override string ToString() => GetPattern().ToString();

    }
}