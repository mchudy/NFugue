using NFugue.Patterns;
using NFugue.Playing;
using NFugue.Staccato;
using Sanford.Multimedia.Midi;

namespace NFugue.Midi
{
    public class MidiFileConverter
    {
        public static void SavePatternToMidi(IPatternProducer patternProducer, string filePath)
        {
            using (var player = new Player())
            {
                player.GetSequence(patternProducer)
                    .Save(filePath);
            }
        }

        public static Pattern LoadPatternFromMidi(string filePath)
        {
            MidiParser midiParser = new MidiParser();
            StaccatoPatternBuilder patternBuilder = new StaccatoPatternBuilder(midiParser);
            midiParser.Parse(new Sequence(filePath));
            return patternBuilder.Pattern;
        }
    }
}