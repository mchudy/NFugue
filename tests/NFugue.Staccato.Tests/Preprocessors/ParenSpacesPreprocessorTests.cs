using FluentAssertions;
using NFugue.Staccato.Preprocessors;
using Xunit;

namespace Staccato.Tests.Preprocessors
{
    public class ParenSpacesPreprocessorTests
    {
        readonly ParenSpacesPreprocessor preprocessor = new ParenSpacesPreprocessor();

        [Fact]
        public void Test_preprocess()
        {
            string output = preprocessor.Preprocess("a b c :test(1, 2, 3) #tag #(tag tag) aminwha90 'lyric '(lyric lyric)", null);
            output.Should().Be("a b c :test(1,_2,_3) #tag #(tag_tag) aminwha90 'lyric '(lyric_lyric)");
        }

        [Fact]
        public void Test_unprocess()
        {
            string output = ParenSpacesPreprocessor.Unprocess("(1,_2,_3)");
            output.Should().Be("(1, 2, 3)");
        }
    }
}