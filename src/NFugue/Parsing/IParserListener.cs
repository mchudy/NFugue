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
        void OnBeforeParsingStarted(object sender, EventArgs e);

        /// <summary>
        /// Called when the parser has parsed its last item. 
        /// Provides listeners with a chance to clean up.
        /// </summary>
        void OnAfterParsingFinished(object sender, EventArgs e);

        /// <summary>
        /// Called when the parser encounters a new track (also known as a channel; previously in NFugue,
        /// known as a Voice). Tracks correspond to MIDI tracks/channels.
        /// </summary>
        /// <param name="e">the new track event that has been parsed</param>
        void OnTrackChanged(object sender, TrackChangedEventArgs e);

        /// <summary>
        /// Called when the parser encounters a new layer.
        /// A layer deals with polyphony within a track. While any track may have layers, layers are intended
        /// for use with the percussion track, where each layer may represent notes for a specific percussive instrument.
        /// Layers can essentially be thought of as a "track within a track." Each layer maintains its own 
        /// time progression, so "L1 Eq Eq L2 Cq Gq" would be the same as saying "Eq+Cq Eq+Gq". Layers are a NFugue
        /// feature, and are not a part of the MIDI specification.
        /// </summary>
        void OnLayerChanged(object sender, LayerChangedEventArgs e);

        /// <summary>
        /// Called when the parser encounters a new instrument selection.
        /// </summary>
        /// <param name="e">the MIDI instrument value that has been parsed</param>
        void OnInstrumentParsed(object sender, InstrumentParsedEventArgs e);

        /// <summary>
        /// Called when the parser encounters a new tempo selection.
        /// </summary>
        /// <param name="e">The new tempo value</param>
        void OnTempoChanged(object sender, TempoChangedEventArgs e);

        void OnKeySignatureParsed(object sender, KeySignatureParsedEventArgs e);

        /// <summary>
        /// The first parameter is the number of beats per measure; 
        /// The second parameter is the power by which 2 must be raised to create the note that represents one beat.
        /// Example 1: For a 5/8 time signature, expect 5,3 (since 2^3 = 8)
        /// Example 2: For a 4/4 time signature, expect 4,2 (since 2^2 = 4)
        /// </summary>
        void OnTimeSignatureParsed(object sender, TimeSignatureParsedEventArgs e);

        /// <summary>
        /// The separator character which indicates a bar line has been parsed. Generally,
        /// you will want to care about this if you're counting measures, but this should
        /// have no effect on the rendering of a parsed piece of music.
        /// 
        /// </summary>
        /// <param name="e">This is the id of the measure, which is an optional numeric value following the bar character.</param>
        void OnBarLineParsed(object sender, BarLineParsedEventArgs e);

        void OnTrackBeatTimeBookmarked(object sender, TrackBeatTimeBookmarkEventArgs e);

        void OnTrackBeatTimeBookmarkRequested(object sender, TrackBeatTimeBookmarkEventArgs e);

        void OnTrackBeatTimeRequested(object sender, TrackBeatTimeRequestedEventArgs e);

        void OnPitchWheelParsed(object sender, PitchWheelParsedEventArgs e);

        void OnChannelPressureParsed(object sender, ChannelPressureParsedEventArgs e);

        void OnPolyphonicPressureParsed(object sender, PolyphonicPressureParsedEventArgs e);

        void OnSystemExclusiveParsed(object sender, SystemExclusiveParsedEventArgs e);

        void OnControllerEventParsed(object sender, ControllerEventParsedEventArgs e);

        void OnLyricParsed(object sender, LyricParsedEventArgs e);

        void OnMarkerParsed(object sender, MarkerParsedEventArgs e);

        void OnFunctionParsed(object sender, FunctionParsedEventArgs e);

        /// <summary>
        /// Used to indicate when a note is pressed. Used in realtime cases when 
        /// notes are actually being pressed and released. Parsers that do not
        /// operate in realtime are not expected to report onNotePressed.
        /// 
        /// Expect the Note event to contain only the note number and note-on velocity.
        /// </summary>
        void OnNotePressed(object sender, NoteEventArgs e);

        /// <summary>
        /// Used to indicate when a note is released. Used in realtime cases when 
        /// notes are actually being pressed and released. Parsers that do not
        /// operate in realtime are not expected to report onNoteReleased.
        /// 
        /// Expect the Note event to contain only the note number and note-off velocity.
        /// Duration may not be set on the Note from onNoteReleased.
        /// </summary>
        void OnNoteReleased(object sender, NoteEventArgs e);

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
        void OnNoteParsed(object sender, NoteEventArgs e);

        void OnChordParsed(object sender, ChordParsedEventArgs e);
    }
}
