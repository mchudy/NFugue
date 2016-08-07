using NFugue.Theory;
using System;
using Xunit;

namespace NFugue.Tests.Theory
{
    public class NoteTests
    {
        [Fact]
        public void Duration_string_for_quarter_note()
        {
            Assert.Equal(Note.DurationString(0.25), "q", StringComparer.OrdinalIgnoreCase);
        }
    }
}
