using NFugue.Patterns;
using NFugue.Playing;
using NFugue.Staccato;
using Sanford.Multimedia.Midi;

namespace NFugue.Midi.Conversion
{
    /// <summary>
    /// Provides methods for converting MIDI files to <see cref="Pattern"/>
    /// and vice versa
    /// </summary>
    public class MidiFileConverter
    {
        /// <summary>
        /// Saves pattern produced by the <paramref name="patternProducer"/> to a MIDI file
        /// </summary>
        /// <param name="patternProducer">Producer from which the pattern should be saved</param>
        /// <param name="filePath">Path to the MIDI file</param>
        public static void SavePatternToMidi(IPatternProducer patternProducer, string filePath)
        {
            using (var player = new Player())
            {
                player.GetSequence(patternProducer)
                    .Save(filePath);
            }
        }

        /// <summary>
        /// Loads a <see cref="Pattern"/> from a MIDI file
        /// </summary>
        /// <param name="filePath">Path to the MIDI file</param>
        /// <returns>Loaded pattern</returns>
        public static Pattern LoadPatternFromMidi(string filePath)
        {
            var midiParser = new MidiParser();
            var patternBuilder = new StaccatoPatternBuilder(midiParser);
            midiParser.Parse(new Sequence(filePath));
            return patternBuilder.Pattern;
        }
    }
}