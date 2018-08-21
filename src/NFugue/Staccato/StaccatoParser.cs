using NFugue.Patterns;
using NFugue.Staccato.Functions;
using NFugue.Staccato.Subparsers;
using NFugue.Staccato.Subparsers.NoteSubparser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NFugue.Parsing;

namespace NFugue.Staccato
{
    public class StaccatoParser : Parsing.Parser
    {
        private readonly IList<IPreprocessor> preprocessors = new List<IPreprocessor>();
        private readonly IList<ISubparser> subparsers = new List<ISubparser>();

        public StaccatoParser()
        {
            Context = new StaccatoParserContext(this);

            NoteSubparser.PopulateContext(Context);
            TempoSubparser.PopulateContext(Context);
            IVLSubparser.PopulateContext(Context);

            InitializeSubparsers();
            InitializePreprocessors();
            InitializeFunctionManager();
        }

        public StaccatoParserContext Context { get; }
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
                        subparser.Parse(substring, Context);
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
                musicString = preprocessor.Preprocess(musicString, Context);
            }
            return musicString;
        }

        public string Preprocess(IPatternProducer producer)
        {
            return Preprocess(producer.ToString());
        }

        internal IEnumerable<string> PreprocessAndSplit(string musicString)
        {
            return Preprocess(musicString).Split(' ');
        }

        internal IEnumerable<ISubparser> Subparsers => new ReadOnlyCollection<ISubparser>(subparsers);

        private void InitializeFunctionManager()
        {
            var types = typeof(StaccatoParser).Assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .ToList();
            var preprocessorFunctionTypes = types.Where(t => typeof(IPreprocessorFunction).IsAssignableFrom(t));
            var subparserFunctionTypes = types.Where(t => typeof(ISubparserFunction).IsAssignableFrom(t));
            foreach (Type type in preprocessorFunctionTypes)
            {
                FunctionManager.Instance.AddPreprocessorFunction((IPreprocessorFunction)Activator.CreateInstance(type));
            }
            foreach (Type type in subparserFunctionTypes)
            {
                FunctionManager.Instance.AddSubparserFunction((ISubparserFunction)Activator.CreateInstance(type));
            }
        }

        private void InitializePreprocessors()
        {
            var preprocessorTypes = typeof(StaccatoParser).Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && typeof(IPreprocessor).IsAssignableFrom(t));
            foreach (Type type in preprocessorTypes)
            {
                preprocessors.Add((IPreprocessor)Activator.CreateInstance(type));
            }
        }

        private void InitializeSubparsers()
        {
            var subparserTypes = typeof(StaccatoParser).Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && typeof(ISubparser).IsAssignableFrom(t));
            foreach (Type subparserType in subparserTypes)
            {
                subparsers.Add((ISubparser)Activator.CreateInstance(subparserType));
            }
        }
    }
}