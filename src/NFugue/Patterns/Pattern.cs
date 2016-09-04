using System.Collections.Generic;
using System.Text;

namespace NFugue.Patterns
{
    public class Pattern : IPatternProducer, ITokenProducer
    {
        protected StringBuilder patternBuilder = new StringBuilder();

        private const int UNDECLARED_EXPLICIT = -1;
        private int explicitVoice = UNDECLARED_EXPLICIT;
        private int explicitInstrument = UNDECLARED_EXPLICIT;
        private int explicitTempo = UNDECLARED_EXPLICIT;

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
            //StaccatoParserPatternHelper spph = new StaccatoParserPatternHelper();
            //return spph.getTokens(this.getPattern());
            return null;
        }

        public override string ToString()
        {
            var b2 = new StringBuilder();

            // Add the explicit tempo, if one has been provided
            if (explicitTempo != UNDECLARED_EXPLICIT)
            {
                //b2.Append(TempoSubparser.TEMPO);
                b2.Append(explicitTempo);
                b2.Append(" ");
            }

            // Add the explicit voice, if one has been provided
            if (explicitVoice != UNDECLARED_EXPLICIT)
            {
                // b2.Append(IVLSubparser.VOICE);
                b2.Append(explicitVoice);
                b2.Append(" ");
            }

            // Add the explicit voice, if one has been provided
            if (explicitInstrument != UNDECLARED_EXPLICIT)
            {
                // b2.Append(IVLSubparser.INSTRUMENT);
                b2.Append("[");
                // b2.Append(MidiDictionary.INSTRUMENT_BYTE_TO_STRING.get((byte)explicitInstrument));
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

        public Pattern SetVoice(int trackCounter)
        {
            throw new System.NotImplementedException();
        }
    }
}