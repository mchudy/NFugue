using FluentAssertions;
using NFugue.Theory;
using Xunit;

namespace NFugue.Tests.Theory
{
    public class NoteTests
    {
        private Note rest = new Note { IsRest = true };

        [Fact]
        public void Tone_string_for_A4()
        {
            Note.GetToneString(57).Should().BeEquivalentTo("A4");
        }

        [Fact]
        public void Duration_string_tests()
        {
            Note.DurationString(0.25).Should().BeEquivalentTo("q");
            Note.DurationString(0.75).Should().BeEquivalentTo("h.");
            Note.DurationString(1.125).Should().BeEquivalentTo("wi");
        }

        [Fact]
        public void Percussion_string_test()
        {
            Note.PercussionString(56).Should().BeEquivalentTo("[cowbell]");
        }

        [Fact]
        public void Duration_string_for_beat_regular_values()
        {
            Note.DurationStringForBeat(2).Should().Be("h");
            Note.DurationStringForBeat(4).Should().Be("q");
            Note.DurationStringForBeat(8).Should().Be("i");
            Note.DurationStringForBeat(16).Should().Be("s");
        }

        [Fact]
        public void Duration_string_for_beat_irregular_values()
        {
            Note.DurationStringForBeat(10).Should().Be("/0.1");
            Note.DurationStringForBeat(6).Should().StartWith("/0.166666666666");
        }

        [Fact]
        public void Should_return_correct_frequency_for_A5()
        {
            Note.FrequencyForNote("A5").Should().BeApproximately(440.0, 0.01);
            Note.FrequencyForNote("A").Should().BeApproximately(440.0, 0.01);
        }

        [Fact]
        public void Should_return_correct_frequency_for_MIDI_value()
        {
            Note.FrequencyForNote(69).Should().BeApproximately(440.0, 0.01);
        }

        [Fact]
        public void Should_recognize_the_same_notes_in_different_notation()
        {
            Note.IsSameNote("G#", "Ab").Should().BeTrue();
            Note.IsSameNote("BB", "a#").Should().BeTrue();
            Note.IsSameNote("C", "C").Should().BeTrue();
        }

        [Fact]
        public void Octave_for_rest_should_be_zero()
        {
            rest.Octave.Should().Be(0);
        }

        [Fact]
        public void Value_for_rest_should_be_zero()
        {
            rest.Value.Should().Be(0);
        }

        [Fact]
        public void Tone_string_for_rest_should_be_R()
        {
            rest.ToString().Should().Be("R");
        }

        [Fact]
        public void Frequency_for_rest_should_be_zero()
        {
            Note.FrequencyForNote("R").Should().Be(0);
        }


        [Fact]
        public void Test_position_in_octave()
        {
            new Note("C").PositionInOctave.Should().Be(0);
            new Note("Bb5").PositionInOctave.Should().Be(10);
            new Note("F#2").PositionInOctave.Should().Be(6);
        }
    }
}
