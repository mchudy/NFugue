using NFugue.Midi;
using System.Collections.Generic;

namespace NFugue.Temporal
{
    public class TemporalEventManager
    {
        public readonly SortedDictionary<long, List<ITemporalEvent>> TimeToEventMap = new SortedDictionary<long, List<ITemporalEvent>>();
        private int tempoBeatsPerMinute = MidiDefaults.DefaultTempoBeatsPerMinute;
        private int beatsPerWhole = MidiDefaults.DefaultTempoBeatsPerWhole;
        private int currentTrack = 0;
        private int[] currentLayer = new int[MidiDefaults.Tracks];
        private double[ , ] beatTime = new double[MidiDefaults.Tracks, MidiDefaults.Layers];
        private Dictionary<string, double> bookmarkedTrackTimeMap;

        public TemporalEventManager() { }

        public void Reset()
        {
            bookmarkedTrackTimeMap = new Dictionary<string, double>();
            tempoBeatsPerMinute = MidiDefaults.DefaultTempoBeatsPerMinute;
            currentTrack = 0;
            for (int i = 0; i < MidiDefaults.Tracks; i++)
            {
                currentLayer[i] = 0;
            }
            TimeToEventMap.Clear();
        }

        public void Finish() { }

        public int Tempo
        {
            set { tempoBeatsPerMinute = value; }
        }

        /// <summary>
        /// Sets the current track to which new events will be added
        /// </summary>
        /// <value>The track to select.</value>
        public int CurrentTrack
        {
            set { currentTrack = value; }
        }

        /// <summary>
        /// Sets the current layer within the track to which new events will be added.
        /// </summary>
        /// <value>The layer to select.</value>
        public int CurrentLayer
        {
            set { currentLayer[currentTrack] = value; }
        }

        /// <summary>
        /// Advances the timer for the current track by the specified duration,
        /// which is specified in Pulses Per Quarter (PPQ)
        /// </summary>
        /// <param name="advanceTime">The duration to increase the track timer.</param>
        public void AdvanceTrackBeatTime(double advanceTime)
        {
            beatTime[currentTrack, currentLayer[currentTrack]] += advanceTime;
        }

        /// <summary>
        /// Sets or Gets the timer for the current track and current layer by the given time,
        /// which is specified in Pulses Per Quarter (PPQ).
        /// </summary>
        /// <value>The time at which to set the track timer.</value>
        public double TrackBeatTime
        {
            set => beatTime[currentTrack, currentLayer[currentTrack]] = value;
            get => beatTime[currentTrack, currentLayer[currentTrack]];
        }

        public void AddTrackTickTimeBookmark(string timeBookmarkID)
        {
            bookmarkedTrackTimeMap[timeBookmarkID] = TrackBeatTime;
        }

        public double GetTrackBeatTimeBookmark(string timeBookmarkID)
        {
            return bookmarkedTrackTimeMap[timeBookmarkID];
        }

        public void AddRealTimeEvent(IDurationTemporalEvent @event)
        {
            AddRealTimeEvent((ITemporalEvent)@event);
            AdvanceTrackBeatTime(@event.Duration);
        }

        public void AddRealTimeEvent(ITemporalEvent @event)
        {
            List<ITemporalEvent> eventList;
            if (!TimeToEventMap.TryGetValue(ConvertBeatsToMillis(TrackBeatTime), out eventList))
            {
                eventList = new List<ITemporalEvent>();
                TimeToEventMap[ConvertBeatsToMillis(TrackBeatTime)] = eventList;
            }
            eventList.Add(@event);
        }

        private long ConvertBeatsToMillis(double beats)
        {
            return (long)((beats * beatsPerWhole * 60000.0D) / tempoBeatsPerMinute);
        }
    }
 }