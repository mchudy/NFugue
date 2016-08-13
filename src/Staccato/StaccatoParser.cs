using NFugue.Parser;
using NFugue.Patterns;
using Staccato.Subparsers;
using System.Collections.Generic;
using System.Linq;

namespace Staccato
{
    public class StaccatoParser : Parser
    {
        private readonly StaccatoParserContext context;
        private readonly IList<IPreprocessor> preprocessors = new List<IPreprocessor>();
        private readonly IList<ISubparser> subparsers = new List<ISubparser>();

        public StaccatoParser()
        {
            context = new StaccatoParserContext(this);
            subparsers.Add(new BeatTimeSubparser());
            subparsers.Add(new BarLineSubparser());
        }

        public bool ThrowsExceptionOnUnknownToken { get; set; }

        public void Parse(string musicString)
        {
            OnBeforeParsingStarted();
            foreach (var substring in PreprocessAndSplit(musicString))
            {
                if (!string.IsNullOrEmpty(substring))
                {
                    var subparser = subparsers.FirstOrDefault(s => s.Matches(substring));
                    if (subparser != null)
                    {
                        subparser.Parse(substring, context);
                    }
                    else if (ThrowsExceptionOnUnknownToken)
                    {
                        throw new ParserException(StaccatoMessages.NoParserFound + substring);
                    }
                }
            }
            OnAfterParsingFinished();
        }

        public void Parse(IPatternProducer producer)
        {
            Parse(producer.GetPattern().ToString());
        }

        public string Preprocess(string musicString)
        {
            foreach (var preprocessor in preprocessors)
            {
                musicString = preprocessor.Preprocess(musicString, context);
            }
            return musicString;
        }

        public string Preprocess(IPatternProducer producer)
        {
            return Preprocess(producer.ToString());
        }

        protected IEnumerable<string> PreprocessAndSplit(string musicString)
        {
            return Preprocess(musicString).Split(' ');
        }
    }
}