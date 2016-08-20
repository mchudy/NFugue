using FluentAssertions;
using Staccato.Preprocessors;
using Xunit;

namespace Staccato.Tests.Preprocessors
{
    public class CollectedNotesPreprocessorTests
    {
        private CollectedNotesPreprocessor preprocessor = new CollectedNotesPreprocessor();

        [Fact]
        public void Test_non_collected_notes()
        {
            preprocessor.Preprocess("C E Gq", null).Should().Be("C E Gq");
        }

        [Fact]
        public void Test_collected_notes_with_string_duration()
        {
            preprocessor.Preprocess("(C E G)q", null).Should().Be("Cq Eq Gq");
        }


        [Fact]
        public void Test_collected_notes_with_decimal_duration()
        {
            preprocessor.Preprocess("(C E G)/0.25", null).Should().Be("C/0.25 E/0.25 G/0.25");
        }

        [Fact]
        public void Test_collected_notes_with_pluses()
        {
            preprocessor.Preprocess("(C+E+G)q", null).Should().Be("Cq+Eq+Gq");
        }

        [Fact]
        public void Test_collected_notes_with_other_notes()
        {
            preprocessor.Preprocess("Ah Bh (C E G)q", null).Should().Be("Ah Bh Cq Eq Gq");
        }

        [Fact]
        public void Test_collected_notes_with_plus_and_space()
        {
            preprocessor.Preprocess("Zi (C+E G)q Fw", null).Should().Be("Zi Cq+Eq Gq Fw");
        }

        [Fact]
        public void Test_multiple_collected_notes_in_the_same_string()
        {
            preprocessor.Preprocess("Zi (1+2 3)q Fw (4+5 6)q Zo", null)
                .Should().Be("Zi 1q+2q 3q Fw 4q+5q 6q Zo");
        }

        [Fact]
        public void Test_multiple_collected_notes_in_the_same_string_with_decimal_duration()
        {
            preprocessor.Preprocess("Zi (1+2 3)/4.0 Fw (4+5 6)/0.5 Zo", null)
                .Should().Be("Zi 1/4.0+2/4.0 3/4.0 Fw 4/0.5+5/0.5 6/0.5 Zo");
        }

        [Fact]
        public void Test_nothing_after_parenthesis()
        {
            preprocessor.Preprocess("Zi (1+2 3) Fw (4+5 6) Zo", null)
                .Should().Be("Zi (1+2 3) Fw (4+5 6) Zo");
        }
    }
}