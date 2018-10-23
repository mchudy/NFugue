using NFugue.Theory;
using System;
using System.Collections.Generic;

namespace NFugue.Parsing
{
    public class Parser
    {
        public readonly SynchronizedCollection<IParserListener> ParserListeners = new SynchronizedCollection<IParserListener>();

        public void AddParserListener(IParserListener listener)
        {
            ParserListeners.Add(listener);

            BeforeParsingStarted           += listener.BeforeParsingStarted;
            AfterParsingFinished           += listener.AfterParsingFinished;
            TrackChanged                   += listener.TrackChanged;
            LayerChanged                   += listener.LayerChanged;
            InstrumentParsed               += listener.InstrumentParsed;
            TempoChanged                   += listener.TempoChanged;
            KeySignatureParsed             += listener.KeySignatureParsed;
            TimeSignatureParsed            += listener.TimeSignatureParsed;
            BarLineParsed                  += listener.BarLineParsed;
            TrackBeatTimeBookmarked        += listener.TrackBeatTimeBookmarked;
            TrackBeatTimeBookmarkRequested += listener.TrackBeatTimeBookmarkRequested;
            TrackBeatTimeRequested         += listener.TrackBeatTimeRequested;
            PitchWheelParsed               += listener.PitchWheelParsed;
            ChannelPressureParsed          += listener.ChannelPressureParsed;
            PolyphonicPressureParsed       += listener.PolyphonicPressureParsed;
            SystemExclusiveParsed          += listener.SystemExclusiveParsed;
            ControllerEventParsed          += listener.ControllerEventParsed;
            LyricParsed                    += listener.LyricParsed;
            MarkerParsed                   += listener.MarkerParsed;
            FunctionParsed                 += listener.FunctionParsed;
            NotePressed                    += listener.NotePressed;
            NoteReleased                   += listener.NoteReleased;
            NoteParsed                     += listener.NoteParsed;
            ChordParsed                    += listener.ChordParsed;
        }

        public void RemoveParserListener(IParserListener listener)
        {
            ParserListeners.Remove(listener);

            BeforeParsingStarted           -= listener.BeforeParsingStarted;
            AfterParsingFinished           -= listener.AfterParsingFinished;
            TrackChanged                   -= listener.TrackChanged;
            LayerChanged                   -= listener.LayerChanged;
            InstrumentParsed               -= listener.InstrumentParsed;
            TempoChanged                   -= listener.TempoChanged;
            KeySignatureParsed             -= listener.KeySignatureParsed;
            TimeSignatureParsed            -= listener.TimeSignatureParsed;
            BarLineParsed                  -= listener.BarLineParsed;
            TrackBeatTimeBookmarked        -= listener.TrackBeatTimeBookmarked;
            TrackBeatTimeBookmarkRequested -= listener.TrackBeatTimeBookmarkRequested;
            TrackBeatTimeRequested         -= listener.TrackBeatTimeRequested;
            PitchWheelParsed               -= listener.PitchWheelParsed;
            ChannelPressureParsed          -= listener.ChannelPressureParsed;
            PolyphonicPressureParsed       -= listener.PolyphonicPressureParsed;
            SystemExclusiveParsed          -= listener.SystemExclusiveParsed;
            ControllerEventParsed          -= listener.ControllerEventParsed;
            LyricParsed                    -= listener.LyricParsed;
            MarkerParsed                   -= listener.MarkerParsed;
            FunctionParsed                 -= listener.FunctionParsed;
            NotePressed                    -= listener.NotePressed;
            NoteReleased                   -= listener.NoteReleased;
            NoteParsed                     -= listener.NoteParsed;
            ChordParsed                    -= listener.ChordParsed;
        }

        public void ClearParserListeners()
        {
            ParserListeners.Clear();
        }

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
            BarLineParsed?.Invoke(this, new BarLineParsedEventArgs { Time = measure });
        }

