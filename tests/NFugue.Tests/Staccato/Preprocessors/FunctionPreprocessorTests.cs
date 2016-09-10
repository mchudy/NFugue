using FluentAssertions;
using NFugue.Staccato;
using NFugue.Staccato.Functions;
using NFugue.Staccato.Preprocessors;
using Xunit;

namespace Staccato.Tests.Preprocessors
{
    public class FunctionPreprocessorTests
    {
        private readonly FunctionPreprocessor preprocessor = new FunctionPreprocessor();
        private readonly StaccatoParserContext context = new StaccatoParserContext(null);

        [Fact]
        public void Given_trill_preprocessor_function_should_transform_input()
        {
            FunctionManager.Instance.AddPreprocessorFunction(new TrillFunction());

            preprocessor.Preprocess("a b c :TRILL(Cq) e r", context)
                .Should().Be("a b c C5t D5t C5t D5t C5t D5t C5t D5t e r");

            preprocessor.Preprocess("a b c :TRILL(Ch Eh) e r", context)
                .Should().Be("a b c C5t D5t C5t D5t C5t D5t C5t D5t C5t D5t C5t D5t C5t D5t C5t D5t E5t F#5t E5t F#5t E5t F#5t E5t F#5t E5t F#5t E5t F#5t E5t F#5t E5t F#5t e r");
        }

        [Fact]
        public void Given_subparser_function_should_not_change_input()
        {
            FunctionManager.Instance.AddSubparserFunction(new PitchWheelFunction());
            preprocessor.Preprocess(":pw(4000) a", context).Should().Be(":pw(4000) a");
        }
    }
}