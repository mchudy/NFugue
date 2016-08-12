using System;

namespace NFugue.Parser
{
    public class Parser
    {
        public event EventHandler BeforeParsingStarted;
        public event EventHandler AfterParsingFinished;
        public event EventHandler<TrackChangedEventArgs> TrackChanged;
        public event EventHandler<LayerChangedEventArgs> LayerChanged;
        public event EventHandler<InstrumentParsedEventArgs> InstrumentParsed;
        public event EventHandler<TempoChangedEventArgs> TempoChanged;
        public event EventHandler<KeySignatureParsedEventArgs> KeySignatureParsed;
        public event EventHandler<TimeSignatureParsedEventArgs> TimeSignatureParsed;
        public event EventHandler<BarLineParsedEventArgs> BarLineParsed;
        public event EventHandler<TrackBeatTimeBookmarkEventArgs> TrackBeatTimeBookmarked;
        public event EventHandler<TrackBeatTimeBookmarkEventArgs> TrackBeatTimeBookmarkRequested;
        public event EventHandler<TrackBeatTimeRequestedEventArgs> TrackBeatTimeRequested;
        public event EventHandler<PitchWheelParsedEventArgs> PitchWheelParsed;
        public event EventHandler<ChannelPressureParsedEventArgs> ChannelPressureParsed;
        public event EventHandler<PolyphonicPressureParsedEventArgs> PolyphonicPressureParsed;
        public event EventHandler<SystemExclusiveParsedEventArgs> SystemExclusiveParsed;
        public event EventHandler<ControllerEventParsedEventArgs> ControllerEventParsed;
        public event EventHandler<LyricParsedEventArgs> LyricParsed;
        public event EventHandler<MarkerParsedEventArgs> MarkerParsed;
        public event EventHandler<FunctionParsedEventArgs> FunctionParsed;
        public event EventHandler<NoteEventArgs> NotePressed;
        public event EventHandler<NoteEventArgs> NoteReleased;
        public event EventHandler<NoteEventArgs> NoteParsed;
        public event EventHandler<ChordParsedEventArgs> ChordParsed;

        protected void OnBeforeParsingStarted()
        {
            BeforeParsingStarted?.Invoke(this, EventArgs.Empty);
        }

        protected void OnAfterParsingFinished()
        {
            AfterParsingFinished?.Invoke(this, EventArgs.Empty);
        }

        public void OnBarLineParsed(long measure)
        {
            BarLineParsed?.Invoke(this, new BarLineParsedEventArgs { Id = measure });
        }

        public void OnTrackBeatTimeRequested(double time)
        {
            TrackBeatTimeRequested?.Invoke(this, new TrackBeatTimeRequestedEventArgs { Time = time });
        }

        public void OnTrackBeatTimeBookmarkRequested(string timeBookmarkId)
        {
            TrackBeatTimeBookmarkRequested?.Invoke(this, new TrackBeatTimeBookmarkEventArgs { TimeBookmarkId = timeBookmarkId });
        }
    }
}