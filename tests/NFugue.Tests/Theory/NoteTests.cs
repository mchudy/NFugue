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

    }
}
