using NFugue.Theory;
using System;
using Xunit;

namespace NFugue.Tests.Theory
{
    public class NoteTests
    {
        [Fact]
        public void Duration_string_tests()
        {
            Assert.Equal(Note.DurationString(0.25), "q", StringComparer.OrdinalIgnoreCase);
            Assert.Equal(Note.DurationString(0.75), "h.", StringComparer.OrdinalIgnoreCase);
            Assert.Equal(Note.DurationString(1.125), "wi", StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Percussion_string_test()
        {
            Assert.Equal(Note.PercussionString(56), "[COWBELL]", StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Octave_for_rest_should_be_zero()
        {
            var note = new Note { IsRest = true };
            Assert.Equal(note.GetOctave(), 0);
        }

        [Fact]
        public void Duration_string_for_beat_regular_values()
        {
            Assert.Equal(Note.DurationStringForBeat(2), "h");
            Assert.Equal(Note.DurationStringForBeat(4), "q");
            Assert.Equal(Note.DurationStringForBeat(8), "i");
            Assert.Equal(Note.DurationStringForBeat(16), "s");
        }

        [Fact]
        public void Duration_string_for_beat_irregular_values()
        {
            Assert.Equal(Note.DurationStringForBeat(10), "/0.1");
            Assert.True(Note.DurationStringForBeat(6).StartsWith("/0.166666666666"));
        }

        [Fact]
        public void Should_return_correct_frequency_for_A5()
        {
            Assert.Equal(440.0, Note.FrequencyForNote("A5"));
            Assert.Equal(440.0, Note.FrequencyForNote("A"));
        }

        [Fact]
        public void Should_return_correct_frequency_for_MIDI_value()
        {
            Assert.Equal(440.0, Note.FrequencyForNote(69));
        }

    }
}
