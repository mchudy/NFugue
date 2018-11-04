using NFugue.Parsing;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NFugue.Temporal
{
    public class TemporalPLP : Parser, IParserListener
    {
        private readonly TemporalEventManager eventManager = new TemporalEventManager();

        public SortedDictionary<long, List<ITemporalEvent>> TimeToEventMap
        {
            get => eventManager.TimeToEventMap;
        }

        // FIXME: Same name as Parser.OnBeforeParsingStarted()
        public void OnBeforeParsingStarted(object sender, EventArgs e)
        {
            eventManager.Reset();
        }

        public void OnAfterParsingFinished(object sender, EventArgs e)
        {
            eventManager.Finish();
        }

        public void OnTrackChanged(object sender, TrackChangedEventArgs e)
        {
            eventManager.CurrentTrack = e.Track;
            eventManager.AddRealTimeEvent(new TemporalEvents.TrackEvent(e.Track));
        }

        public void OnLayerChanged(object sender, LayerChangedEventArgs e)
        {
            eventManager.CurrentLayer = e.Layer;
            eventManager.AddRealTimeEvent(new TemporalEvents.LayerEvent(e.Layer));
        }

        public void OnInstrumentParsed(object sender, InstrumentParsedEventArgs e)
        {
            eventManager.AddRealTimeEvent(new TemporalEvents.InstrumentEvent(e.Instrument));
        }

        public void OnTempoChanged(object sender, TempoChangedEventArgs e)
        {
            eventManager.Tempo = e.TempoBPM;
            eventManager.AddRealTimeEvent(new TemporalEvents.TempoEvent(e.TempoBPM));
        }

        public void OnKeySignatureParsed(object sender, KeySignatureParsedEventArgs e)
        {
            eventManager.AddRealTimeEvent(new TemporalEvents.KeySignatureEvent(e.Key, e.Scale));
        }

        public void OnTimeSignatureParsed(object sender, TimeSignatureParsedEventArgs e)
        {
            eventManager.AddRealTimeEvent(new TemporalEvents.TimeSignatureEvent(e.Numerator, e.PowerOfTwo));
        }

        public void OnBarLineParsed(object sender, BarLineParsedEventArgs e)
        {
            eventManager.AddRealTimeEvent(new TemporalEvents.BarEvent(e.Time));
        }

        public void OnTrackBeatTimeBookmarked(object sender, TrackBeatTimeBookmarkEventArgs e)
        {
            eventManager.AddTrackTickTimeBookmark(e.TimeBookmarkId);
        }

        public void OnTrackBeatTimeBookmarkRequested(object sender, TrackBeatTimeBookmarkEventArgs e)
        {
            double time = eventManager.GetTrackBeatTimeBookmark(e.TimeBookmarkId);
            eventManager.TrackBeatTime = time;
        }

        public void OnTrackBeatTimeRequested(object sender, TrackBeatTimeRequestedEventArgs e)
        {
            eventManager.TrackBeatTime = e.Time;
        }

        public void OnPitchWheelParsed(object sender, PitchWheelParsedEventArgs e)
        {
            eventManager.AddRealTimeEvent(new TemporalEvents.PitchWheelEvent(e.LSB, e.MSB));
        }

        public void OnChannelPressureParsed(object sender, ChannelPressureParsedEventArgs e)
        {
            eventManager.AddRealTimeEvent(new TemporalEvents.ChannelPressureEvent(e.Pressure));
        }

        public void OnPolyphonicPressureParsed(object sender, PolyphonicPressureParsedEventArgs e)
        {
            eventManager.AddRealTimeEvent(new TemporalEvents.PolyphonicPressureEvent(e.Key, e.Pressure));
        }

        public void OnSystemExclusiveParsed(object sender, SystemExclusiveParsedEventArgs e)
        {
            eventManager.AddRealTimeEvent(new TemporalEvents.SystemExclusiveEvent(e.Bytes));
        }

        public void OnControllerEventParsed(object sender, ControllerEventParsedEventArgs e)
        {
            eventManager.AddRealTimeEvent(new TemporalEvents.ControllerEvent(e.Controller, e.Value));
        }

        public void OnLyricParsed(object sender, LyricParsedEventArgs e)
        {
            eventManager.AddRealTimeEvent(new TemporalEvents.LyricEvent(e.Lyric));
        }

        public void OnMarkerParsed(object sender, MarkerParsedEventArgs e)
        {
            eventManager.AddRealTimeEvent(new TemporalEvents.MarkerEvent(e.Marker));
        }

        public void OnFunctionParsed(object sender, FunctionParsedEventArgs e)
        {
            eventManager.AddRealTimeEvent(new TemporalEvents.UserEvent(e.Id, e.Message));
        }

        public void OnNotePressed(object sender, NoteEventArgs e)
        {

        }

        public void OnNoteReleased(object sender, NoteEventArgs e)
        {

        }

        public void OnNoteParsed(object sender, NoteEventArgs e)
        {
            eventManager.AddRealTimeEvent(new TemporalEvents.NoteEvent(e.Note));
        }

        public void OnChordParsed(object sender, ChordParsedEventArgs e)
        {
            eventManager.AddRealTimeEvent(new TemporalEvents.ChordEvent(e.Chord));
        }

        private void Delay(int millis)
        {
            Thread.Sleep(millis);
        }

        public void Parse()
        {
            OnBeforeParsingStarted();

            long oldTime = 0;
            var times = eventManager.TimeToEventMap.Keys;
            foreach (long time in times)
            {
                Delay((int)(time - oldTime));
                oldTime = time;

                foreach (ITemporalEvent @event in eventManager.TimeToEventMap[time])
                {
                    @event.Execute(this);
                }
            }

            OnAfterParsingFinished();
        }
    }
}
