using FluentAssertions;
using Staccato.Preprocessors;
using System.Collections.Generic;
using Xunit;

namespace Staccato.Tests.Preprocessors
{
    public class ReplacementMapPreprocessorTests
    {
        private readonly IDictionary<string, string> replacementMap = new Dictionary<string, string>();
        private readonly ReplacementMapPreprocessor preprocessor = new ReplacementMapPreprocessor();

        [Fact]
        public void Test_replacement_map_preprocessor_with_single_iteration()
        {
            replacementMap.Add("R1", "m440.0");
            replacementMap.Add("S", "m770.0");
            preprocessor.ReplacementMap = replacementMap;

            preprocessor.Preprocess("<R1> C <S>", null).Should().Be("m440.0 C m770.0");
        }

        [Fact]
        public void Test_replacement_map_preprocessor_with_multiple_iteration()
        {
            replacementMap.Add("A", "<B> 1");
            replacementMap.Add("B", "<A> 2");
            preprocessor.ReplacementMap = replacementMap;
            preprocessor.Iterations = 3;

            preprocessor.Preprocess("<A> A <B>", null).Should().Be("<B> 1 2 1 A <A> 2 1 2");
        }

    }
}