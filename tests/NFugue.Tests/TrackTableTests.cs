using FluentAssertions;
using NFugue.Patterns;
using NFugue.Rhythms;
using System.Linq;
using Xunit;

namespace NFugue.Tests
{
    public class TrackTableTests
    {
        [Fact]
        public void Test_simple_track_table()
        {
            TrackTable t = new TrackTable(10, 1.0d);
            t.Add(0, 0, new Pattern("A B C D"));
            t.ToString().Should().Be("V0 A B C D V0 R/1.0 V0 R/1.0 V0 R/1.0 V0 R/1.0 V0 R/1.0 V0 R/1.0 V0 R/1.0 V0 R/1.0 V0 R/1.0 V0 R/1.0 V1 R/1.0 V1 R/1.0 V1 R/1.0 V1 R/1.0 V1 R/1.0 V1 R/1.0 V1 R/1.0 V1 R/1.0 V1 R/1.0 V1 R/1.0 V2 R/1.0 V2 R/1.0 V2 R/1.0 V2 R/1.0 V2 R/1.0 V2 R/1.0 V2 R/1.0 V2 R/1.0 V2 R/1.0 V2 R/1.0 V3 R/1.0 V3 R/1.0 V3 R/1.0 V3 R/1.0 V3 R/1.0 V3 R/1.0 V3 R/1.0 V3 R/1.0 V3 R/1.0 V3 R/1.0 V4 R/1.0 V4 R/1.0 V4 R/1.0 V4 R/1.0 V4 R/1.0 V4 R/1.0 V4 R/1.0 V4 R/1.0 V4 R/1.0 V4 R/1.0 V5 R/1.0 V5 R/1.0 V5 R/1.0 V5 R/1.0 V5 R/1.0 V5 R/1.0 V5 R/1.0 V5 R/1.0 V5 R/1.0 V5 R/1.0 V6 R/1.0 V6 R/1.0 V6 R/1.0 V6 R/1.0 V6 R/1.0 V6 R/1.0 V6 R/1.0 V6 R/1.0 V6 R/1.0 V6 R/1.0 V7 R/1.0 V7 R/1.0 V7 R/1.0 V7 R/1.0 V7 R/1.0 V7 R/1.0 V7 R/1.0 V7 R/1.0 V7 R/1.0 V7 R/1.0 V8 R/1.0 V8 R/1.0 V8 R/1.0 V8 R/1.0 V8 R/1.0 V8 R/1.0 V8 R/1.0 V8 R/1.0 V8 R/1.0 V8 R/1.0 V9 R/1.0 V9 R/1.0 V9 R/1.0 V9 R/1.0 V9 R/1.0 V9 R/1.0 V9 R/1.0 V9 R/1.0 V9 R/1.0 V9 R/1.0 V10 R/1.0 V10 R/1.0 V10 R/1.0 V10 R/1.0 V10 R/1.0 V10 R/1.0 V10 R/1.0 V10 R/1.0 V10 R/1.0 V10 R/1.0 V11 R/1.0 V11 R/1.0 V11 R/1.0 V11 R/1.0 V11 R/1.0 V11 R/1.0 V11 R/1.0 V11 R/1.0 V11 R/1.0 V11 R/1.0 V12 R/1.0 V12 R/1.0 V12 R/1.0 V12 R/1.0 V12 R/1.0 V12 R/1.0 V12 R/1.0 V12 R/1.0 V12 R/1.0 V12 R/1.0 V13 R/1.0 V13 R/1.0 V13 R/1.0 V13 R/1.0 V13 R/1.0 V13 R/1.0 V13 R/1.0 V13 R/1.0 V13 R/1.0 V13 R/1.0 V14 R/1.0 V14 R/1.0 V14 R/1.0 V14 R/1.0 V14 R/1.0 V14 R/1.0 V14 R/1.0 V14 R/1.0 V14 R/1.0 V14 R/1.0 V15 R/1.0 V15 R/1.0 V15 R/1.0 V15 R/1.0 V15 R/1.0 V15 R/1.0 V15 R/1.0 V15 R/1.0 V15 R/1.0 V15 R/1.0");
        }

