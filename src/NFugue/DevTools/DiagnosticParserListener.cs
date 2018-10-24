using System;
using System.Diagnostics;
using NFugue.Parsing;
using NLog;

namespace NFugue.DevTools
{
    public class DiagnosticParserListener : IParserListener
    {
        private readonly ILogger logger = LogManager.GetLogger("org.nfugue");

        private void Print(string message)
        {
            Debug.WriteLine(message);
            logger.Trace(message);
        }

        public void OnBeforeParsingStarted(object sender, EventArgs e)
        {
            Print("Before parsing starts");
        }

        public void OnAfterParsingFinished(object sender, EventArgs e)
        {
            Print("After parsing finished");
        }

        public void OnTrackChanged(object sender, TrackChangedEventArgs e)
        {
            Print($"Track changed to {e.Track}");
        }

        public void OnLayerChanged(object sender, LayerChangedEventArgs e)
        {
            Print($"Layer changed to {e.Layer}");
        }

        public void OnInstrumentParsed(object sender, InstrumentParsedEventArgs e)
        {
            Print($"Instrument parsed: {e.Instrument}");
        }

        public void OnTempoChanged(object sender, TempoChangedEventArgs e)
        {
            Print($"Tempo changed to {e.TempoBPM} BPM");
        }

        public void OnKeySignatureParsed(object sender, KeySignatureParsedEventArgs e)
        {
            Print($"Key signature parsed: key = {e.Key}  scale = {e.Scale}");
        }

        public void OnTimeSignatureParsed(object sender, TimeSignatureParsedEventArgs e)
        {
            Print($"Time signature parsed: {e.Numerator + "/" + (int)Math.Pow(2, e.PowerOfTwo)}");
        }

        public void OnBarLineParsed(object sender, BarLineParsedEventArgs e)
        {
            Print($"Bar line parsed at time = {e.Time}");
        }

        public void OnTrackBeatTimeBookmarked(object sender, TrackBeatTimeBookmarkEventArgs e)
        {
            Print($"Track time bookmarked into \'{e.TimeBookmarkId}\'");
        }

        public void OnTrackBeatTimeBookmarkRequested(object sender, TrackBeatTimeBookmarkEventArgs e)
        {
            Print($"Track time bookmark looked up: \'{e.TimeBookmarkId}\'");
        }

        public void OnTrackBeatTimeRequested(object sender, TrackBeatTimeRequestedEventArgs e)
        {
            Print($"Track time requested: {e.Time}");
        }

        public void OnPitchWheelParsed(object sender, PitchWheelParsedEventArgs e)
        {
            Print($"Pitch wheel parsed, lsb = {e.LSB}  msb = {e.MSB}");
        }

        public void OnChannelPressureParsed(object sender, ChannelPressureParsedEventArgs e)
        {
            Print($"Channel pressure parsed: {e.Pressure}");
        }

        public void OnPolyphonicPressureParsed(object sender, PolyphonicPressureParsedEventArgs e)
        {
            Print($"Polyphonic pressure parsed, key = {e.Key}  pressure = {e.Pressure}");
        }

        public void OnSystemExclusiveParsed(object sender, SystemExclusiveParsedEventArgs e)
        {
            Print($"Sysex parsed, bytes = {e.Bytes}");
        }

        public void OnControllerEventParsed(object sender, ControllerEventParsedEventArgs e)
        {
            Print($"Controller event parsed, controller = {e.Controller}  value = {e.Value}");
        }

        public void OnLyricParsed(object sender, LyricParsedEventArgs e)
        {
            Print($"Lyric parsed: {e.Lyric}");
        }

        public void OnMarkerParsed(object sender, MarkerParsedEventArgs e)
        {
            Print($"Marker parsed: {e.Marker}");
        }

        public void OnFunctionParsed(object sender, FunctionParsedEventArgs e)
        {
            Print($"User event parsed, id = {e.Id}  message = {e.Message}");
        }

        public void OnNotePressed(object sender, NoteEventArgs e)
        {
            Print($"Note pressed: value = {e.Note.Value}  onVelocity = {e.Note.OnVelocity}");
        }

        public void OnNoteReleased(object sender, NoteEventArgs e)
        {
            Print($"Note released: value = {e.Note.Value}  offVelocity = {e.Note.OffVelocity}");
        }

        public void OnNoteParsed(object sender, NoteEventArgs e)
        {
            Print($"Note parsed: value = {e.Note.Value}  duration = {e.Note.Duration}  onVelocity = {e.Note.OnVelocity}  offVelocity = {e.Note.OffVelocity}");
        }

        public void OnChordParsed(object sender, ChordParsedEventArgs e)
        {
            Print($"Chord parsed: rootnote = {e.Chord.Root.Value}  intervals = {e.Chord.GetIntervals()}  duration = {e.Chord.Root.Duration}  onVelocity = {e.Chord.Root.OnVelocity}  offVelocity = {e.Chord.Root.OffVelocity}");
        }
    }
}
