using System.ComponentModel.Design.Serialization;
using FluentAssertions;
using NFugue.Staccato;
using NFugue.Staccato.Preprocessors;
using Xunit;

namespace NFugue.Tests.Staccato.Preprocessors
{
    public class BrokenChordPreprocessorTests
    {
        private readonly BrokenChordPreprocessor preprocessor = new BrokenChordPreprocessor();
        private readonly StaccatoParserContext context = new StaccatoParserContext(null);

        [Fact]
        public void Test_one_color_without_octave()
        {
            AssertPreprocess("Cmaj7:$0", "C");
        }

        [Fact]
        public void Test_one_colon_with_octave()
        {
            AssertPreprocess("C5maj7:$0", "C5");
        }


        [Fact]
        public void Test_full_chord()
        {
            AssertPreprocess("Fmaj:$0,$1,$2", "F A C");
        }

        [Fact]
        public void Test_full_chord_with_durations()
        {
            AssertPreprocess("Dmin:$0q,$1i,$2w", "Dq Fi Aw");
        }

        [Fact]
        public void Test_full_chord_with_dynamics()
        {
            AssertPreprocess("Fmaj:$0q,$1w,$2h:a40d90", "Fqa40d90 Awa40d90 Cha40d90");
            AssertPreprocess("Fmaj:$0,$1,$2:q", "Fq Aq Cq");
        }

        [Fact]
        public void Test_underscore_and_plus()
        {
            AssertPreprocess("G6maj:$0q+$1i_$2i", "G6q+B6i_D7i");
            AssertPreprocess("G6maj:$0q+Ri_$2i", "G6q+Ri_D7i");
        }

        [Fact]
        public void Test_underscore_with_dynamics()
        {
            AssertPreprocess("Fmaj:$0+$1,$2:q", "Fq+Aq Cq");
        }

        [Fact]
        public void Test_inversion()
        {
            AssertPreprocess("Amin^^:$0q,$1i,$2h", "Eq Ai Ch");
        }

        [Fact]
        public void Test_combined_durations_with_dot()
        {
            AssertPreprocess("Amin^^:$0q,$1i,$2h:.", "Eq. Ai. Ch.");
        }

        [Fact]
        public void Test_should_not_be_parsed()
        {
            AssertPreprocess(":CON(57,1)", ":CON(57,1)");
        }

        [Fact]
        public void Test_lots_of_rests()
        {
            AssertPreprocess("Cmaj:Rq,Rq,$1q,Rq,Rq", "Rq Rq Eq Rq Rq");
        }

        [Fact]
        public void Test_root()
        {
            AssertPreprocess("Cmaj:$ROOTq,Rq,$ROOTq,Rq", "Cq Rq Cq Rq");
        }

        [Fact]
        public void Test_bass_with_first_inversion()
        {
            AssertPreprocess("Cmaj^:$BASSq,$0q,$ROOTq,Rq", "Eq Eq Cq Rq");
        }

        [Fact]
        public void Test_bass_with_second_inversion()
        {
            AssertPreprocess("Cmaj^^:$BASSq,$0q,$ROOTq,Rq", "Gq Gq Cq Rq");
        }

        [Fact]
        public void Test_all()
        {
            AssertPreprocess("Cmaj:$!q", "(C+E+G)q");
        }

        [Fact]
        public void Test_except_root()
        {
            AssertPreprocess("Cmaj:$NOTROOTq Rq", "(E+G)q Rq");
        }

        [Fact]
        public void Test_except_root_with_second_inversion()
        {
            AssertPreprocess("Cmaj^^:$NOTROOTq Rq", "(G+E)q Rq");
        }

        [Fact]
        public void Test_except_bass()
        {
            AssertPreprocess("Cmaj:$NOTBASSq", "(E+G)q");
        }

        [Fact]
        public void Test_except_bass_with_second_inversion()
        {
            AssertPreprocess("Cmaj^^:$NOTBASSq", "(C+E)q");
        }

        [Fact]
        public void Test_dictionary_lookup()
        {
            context.Dictionary.Add("MARCH", "$0q,$1q+$2q,$1q+$2q,Rq");
            AssertPreprocess("Cmaj:[MARCH]", "Cq Eq+Gq Eq+Gq Rq");
            AssertPreprocess("Cmaj:[MARCH]:a20", "Cqa20 Eqa20+Gqa20 Eqa20+Gqa20 Rqa20");
        }

        [Fact]
        public void Test_intentionally_bad_chord()
        {
            Assert.NotEqual(preprocessor.Preprocess("FOO:$0", context), "G6q+B6i_D7i");
        }

        private void AssertPreprocess(string input, string output)
        {
            var result = preprocessor.Preprocess(input, context);
            Assert.Equal(result, output, ignoreCase: true);
        }
    }
}
