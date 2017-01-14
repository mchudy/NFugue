using NFugue.Extensions;
using NFugue.Midi;
using NFugue.Staccato;
using NFugue.Staccato.Subparsers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NFugue.Midi.Conversion;

namespace NFugue.Patterns
{
    public class Pattern : IPatternProducer, ITokenProducer
    {
        protected StringBuilder patternBuilder = new StringBuilder();

        private const int UndeclaredExplicit = -1;
        private int explicitVoice = UndeclaredExplicit;
        private int explicitInstrument = UndeclaredExplicit;
        private int explicitTempo = UndeclaredExplicit;

        public Pattern()
        { }

        public Pattern(string patternString)
        {
            if (patternBuilder.Length > 0)
            {
                patternBuilder.Append(" ");
            }
            patternBuilder.Append(patternString);
        }

        public Pattern(params string[] strings)
        {
            foreach (var s in strings)
            {
                patternBuilder.Append(s);
                patternBuilder.Append(" ");
            }
        }

        public Pattern(params IPatternProducer[] patternProducers)
        {
            Add(patternProducers);
        }

        public Pattern Add(params IPatternProducer[] patternProducers)
        {
            foreach (var patternProducer in patternProducers)
            {
                Add(patternProducer.GetPattern().ToString());
            }
            return this;
        }

        public Pattern Add(string patternString, int repetitions = 1)
        {
            for (int i = 0; i < repetitions; i++)
            {
                if (patternBuilder.Length > 0)
                {
                    patternBuilder.Append(" ");
                }
                patternBuilder.Append(patternString);
            }
            return this;
        }

        public Pattern Add(IPatternProducer producer, int repetitions = 1)
        {
            for (int i = 0; i < repetitions; i++)
            {
                Add(producer.GetPattern().ToString());
            }
            return this;
        }

        /// <summary>
        /// Prepends each producer in the order it is passed in, 
        /// so if you pass in "F F", "G G", and "E E", and the current
        /// pattern is "A A", you will get "F F G G E E A A".
        /// </summary>
        public Pattern Prepend(params IPatternProducer[] producers)
        {
            StringBuilder temp = new StringBuilder();
            foreach (var producer in producers)
            {
                temp.Append(producer.GetPattern());
                temp.Append(" ");
            }
            this.Prepend(temp.ToString().Trim());
            return this;
        }

        /// <summary>
        /// Inserts the given string to the beginning of this pattern.
        /// If there is content in this pattern already, this method will
        /// insert a space between the given string and this pattern so
        /// the tokens remain separate.
        /// </summary>
        private Pattern Prepend(string patternString)
        {
            if (patternBuilder.Length > 0)
            {
                patternBuilder.Insert(0, " ");
            }
            patternBuilder.Insert(0, patternString);
            return this;
        }

        public Pattern Clear()
        {
            patternBuilder.Clear();
            return this;
        }

        public Pattern Repeat(int n)
        {
            Pattern newPattern = new Pattern();
            string currentPatternString = patternBuilder.ToString();
            for (int i = 0; i < n; i++)
            {
                newPattern.Add(currentPatternString);
            }
            patternBuilder = newPattern.patternBuilder;
            return this;
        }

        public Pattern GetPattern() => this;

        public IEnumerable<Token> GetTokens()
        {
            StaccatoParserPatternHelper spph = new StaccatoParserPatternHelper();
            return spph.GetTokens(GetPattern());
        }

        public override string ToString()
        {
            var b2 = new StringBuilder();

            // Add the explicit tempo, if one has been provided
            if (explicitTempo != UndeclaredExplicit)
            {
                b2.Append(TempoSubparser.TempoChar);
                b2.Append(explicitTempo);
                b2.Append(" ");
            }

            // Add the explicit voice, if one has been provided
            if (explicitVoice != UndeclaredExplicit)
            {
                b2.Append(IVLSubparser.VoiceChar);
                b2.Append(explicitVoice);
                b2.Append(" ");
            }

            // Add the explicit voice, if one has been provided
            if (explicitInstrument != UndeclaredExplicit)
            {
                b2.Append(IVLSubparser.InstrumentChar);
                b2.Append("[");
                b2.Append(((Instrument)explicitInstrument).GetDescription());
                b2.Append("] ");
            }

            // Now add the actual contents of the pattern!
            b2.Append(patternBuilder);

            return b2.ToString();
        }

