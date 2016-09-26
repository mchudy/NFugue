using FluentAssertions;
using NFugue.Rhythms;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NFugue.Tests
{
    public class RhythmTests
    {
        private readonly Rhythm rhythm = new Rhythm();

        [Fact]
        public void Test_replacing_with_default_rhythm_kit()
        {
            rhythm.AddLayer(".OoSs^`*+x");
            rhythm.GetPattern().ToString()
                .Should().Be("V9 L0 Ri [BASS_DRUM]i Rs [BASS_DRUM]s [ACOUSTIC_SNARE]i Rs [ACOUSTIC_SNARE]s [PEDAL_HI_HAT]i [PEDAL_HI_HAT]s Rs [CRASH_CYMBAL_1]i [CRASH_CYMBAL_1]s Rs Rs [HAND_CLAP]s");
        }

        [Fact]
        public void Test_replacing_with_provided_rhythm_kit()
        {
            rhythm.RhythmKit = new Dictionary<char, string>
            {
                {'a', "[BASS_DRUM]i"},
                {'b', "[ACOUSTIC_SNARE]s Rs"},
                {'.', "Ri" }
            };
            rhythm.AddLayer("ab.");

            rhythm.GetPattern().ToString()
                .Should().Be("V9 L0 [BASS_DRUM]i [ACOUSTIC_SNARE]s Rs Ri");
        }

        [Fact]
        public void Test_replacing_and_multiple_layers()
        {
            rhythm.AddLayer(".");
            rhythm.AddLayer("O");
            rhythm.AddLayer("o");

            rhythm.GetPattern().ToString()
                .Should().Be("V9 L0 Ri L1 [BASS_DRUM]i L2 Rs [BASS_DRUM]s");
        }

        [Fact]
        public void Test_rhythm_with_length()
        {
            rhythm.AddLayer("So");
            rhythm.Length = 3;

            rhythm.GetRhythm().First().Should().Be("SoSoSo");
        }

        [Fact]
        public void Tets_multi_layer_rhythm_with_length()
        {
            rhythm.AddLayer("So").AddLayer(".^");
            rhythm.Length = 5;

            var rhythms = rhythm.GetRhythm().ToList();
            rhythms[0].Should().Be("SoSoSoSoSo");
            rhythms[1].Should().Be(".^.^.^.^.^");
        }

        [Fact]
        public void Test_rhythm_with_ranged_alt_layer()
        {
            rhythm.AddLayer("So");
            rhythm.Length = 5;
            rhythm.AddRangedAltLayer(0, 1, 2, "PP");
            rhythm.GetRhythm().FirstOrDefault().Should().Be("SoPPPPSoSo");
        }

        [Fact]
        public void Test_rhythm_with_one_time_alt_layer()
        {
            rhythm.AddLayer("So");
            rhythm.Length = 5;
            rhythm.AddOneTimeAltLayer(0, 2, "PP");
            rhythm.GetRhythm().First().Should().Be("SoSoPPSoSo");
        }

        [Fact]
        public void Test_rhythm_with_recurring_alt_layer_from_0()
        {
            rhythm.AddLayer("So");
            rhythm.Length = 10;
            rhythm.AddRecurringAltLayer(0, 0, 10, 3, "..");
            rhythm.GetRhythm().First().Should().Be("..SoSo..SoSo..SoSo..");
        }

        [Fact]
        public void Test_rhythm_with_recurring_alt_layer_from_index()
        {
            rhythm.AddLayer("So");
            rhythm.Length = 10;
            rhythm.AddRecurringAltLayer(0, 2, 7, 3, "..");
            rhythm.GetRhythm().First().Should().Be("SoSo..SoSo..SoSoSoSo");
        }

        [Fact]
        public void Test_rhythm_with_providing_alt_layer()
        {
            rhythm.AddLayer("So");
            rhythm.Length = 10;
            rhythm.AddAltLayerProvider(0, segment =>
            {
                if (segment == 2 || segment == 5)
                {
                    return "PP";
                }
                if (segment == 8)
                {
                    return "ZZ";
                }
                return null;
            });

            rhythm.GetRhythm().First().Should().Be("SoSoPPSoSoPPSoSoZZSo");
        }

        [Fact]
        public void Test_alts_with_default_z_order()
        {
            rhythm.AddLayer("So");
            rhythm.Length = 10;
            rhythm.AddRecurringAltLayer(0, 0, 7, 2, "..");
            rhythm.AddRangedAltLayer(0, 1, 5, "GG");
            rhythm.AddOneTimeAltLayer(0, 4, "**");
            rhythm.GetRhythm().First().Should().Be("..GGGGGG**GG..SoSoSo");
        }

        [Fact]
        public void Test_alts_with_specified_z_order()
        {
            rhythm.AddLayer("So");
            rhythm.Length = 10;
            rhythm.AddRecurringAltLayer(0, 0, 10, 2, "..", 3);
            rhythm.AddRangedAltLayer(0, 1, 5, "GG", 2);
            rhythm.AddOneTimeAltLayer(0, 4, "**", 1);
            rhythm.GetRhythm().First().Should().Be("..GG..GG..GG..So..So");
        }

        [Fact]
        public void Test_alts_in_multiple_layers()
        {
            rhythm.AddLayer("So").AddLayer("xx");
            rhythm.Length = 10;
            rhythm.AddRecurringAltLayer(0, 0, 7, 2, "..");
            rhythm.AddRangedAltLayer(1, 1, 5, "XX");
            rhythm.AddOneTimeAltLayer(1, 4, "**");

            var rhythms = rhythm.GetRhythm().ToList();
            rhythms[0].Should().Be("..So..So..So..SoSoSo");
            rhythms[1].Should().Be("xxXXXXXX**XXxxxxxxxx");
        }
    }
}