        public void OnTrackBeatTimeRequested(double time)
        {
            TrackBeatTimeRequested?.Invoke(this, new TrackBeatTimeRequestedEventArgs { Time = time });
        }

        public void OnTrackBeatTimeBookmarkRequested(string timeBookmarkId)
        {
            TrackBeatTimeBookmarkRequested?.Invoke(this, new TrackBeatTimeBookmarkEventArgs { TimeBookmarkId = timeBookmarkId });
        }

        public void OnTrackBeatTimeBookmarked(string timeBookmarkId)
        {
            TrackBeatTimeBookmarked?.Invoke(this, new TrackBeatTimeBookmarkEventArgs { TimeBookmarkId = timeBookmarkId });
        }

        public void OnMarkerParsed(string marker)
        {
            if (marker != null) MarkerParsed?.Invoke(this, new MarkerParsedEventArgs { Marker = marker });
        }

        public void OnLyricParsed(string lyric)
        {
            LyricParsed?.Invoke(this, new LyricParsedEventArgs { Lyric = lyric });
        }

        public void OnTempoChanged(int tempo)
        {
            TempoChanged?.Invoke(this, new TempoChangedEventArgs { TempoBPM = tempo });
        }

        public void OnFunctionParsed(string functionName, string parameters)
        {
            FunctionParsed?.Invoke(this, new FunctionParsedEventArgs { Id = functionName, Message = parameters });
        }

        public void OnInstrumentParsed(int value)
        {
            InstrumentParsed?.Invoke(this, new InstrumentParsedEventArgs { Instrument = value });
        }

        public void OnLayerChanged(int value)
        {
            LayerChanged?.Invoke(this, new LayerChangedEventArgs { Layer = value });
        }

        public void OnTrackChanged(int value)
        {
            TrackChanged?.Invoke(this, new TrackChangedEventArgs { Track = value });
        }

        public void OnKeySignatureParsed(int key, int scale)
        {
            KeySignatureParsed?.Invoke(this, new KeySignatureParsedEventArgs { Key = key, Scale = scale });
        }

        public void OnTimeSignatureParsed(int numerator, int denominator)
        {
            TimeSignatureParsed?.Invoke(this, new TimeSignatureParsedEventArgs { Numerator = numerator, PowerOfTwo = denominator });
        }

        public void OnChordParsed(Chord chord)
        {
            ChordParsed?.Invoke(this, new ChordParsedEventArgs { Chord = chord });
        }

        public void OnNoteParsed(Note note)
        {
            NoteParsed?.Invoke(this, new NoteEventArgs { Note = note });
        }

        public void OnChannelPressureParsed(int pressure)
        {
            ChannelPressureParsed?.Invoke(this, new ChannelPressureParsedEventArgs { Pressure = pressure });
        }

        public void OnControllerEventParsed(int controller, int value)
        {
            ControllerEventParsed?.Invoke(this, new ControllerEventParsedEventArgs { Controller = controller, Value = value });
        }

        public void OnPitchWheelParsed(int lsb, int msb)
        {
            PitchWheelParsed?.Invoke(this, new PitchWheelParsedEventArgs { LSB = lsb, MSB = msb });
        }

        public void OnPolyphonicPressureParsed(int key, int pressure)
        {
            PolyphonicPressureParsed?.Invoke(this, new PolyphonicPressureParsedEventArgs { Key = key, Pressure = pressure });
        }

        public void OnSystemExclusiveParsed(byte[] bytes)
        {
            SystemExclusiveParsed?.Invoke(this, new SystemExclusiveParsedEventArgs { Bytes = bytes });
        }

        public void OnNotePressed(Note note)
        {
            NotePressed?.Invoke(this, new NoteEventArgs { Note = note });
        }

        public void OnNoteReleased(Note note)
        {
            NoteReleased?.Invoke(this, new NoteEventArgs { Note = note });
        }
    }
}