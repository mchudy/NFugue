using NFugue.Parsing;
using NFugue.Patterns;
using NFugue.Staccato.Utils;

namespace NFugue.Staccato
{
    public class StaccatoPatternBuilder
    {
        private readonly Parser parser;
        private int track;

        public StaccatoPatternBuilder(Parser parser)
        {
            this.parser = parser;
            BindParserEvents();
        }

        public Pattern Pattern { get; private set; } = new Pattern();

        private void BindParserEvents()
        {
            parser.BeforeParsingStarted += (s, e) => Pattern = new Pattern();
            parser.TrackChanged += (s, e) =>
            {
                Pattern.Add(StaccatoElementsFactory.CreateTrackElement(e.Track));
                track = e.Track;
            };
            parser.LayerChanged += (s, e) => Pattern.Add(StaccatoElementsFactory.CreateLayerElement(e.Layer));
            parser.InstrumentParsed += (s, e) => Pattern.Add(StaccatoElementsFactory.CreateInstrumentElement(e.Instrument));
            parser.TempoChanged += (s, e) => Pattern.Add(StaccatoElementsFactory.CreateTempoElement(e.TempoBPM));
            parser.KeySignatureParsed += (s, e) => Pattern.Add(StaccatoElementsFactory.CreateKeySignatureElement(e.Key, e.Scale));
            parser.TimeSignatureParsed +=
                (s, e) => Pattern.Add(StaccatoElementsFactory.CreateTimeSignatureElement(e.Numerator, e.PowerOfTwo));
            parser.BarLineParsed += (s, e) => Pattern.Add(StaccatoElementsFactory.CreateBarLineElement(e.Time));
            parser.TrackBeatTimeBookmarked +=
                (s, e) => Pattern.Add(StaccatoElementsFactory.CreateTrackBeatTimeBookmarkElement(e.TimeBookmarkId));
            parser.TrackBeatTimeBookmarkRequested +=
                (s, e) => Pattern.Add(StaccatoElementsFactory.CreateTrackBeatTimeBookmarkRequestElement(e.TimeBookmarkId));
            parser.TrackBeatTimeRequested +=
                (s, e) => Pattern.Add(StaccatoElementsFactory.CreateTrackBeatTimeRequestElement(e.Time));
            parser.PitchWheelParsed += (s, e) => Pattern.Add(StaccatoElementsFactory.CreatePitchWheelElement(e.LSB, e.MSB));
            parser.ChannelPressureParsed +=
                (s, e) => Pattern.Add(StaccatoElementsFactory.CreateChannelPressureElement(e.Pressure));
            parser.PolyphonicPressureParsed +=
                (s, e) => Pattern.Add(StaccatoElementsFactory.CreatePolyphonicPressureElement(e.Key, e.Pressure));
            parser.SystemExclusiveParsed += (s, e) => Pattern.Add(StaccatoElementsFactory.CreateSystemExclusiveElement(e.Bytes));
            parser.ControllerEventParsed +=
                (s, e) => Pattern.Add(StaccatoElementsFactory.CreateControllerEventElement(e.Controller, e.Value));
            parser.LyricParsed += (s, e) => Pattern.Add(StaccatoElementsFactory.CreateLyricElement(e.Lyric));
            parser.MarkerParsed += (s, e) => Pattern.Add(StaccatoElementsFactory.CreateMarkerElement(e.Marker));
            parser.FunctionParsed += (s, e) => Pattern.Add(StaccatoElementsFactory.CreateFunctionElement(e.Id, e.Message));
            parser.NoteParsed += (s, e) => Pattern.Add(StaccatoElementsFactory.CreateNoteElement(e.Note));
            parser.ChordParsed += (s, e) => Pattern.Add(StaccatoElementsFactory.CreateChordElement(e.Chord));
        }
    }
}