        [Fact]
        public void Test_cool_placement()
        {
            TrackTable t = new TrackTable(5, 1.0d);
            t.Add(0, "X-.X-", new Pattern("Cmajh Emajh Gmajh Emajh"));
            t.Add(1, ".X..X", new Pattern("Gq Cq Eq Gq"));
            t.ToString().Should().Be("V0 Cmajh Emajh Gmajh Emajh V0  V0 R/1.0 V0 Cmajh Emajh Gmajh Emajh V0  V0 R/1.0 V0 R/1.0 V0 R/1.0 V0 R/1.0 V1 R/1.0 V1 Gq Cq Eq Gq V1 R/1.0 V1 R/1.0 V1 Gq Cq Eq Gq V1 R/1.0 V1 R/1.0 V2 R/1.0 V2 R/1.0 V2 R/1.0 V2 R/1.0 V2 R/1.0 V3 R/1.0 V3 R/1.0 V3 R/1.0 V3 R/1.0 V3 R/1.0 V4 R/1.0 V4 R/1.0 V4 R/1.0 V4 R/1.0 V4 R/1.0 V5 R/1.0 V5 R/1.0 V5 R/1.0 V5 R/1.0 V5 R/1.0 V6 R/1.0 V6 R/1.0 V6 R/1.0 V6 R/1.0 V6 R/1.0 V7 R/1.0 V7 R/1.0 V7 R/1.0 V7 R/1.0 V7 R/1.0 V8 R/1.0 V8 R/1.0 V8 R/1.0 V8 R/1.0 V8 R/1.0 V9 R/1.0 V9 R/1.0 V9 R/1.0 V9 R/1.0 V9 R/1.0 V10 R/1.0 V10 R/1.0 V10 R/1.0 V10 R/1.0 V10 R/1.0 V11 R/1.0 V11 R/1.0 V11 R/1.0 V11 R/1.0 V11 R/1.0 V12 R/1.0 V12 R/1.0 V12 R/1.0 V12 R/1.0 V12 R/1.0 V13 R/1.0 V13 R/1.0 V13 R/1.0 V13 R/1.0 V13 R/1.0 V14 R/1.0 V14 R/1.0 V14 R/1.0 V14 R/1.0 V14 R/1.0 V15 R/1.0 V15 R/1.0 V15 R/1.0 V15 R/1.0 V15 R/1.0");
        }

        [Fact]
        public void Test_track_settings_and_fluent_api()
        {
            TrackTable t = new TrackTable(5, 1.0d)
                    .Add(0, "X-.X-", new Pattern("Cmajh Emajh Gmajh Emajh"))
                    .Add(1, ".X..X", new Pattern("Gq Cq Eq Gq"))
                    .SetTrackSettings(0, "I[Flute]")
                    .SetTrackSettings(1, "I[Piano]");
            t.ToString().StartsWith("V0 I[Flute] V1 I[Piano]").Should().BeTrue();
        }

        [Fact]
        public void Test_get_pattern_at()
        {
            TrackTable t = new TrackTable(5, 1.0d)
                    .Add(0, "X-.X-", new Pattern("Cmajh Emajh Gmajh Emajh"))
                    .Add(1, ".X..X", new Pattern("Gq Cq Eq Gq"))
                    .SetTrackSettings(0, "I[Flute]")
                    .SetTrackSettings(1, "I[Piano]");
            t.GetPatternAt(1).ToString().Should().Be("V0  V1 Gq Cq Eq Gq V2 R/1.0 V3 R/1.0 V4 R/1.0 V5 R/1.0 V6 R/1.0 V7 R/1.0 V8 R/1.0 V9 R/1.0 V10 R/1.0 V11 R/1.0 V12 R/1.0 V13 R/1.0 V14 R/1.0 V15 R/1.0");
        }

        [Fact]
        public void Test_put_rhythm_in_track()
        {
            Rhythm rhythm = new Rhythm();
            rhythm.AddLayer("So").AddLayer("xx");
            rhythm.Length = 10;
            rhythm.AddRecurringAltLayer(0, 0, 7, 2, "..");
            rhythm.AddRangedAltLayer(1, 1, 5, "XX");
            rhythm.AddOneTimeAltLayer(1, 4, "**");

            var rhythms = rhythm.GetRhythm().ToList();
            rhythms[0].Should().Be("..So..So..So..SoSoSo");
            rhythms[1].Should().Be("xxXXXXXX**XXxxxxxxxx");

            TrackTable t = new TrackTable(10, 1.0d);
            t.Add(rhythm);
            t[9, 3].ToString().Should().Be("V9 L0 [ACOUSTIC_SNARE]i Rs [BASS_DRUM]s L1 [HAND_CLAP]i [HAND_CLAP]i");
            t[9, 9].ToString().Should().Be("V9 L0 [ACOUSTIC_SNARE]i Rs [BASS_DRUM]s L1 Rs [HAND_CLAP]s Rs [HAND_CLAP]s");
        }

    }
}