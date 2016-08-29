using FluentAssertions;
using NFugue.Staccato;
using NFugue.Staccato.Functions;
using NFugue.Staccato.Preprocessors;
using Xunit;

namespace Staccato.Tests.Preprocessors
{
    public class FunctionPreprocessorTests
    {
        [Fact]
        public void Test_trill_function_preprocessor()
        {
            var preprocessor = new FunctionPreprocessor();
            var context = new StaccatoParserContext(null);
            FunctionManager.Instance.AddPreprocessorFunction(new TrillFunction());

            preprocessor.Preprocess("a b c :TRILL(Cq) e r", context)
                .Should().Be("a b c C5t D5t C5t D5t C5t D5t C5t D5t e r");

            preprocessor.Preprocess("a b c :TRILL(Ch Eh) e r", context)
                .Should().Be("a b c C5t D5t C5t D5t C5t D5t C5t D5t C5t D5t C5t D5t C5t D5t C5t D5t E5t F#5t E5t F#5t E5t F#5t E5t F#5t E5t F#5t E5t F#5t E5t F#5t E5t F#5t e r");
        }
    }
}