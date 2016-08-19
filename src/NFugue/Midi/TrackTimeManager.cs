using System.Collections.Generic;

namespace NFugue.Midi
{
    public class TrackTimeManager
    {
        private readonly IDictionary<string, double> bookmarkedTrackTimeMap = new Dictionary<string, double>();
        private readonly sbyte[] currentLayerNumber = new sbyte[MidiDefaults.Tracks];
        private sbyte currentTrackNumber = 0;

        public double[,] BeatTime { get; set; } = new double[MidiDefaults.Tracks, MidiDefaults.Layers];
        public sbyte LastCreatedTrackNumber { get; private set; } = 0;
        public double InitialNoteBeatTimeForHarmonicNotes { get; set; } = 0.0;

        public sbyte CurrentTrackNumber
        {
            get { return currentTrackNumber; }
            set
            {
                if (value > LastCreatedTrackNumber)
                {
                    for (int i = LastCreatedTrackNumber + 1; i < value; i++)
                    {
                        CreateTrack((sbyte)i);
                    }
                    LastCreatedTrackNumber = value;
                }
                currentTrackNumber = value;
            }
        }

        public sbyte CurrentLayerNumber
        {
            get { return currentLayerNumber[currentTrackNumber]; }
            set { currentLayerNumber[currentTrackNumber] = value; }
        }

        public void AdvanceTrackBeatTime(double advanceTime)
        {
            BeatTime[currentTrackNumber, currentLayerNumber[currentTrackNumber]] += advanceTime;
        }

        public double TrackBeatTime
        {
            get { return BeatTime[CurrentTrackNumber, CurrentLayerNumber]; }
            set { BeatTime[currentTrackNumber, currentLayerNumber[currentTrackNumber]] = value; }
        }

        public void SetAllTrackBeatTime(double newTime)
        {
            for (int track = 0; track < MidiDefaults.Tracks; track++)
            {
                for (int layer = 0; layer < MidiDefaults.Layers; layer++)
                {
                    if (BeatTime[track, layer] < newTime)
                    {
                        BeatTime[track, layer] = newTime;
                    }
                }
            }
        }

        public void AddTrackTickTimeBookmark(string timeBookmarkId)
        {
            bookmarkedTrackTimeMap[timeBookmarkId] = TrackBeatTime;
        }

        public double GetTrackBeatTimeBookmark(string timeBookmarkId)
        {
            return bookmarkedTrackTimeMap[timeBookmarkId];
        }

        public double GetLatestTrackBeatTime(sbyte trackNumber)
        {
            double latestTime = 0.0;
            for (int i = 0; i < MidiDefaults.Layers; i++)
            {
                if (BeatTime[trackNumber, i] > latestTime)
                {
                    latestTime = BeatTime[trackNumber, i];
                }
            }
            return latestTime;
        }

        protected virtual void CreateTrack(sbyte track)
        {
            for (int layer = 0; layer < MidiDefaults.Layers; layer++)
            {
                BeatTime[track, layer] = 0;
            }
            currentLayerNumber[track] = 0;
        }
    }
}