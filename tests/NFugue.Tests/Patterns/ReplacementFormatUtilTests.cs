using System;
using System.Collections.Generic;
using FluentAssertions;
using NFugue.Patterns;
using NFugue.Theory;
using Xunit;

namespace NFugue.Tests.Patterns
{
    public class ReplacementFormatUtilTests
    {
        [Fact]
        public void Test_simple_replacement()
        {
            var patterns = new List<IPatternProducer>
            {
                new Pattern("C"),
                new Pattern("D"),
                new Pattern("E"),
            };

            var result =
                ReplacementFormatUtil.ReplaceDollarsWithCandidates("$0 $1 $2 $!", patterns, new Pattern("C+D+E"));

            result.ToString().Should().Be("C D E C+D+E");
        }

        [Fact]
        public void Test_characters_that_dont_get_replaced()
        {
            var patterns = new List<IPatternProducer> {new Pattern("C"), null, null};

            var result = ReplacementFormatUtil.ReplaceDollarsWithCandidates("$0 T U V", patterns, null);

            result.ToString().Should().Be("C T U V");
        }

        [Fact]
        public void Test_special_replacement()
        {
            var patterns = new Pattern[3];
            patterns[0] = new Pattern("C");
            var specialMap = new Dictionary<string, IPatternProducer> {{"T", new Pattern("E")}};

            var result = ReplacementFormatUtil.ReplaceDollarsWithCandidates("$0 T U V $T", patterns, null, specialMap, " ", " ", null);

            result.ToString().Should().Be("C T U V E");
        }

        [Fact]
        public void Test_replacement_with_individual_appenders()
        {
            var patterns = new List<IPatternProducer>
            {
                new Pattern("C"),
                new Pattern("D"),
                new Pattern("E")
            };

            var result = ReplacementFormatUtil.ReplaceDollarsWithCandidates("$0q $1h $2w $!q", patterns, new Pattern("(C+D+E)"));

            result.ToString().Should().Be("Cq Dh Ew (C+D+E)q");
        }

        [Fact]
        public void Test_replacement_with_chord()
        {
            var chord = new Chord("Cmaj");

            var result = ReplacementFormatUtil.ReplaceDollarsWithCandidates("$0q $1h $2w $!q", chord.GetNotes(), chord);

            result.ToString().Should().Be("Cq Eh Gw CMAJq");
        }

        [Fact]
        public void Test_underscores_and_plus()
        {
            var chord = new Chord("Cmaj");

            var result = ReplacementFormatUtil.ReplaceDollarsWithCandidates("$0q+$1q $2h $1h+$2q_$0q", chord.GetNotes(), chord);

            result.ToString().Should().Be("Cq+Eq Gh Eh+Gq_Cq");
        }

        [Fact]
        public void Test_replacement_with_long_special_map_keys()
        {
            var chord = new Chord("Cmaj");
            var specialMap = new Dictionary<string, IPatternProducer>
            {
                ["URANUS"] = new Pattern("C D E F"),
                ["NEPTUNE"] = new Pattern("A B Ab B"),
                ["JUPITER"] = new Pattern("C G C G"),
                ["MARS"] = new Pattern("F E D C")
            };

            var result = ReplacementFormatUtil.ReplaceDollarsWithCandidates("$0q,$1h,$2w,$URANUSq,Rq,$NEPTUNEq,$!q", chord.GetNotes(), chord, specialMap, ",", " ", ".");

            result.ToString().Should().Be("Cq. Eh. Gw. Cq. Dq. Eq. Fq. Rq. Aq. Bq. Abq. Bq. CMAJq.");
        }
    }
}
