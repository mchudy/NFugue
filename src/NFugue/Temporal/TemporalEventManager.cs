using NFugue.Midi;
using System.Collections.Generic;

namespace NFugue.Temporal
{
    public class TemporalEventManager
    {
        public readonly SortedDictionary<long, List<ITemporalEvent>> TimeToEventMap = new SortedDictionary<long, List<ITemporalEvent>>();
        private int tempoBeatsPerMinute = MidiDefaults.DefaultTempoBeatsPerMinute;
        private int beatsPerWhole = MidiDefaults.DefaultTempoBeatsPerWhole;
        private byte currentTrack = 0;
        private byte[] currentLayer = new byte[MidiDefaults.Tracks];
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

        public void SetTempo(int tempoBPM)
        {
            tempoBeatsPerMinute = tempoBPM;
        }

        /**
         * Sets the current track to which new events will be added.
         * @param layer the track to select
         */
        public void SetCurrentTrack(byte track)
        {
            currentTrack = track;
        }

        /**
         * Sets the current layer within the track to which new events will be added.
         * @param layer the layer to select
         */
        public void SetCurrentLayer(byte layer)
        {
            currentLayer[currentTrack] = layer;
        }

        /**
         * Advances the timer for the current track by the specified duration,
         * which is specified in Pulses Per Quarter (PPQ)
         * @param duration the duration to increase the track timer
         */
        public void AdvanceTrackBeatTime(double advanceTime)
        {
            beatTime[currentTrack, currentLayer[currentTrack]] += advanceTime;
        }

        /**
         * Sets the timer for the current track by the given time,
         * which is specified in Pulses Per Quarter (PPQ)
         * @param newTickTime the time at which to set the track timer
         */
        public void SetTrackBeatTime(double newTime)
        {
            beatTime[currentTrack, currentLayer[currentTrack]] = newTime;
        }

        /**
         * Returns the timer for the current track and current layer.
         * @return the timer value for the current track, specified in Pulses Per Quarter (PPQ)
         */
        public double GetTrackBeatTime()
        {
            return beatTime[currentTrack, currentLayer[currentTrack]];
        }

        public void AddTrackTickTimeBookmark(string timeBookmarkID)
        {
            bookmarkedTrackTimeMap[timeBookmarkID] = GetTrackBeatTime();
        }

        public double GetTrackBeatTimeBookmark(string timeBookmarkID)
        {
            return bookmarkedTrackTimeMap[timeBookmarkID];
        }

        public void AddRealTimeEvent(IDurationTemporalEvent @event)
        {
            AddRealTimeEvent((ITemporalEvent)@event);
            AdvanceTrackBeatTime(@event.GetDuration());
        }

        public void AddRealTimeEvent(ITemporalEvent @event)
        {
            List<ITemporalEvent> eventList = TimeToEventMap[ConvertBeatsToMillis(GetTrackBeatTime())];
            if (eventList == null)
            {
                eventList = new List<ITemporalEvent>();
                TimeToEventMap[ConvertBeatsToMillis(GetTrackBeatTime())] = eventList;
            }
            eventList.Add(@event);
        }

        private long ConvertBeatsToMillis(double beats)
        {
            return (long)((beats * beatsPerWhole * 60000.0D) / tempoBeatsPerMinute);
        }
    }
 }