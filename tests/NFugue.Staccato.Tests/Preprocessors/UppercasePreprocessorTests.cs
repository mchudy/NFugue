using FluentAssertions;
using NFugue.Staccato.Preprocessors;
using Xunit;

namespace Staccato.Tests.Preprocessors
{
    public class UppercasePreprocessorTests
    {
        [Fact]
        public void Test_preprocess()
        {
            var preprocessor = new UppercasePreprocessor();
            string output = preprocessor.Preprocess("a b c :test(1, 2, 3) #tag #(tag tag) aminwha90 'lyric '(lyric lyric)", null);
            output.Should().Be("A B C :TEST(1, 2, 3) #tag #(tag tag) AMINWHA90 'lyric '(lyric lyric)");
        }
    }
}
