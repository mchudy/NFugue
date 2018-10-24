using NFugue.Theory;
using System;

namespace NFugue.Parsing
{
    public interface IParserListener
    {
        /// <summary>
        /// Called when the parser first starts up, but before it starts parsing anything. 
        /// Provides listeners with a chance to initialize variables and get ready for the parser events. 
        /// </summary>
        void BeforeParsingStarted(object sender, EventArgs e);

        /// <summary>
        /// Called when the parser has parsed its last item. 
        /// Provides listeners with a chance to clean up.
        /// </summary>
        void AfterParsingFinished(object sender, EventArgs e);

        /// <summary>
        /// Called when the parser encounters a new track (also known as a channel; previously in NFugue,
        /// known as a Voice). Tracks correspond to MIDI tracks/channels.
        /// </summary>
        /// <param name="e">the new track event that has been parsed</param>
        void TrackChanged(object sender, TrackChangedEventArgs e);

        /// <summary>
        /// Called when the parser encounters a new layer.
        /// A layer deals with polyphony within a track. While any track may have layers, layers are intended
        /// for use with the percussion track, where each layer may represent notes for a specific percussive instrument.
        /// Layers can essentially be thought of as a "track within a track." Each layer maintains its own 
        /// time progression, so "L1 Eq Eq L2 Cq Gq" would be the same as saying "Eq+Cq Eq+Gq". Layers are a NFugue
        /// feature, and are not a part of the MIDI specification.
        /// </summary>
        void LayerChanged(object sender, LayerChangedEventArgs e);

        /// <summary>
        /// Called when the parser encounters a new instrument selection.
        /// </summary>
        /// <param name="e">the MIDI instrument value that has been parsed</param>
        void InstrumentParsed(object sender, InstrumentParsedEventArgs e);

        /// <summary>
        /// Called when the parser encounters a new tempo selection.
        /// </summary>
        /// <param name="e">The new tempo value</param>
        void TempoChanged(object sender, TempoChangedEventArgs e);

        void KeySignatureParsed(object sender, KeySignatureParsedEventArgs e);

        /// <summary>
        /// The first parameter is the number of beats per measure; 
        /// The second parameter is the power by which 2 must be raised to create the note that represents one beat.
        /// Example 1: For a 5/8 time signature, expect 5,3 (since 2^3 = 8)
        /// Example 2: For a 4/4 time signature, expect 4,2 (since 2^2 = 4)
        /// </summary>
        void TimeSignatureParsed(object sender, TimeSignatureParsedEventArgs e);

        /// <summary>
        /// The separator character which indicates a bar line has been parsed. Generally,
        /// you will want to care about this if you're counting measures, but this should
        /// have no effect on the rendering of a parsed piece of music.
        /// 
        /// </summary>
        /// <param name="e">This is the id of the measure, which is an optional numeric value following the bar character.</param>
        void BarLineParsed(object sender, BarLineParsedEventArgs e);

        void TrackBeatTimeBookmarked(object sender, TrackBeatTimeBookmarkEventArgs e);

        void TrackBeatTimeBookmarkRequested(object sender, TrackBeatTimeBookmarkEventArgs e);

        void TrackBeatTimeRequested(object sender, TrackBeatTimeRequestedEventArgs e);

        void PitchWheelParsed(object sender, PitchWheelParsedEventArgs e);

        void ChannelPressureParsed(object sender, ChannelPressureParsedEventArgs e);

        void PolyphonicPressureParsed(object sender, PolyphonicPressureParsedEventArgs e);

        void SystemExclusiveParsed(object sender, SystemExclusiveParsedEventArgs e);

        void ControllerEventParsed(object sender, ControllerEventParsedEventArgs e);

        void LyricParsed(object sender, LyricParsedEventArgs e);

        void MarkerParsed(object sender, MarkerParsedEventArgs e);

        void FunctionParsed(object sender, FunctionParsedEventArgs e);

        /// <summary>
        /// Used to indicate when a note is pressed. Used in realtime cases when 
        /// notes are actually being pressed and released. Parsers that do not
        /// operate in realtime are not expected to report onNotePressed.
        /// 
        /// Expect the Note event to contain only the note number and note-on velocity.
        /// </summary>
        void NotePressed(object sender, NoteEventArgs e);

        /// <summary>
        /// Used to indicate when a note is released. Used in realtime cases when 
        /// notes are actually being pressed and released. Parsers that do not
        /// operate in realtime are not expected to report onNoteReleased.
        /// 
        /// Expect the Note event to contain only the note number and note-off velocity.
        /// Duration may not be set on the Note from onNoteReleased.
        /// </summary>
        void NoteReleased(object sender, NoteEventArgs e);

        /// <summary>
        /// We may have actually parsed a musical note!
        /// In previous versions of NFugue, ParserListener had separate listeners for parallel notes and sequential notes
        /// (now termed harmonic and melodic notes, respectively)
        /// In this version of NFugue, whether a note is the first note, a harmonic note, or a melodic note is kept
        /// as a property on the Note object itself.
        /// 
        /// </summary>
        /// <param name="e">note The note that was parsed. Please see the Note class for more details about notes!</param>
        /// <seealso cref="Note"/>
        void NoteParsed(object sender, NoteEventArgs e);

        void ChordParsed(object sender, ChordParsedEventArgs e);
    }
}
