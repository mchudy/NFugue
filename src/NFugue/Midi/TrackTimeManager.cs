using System.Collections.Generic;

namespace NFugue.Midi
{
    public class TrackTimeManager
    {
        private readonly IDictionary<string, double> bookmarkedTrackTimeMap = new Dictionary<string, double>();
        private readonly int[] currentLayerNumber = new int[MidiDefaults.Tracks];
        private int currentTrackNumber = 0;

        public double[,] BeatTime { get; set; } = new double[MidiDefaults.Tracks, MidiDefaults.Layers];
        public int LastCreatedTrackNumber { get; private set; } = 0;
        public double InitialNoteBeatTimeForHarmonicNotes { get; set; } = 0.0;

        public int CurrentTrackNumber
        {
            get { return currentTrackNumber; }
            set
            {
                if (value > LastCreatedTrackNumber)
                {
                    for (int i = LastCreatedTrackNumber + 1; i < value; i++)
                    {
                        CreateTrack(i);
                    }
                    LastCreatedTrackNumber = value;
                }
                currentTrackNumber = value;
            }
        }

        public int CurrentLayerNumber
        {
            get { return currentLayerNumber[currentTrackNumber]; }
            set { currentLayerNumber[currentTrackNumber] = value; }
        }

        public void AdvanceTrackBeatTime(double advanceTime)
        {
            BeatTime[CurrentTrackNumber, CurrentLayerNumber] += advanceTime;
        }

        public double TrackBeatTime
        {
            get { return BeatTime[CurrentTrackNumber, CurrentLayerNumber]; }
            set { BeatTime[CurrentTrackNumber, CurrentLayerNumber] = value; }
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

        public double GetLatestTrackBeatTime(int trackNumber)
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

        protected virtual void CreateTrack(int track)
        {
            for (int layer = 0; layer < MidiDefaults.Layers; layer++)
            {
                BeatTime[track, layer] = 0;
            }
            currentLayerNumber[track] = 0;
        }
    }
}