        /// <summary>
        /// Provides a way to explicitly set the tempo on a Pattern directly
        /// through the pattern rather than by adding text to the contents
        /// of the Pattern.
        /// </summary>
        /// <remarks>
        /// When Pattern.ToString() is called, the a tempo will be prepended 
        /// to the beginning of the pattern in the form of "Tx", where x is the
        /// tempo number.
        /// </remarks>
        /// <returns>This pattern</returns>
        public Pattern SetTempo(int tempo)
        {
            explicitTempo = tempo;
            return this;
        }

        public Pattern SetTempo(Tempo tempo)
        {
            return SetTempo((int)tempo);
        }

        public Pattern SetVoice(int voice)
        {
            explicitVoice = voice;
            return this;
        }

        public Pattern SetInstrument(int instrument)
        {
            explicitInstrument = instrument;
            return this;
        }

        public Pattern SetInstrument(Instrument instrument)
        {
            return SetInstrument((int)instrument);
        }

        /// <summary>
        /// Expects a parameter of "note decorators" - i.e., things that are added to 
        /// the end of a note, such as or attack/decay settings; splits the given 
        /// parameter on spaces and applies each decorator to each note as it is encountered
        /// in the current pattern. 
        /// </summary>
        /// <remarks>
        /// If there is one decorator in the parameter, this method will apply that same
        /// decorator to all note in the pattern.
        /// 
        /// If there are more notes than decorators, a counter resets to 0 and the decorators
        /// starting from the first are applied to the future notes.
        /// </remarks>
        /// <example>
        /// new Pattern("A B C").AddToEachNoteToken("q")       --> "Aq Bq Cq"
        /// new Pattern("A B C").AddToEachNoteToken("q i")     --> "Aq Bi Cq" (rolls back to q for third note)
        /// new Pattern("A B C").AddToEachNoteToken("q i s")   --> "Aq Bi Cs"
        /// new Pattern("A B C").AddToEachNoteToken("q i s w") --> "Aq Bi Cs" (same as "q i s")
        /// </example>
        public Pattern AddToEachNoteToken(string decoratorString)
        {
            int currentDecorator = 0;
            string[] decorators = decoratorString.Split(' ');

            StringBuilder b2 = new StringBuilder();

            List<Token> tokens = GetTokens().ToList();
            foreach (Token token in tokens)
            {
                if (token.Type == TokenType.Note)
                {
                    b2.Append(token);
                    b2.Append(decorators[currentDecorator++ % decorators.Length]);
                }
                else
                {
                    b2.Append(token);
                }
                b2.Append(" ");
            }
            patternBuilder = new StringBuilder(b2.ToString().Trim());
            return this;
        }

        /// <summary>
        /// Saves the pattern to a file
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <param name="comments">Additional comments which will be added at the beggining of the file</param>
        /// <returns></returns>
        public Pattern Save(string filePath, params string[] comments)
        {
            using (var writer = new StreamWriter(filePath))
            {
                if (comments.Any())
                {
                    writer.WriteLine("# ");
                    foreach (var comment in comments)
                    {
                        writer.WriteLine($"# {comment}");
                    }
                    writer.WriteLine("# ");
                }
                writer.Write(ToString());
            }
            return this;
        }

        /// <summary>
        /// Reads pattern from the file
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>Parsed pattern</returns>
        public static Pattern Load(string filePath)
        {
            var pattern = new Pattern();
            foreach (string line in File.ReadLines(filePath))
            {
                if (!line.StartsWith("#"))
                {
                    pattern.Add(line);
                }
            }
            return pattern;
        }

        /// <summary>
        /// Loads pattern from a MIDI file
        /// </summary>
        /// <param name="filePath">Path to the MIDI file</param>
        /// <returns>Loaded pattern</returns>
        public static Pattern LoadFromMidi(string filePath)
        {
            return MidiFileConverter.LoadPatternFromMidi(filePath);
        }
    }
}