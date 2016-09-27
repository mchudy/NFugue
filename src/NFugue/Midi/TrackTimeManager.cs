using System.Collections.Generic;

namespace NFugue.Midi
{
    /// <summary>
    /// Manages all tracks and layers and maintains time bookmarks
    /// </summary>
    /// <remarks>
    /// This class is time agnostic and might be used for track beats
    /// specified in various units.
    /// </remarks>
    public class TrackTimeManager
    {
        private readonly IDictionary<string, double> bookmarkedTrackTimeMap = new Dictionary<string, double>();
        private readonly int[] currentLayerNumber = new int[MidiDefaults.Tracks];
        private int currentTrackNumber = 0;

        public double[,] BeatTime { get; set; } = new double[MidiDefaults.Tracks, MidiDefaults.Layers];
        public int LastCreatedTrackNumber { get; private set; } = 0;
        public double InitialNoteBeatTimeForHarmonicNotes { get; set; } = 0.0;

        /// <summary>
        /// Gets or the sets the current track to which new events will be added
        /// </summary>
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

        /// <summary>
        /// Gets or sets the current layer within the track to which new events 
        /// will be added
        /// </summary>
        public int CurrentLayerNumber
        {
            get { return currentLayerNumber[currentTrackNumber]; }
            set { currentLayerNumber[currentTrackNumber] = value; }
        }

        /// <summary>
        /// Advances the time for the current track by the specified duration
        /// </summary>
        /// <param name="advanceTime">The duration by which to increase the track time (in PPQ)</param>
        public void AdvanceTrackBeatTime(double advanceTime)
        {
            BeatTime[CurrentTrackNumber, CurrentLayerNumber] += advanceTime;
        }

        /// <summary>
        /// Gets or sets the time for the current track and the current layer in PPQ
        /// </summary>
        public double TrackBeatTime
        {
            get { return BeatTime[CurrentTrackNumber, CurrentLayerNumber]; }
            set { BeatTime[CurrentTrackNumber, CurrentLayerNumber] = value; }
        }

        /// <summary>
        /// Sets the time for all tracks to the given time
        /// </summary>
        /// <param name="newTime">New time in PPQ</param>
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

        /// <summary>
        /// Returns the latest track time across all layers in the given track
        /// </summary>
        /// <param name="trackNumber"></param>
        /// <returns></returns>
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