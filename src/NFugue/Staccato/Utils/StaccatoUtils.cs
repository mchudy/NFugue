using NFugue.Midi;
using NFugue.Providers;
using NFugue.Staccato.Functions;
using NFugue.Staccato.Subparsers;
using NFugue.Theory;
using System;
using System.Linq;

namespace NFugue.Staccato.Utils
{
    public static class StaccatoElementsFactory
    {
        public static string CreateTrackElement(int track)
        {
            return IVLSubparser.VoiceChar.ToString() + track;
        }

        public static string CreateLayerElement(int layer)
        {
            return IVLSubparser.LayerChar.ToString() + layer;
        }

        public static string CreateInstrumentElement(int instrument)
        {
            return IVLSubparser.InstrumentChar.ToString() + instrument;
        }

        public static string CreateTempoElement(int bpm)
        {
            return TempoSubparser.TempoChar.ToString() + bpm;
        }

        public static string CreateKeySignatureElement(sbyte notePositionInOctave, sbyte scale)
        {
            return SignatureSubparser.KeySignatureString +
                   KeyProviderFactory.GetKeyProvider().CreateKeyString(notePositionInOctave, scale);
        }

        public static string CreateTimeSignatureElement(sbyte numerator, sbyte powerOfTwo)
        {
            return SignatureSubparser.TimeSignatureString + numerator + SignatureSubparser.SeparatorString +
                   (int)Math.Pow(2, powerOfTwo);
        }

        public static string CreateBarLineElement(long time) => BarLineSubparser.Barline.ToString();


        public static string CreateTrackBeatTimeBookmarkElement(string timeBookmarkId)
        {
            return char.ToString(LyricMarkerSubparser.MarkerChar) + timeBookmarkId;
        }

        public static string CreateTrackBeatTimeBookmarkRequestElement(string timeBookmarkId)
        {
            return char.ToString(BeatTimeSubparser.BeatTimeChar) + char.ToString(BeatTimeSubparser.BeatTimeUseMarker) + timeBookmarkId;
        }

        public static string CreateTrackBeatTimeRequestElement(double time)
        {
            return char.ToString(BeatTimeSubparser.BeatTimeChar) + time;
        }

        public static string CreatePitchWheelElement(sbyte lsb, sbyte msb)
        {
            return FunctionSubparser.GenerateFunctionCall(new PitchWheelFunction().GetNames().FirstOrDefault(), lsb, msb);
        }

        public static string CreateChannelPressureElement(sbyte pressure)
        {
            return FunctionSubparser.GenerateFunctionCall(new ChannelPressureFunction().GetNames().FirstOrDefault(), pressure);
        }

        public static string CreatePolyphonicPressureElement(sbyte key, sbyte pressure)
        {
            return FunctionSubparser.GenerateFunctionCall(new PolyPressureFunction().GetNames().FirstOrDefault(), key, pressure);
        }

        public static string CreateSystemExclusiveElement(params sbyte[] bytes)
        {
            return FunctionSubparser.GenerateFunctionCall(new SysexFunction().GetNames().FirstOrDefault(), bytes);
        }

        public static string CreateControllerEventElement(sbyte controller, sbyte value)
        {
            return FunctionSubparser.GenerateFunctionCall(new ControllerFunction().GetNames().FirstOrDefault(), controller, value);
        }

        public static string CreateLyricElement(string lyric)
        {
            return FunctionSubparser.GenerateParenParamIfNecessary(char.ToString(LyricMarkerSubparser.LyricChar), lyric);
        }

        public static string CreateMarkerElement(string marker)
        {
            return FunctionSubparser.GenerateParenParamIfNecessary(char.ToString(LyricMarkerSubparser.MarkerChar), marker);
        }

        public static string CreateFunctionElement(string id, object message)
        {
            return FunctionSubparser.GenerateFunctionCall(id, message);
        }

        public static string CreateNoteElement(Note note)
        {
            return note.GetPattern().ToString();
        }

        public static string CreateNoteElement(Note note, sbyte track)
        {
            return (track == MidiDefaults.PercussionTrack) ? note.GetPercussionPattern().ToString() : CreateNoteElement(note);
        }

        public static string CreateChordElement(Chord chord)
        {
            return chord.GetPattern().ToString();
        }

    }
}