using FluentAssertions;
using NFugue.Staccato;
using NFugue.Staccato.Preprocessors;
using System;
using Xunit;

namespace Staccato.Tests.Preprocessors
{
    public class MicrotonePreprocessorTests
    {
        private readonly MicrotonePreprocessor preprocessor = new MicrotonePreprocessor();
        private readonly StaccatoParserContext context = new StaccatoParserContext(null);

        [Fact]
        public void Should_not_break_regular_string_with_M()
        {
            preprocessor.Preprocess("TIME:44/2 KEY:C", context).Should().Be("TIME:44/2 KEY:C");
        }

        [Fact]
        public void Test_without_given_duration()
        {
            preprocessor.Preprocess("a m512.3 e", context).Should().Be("a :PitchWheel(13384) 59/0.25 :PitchWheel(8192) e");
        }

        [Fact]
        public void Test_given_letter_duration()
        {
            preprocessor.Preprocess("a m512.3h e", context).Should().Be("a :PitchWheel(13384) 59h :PitchWheel(8192) e");
        }

        [Fact]
        public void Test_given_numeric_duration()
        {
            preprocessor.Preprocess("a m512.3/0.5 e", context).Should().Be("a :PitchWheel(13384) 59/0.5 :PitchWheel(8192) e");
        }

        [Fact]
        public void Test_without_decimal_in_frequency()
        {
            preprocessor.Preprocess("a m500 e", context).Should().Be("a :PitchWheel(9937) 59/0.25 :PitchWheel(8192) e");
        }

        [Fact]
        public void Test_micronote_parsed_when_first()
        {
            preprocessor.Preprocess("m500 e", context).Should().Be(":PitchWheel(9937) 59/0.25 :PitchWheel(8192) e");
        }

        [Fact]
        public void Test_micronote_parsed_when_last()
        {
            preprocessor.Preprocess("a m500", context).Should().Be("a :PitchWheel(9937) 59/0.25 :PitchWheel(8192)");
        }

        [Fact]
        public void Test_micronote_parsed_by_itself()
        {
            preprocessor.Preprocess("m500", context).Should().Be(":PitchWheel(9937) 59/0.25 :PitchWheel(8192)");
        }

        [Fact]
        public void Test_two_microtones_parse()
        {
            preprocessor.Preprocess("a m512.3 e m500 a", context)
                .Should().Be("a :PitchWheel(13384) 59/0.25 :PitchWheel(8192) e :PitchWheel(9937) 59/0.25 :PitchWheel(8192) a");
        }

        [Fact]
        public void Test_carnatic_values()
        {
            preprocessor.Preprocess("m261.6256q m290.6951q", context)
               .Should().Be(":PitchWheel(8192) 48q :PitchWheel(8192) :PitchWheel(14942) 49q :PitchWheel(8192)");
        }

        [Fact]
        public void Should_throw_given_bad_definition()
        {
            Action act = () => preprocessor.Preprocess("a ma e", context);
            act.ShouldThrow<ArgumentException>();
        }


        [Fact]
        public void Test_microtone_adjustment()
        {
            MicrotonePreprocessor.ConvertFrequencyToStaccato(440.0, "s").Should().Be("57s");
            MicrotonePreprocessor.ConvertFrequencyToStaccato(155.56, "q").Should().Be("39q");
        }
    }
}