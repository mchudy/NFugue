using NFugue.Theory;
using System;
using Xunit;

namespace NFugue.Tests.Theory
{
    public class IntervalsTests
    {
        [Fact]
        public void Test_Nth_interval()
        {
            var intervals = new Intervals("1 3 5");
            Assert.Equal("1", intervals.GetNthInterval(0));
            Assert.Equal("3", intervals.GetNthInterval(1));
            Assert.Equal("5", intervals.GetNthInterval(2));
        }

        [Fact]
        public void Nth_interval_for_index_outside_of_range_should_throw()
        {
            var intervals = new Intervals("1 3 5");
            Assert.Throws<IndexOutOfRangeException>(() => intervals.GetNthInterval(3));
        }

        [Fact]
        public void Test_rotate()
        {
            var intervals = new Intervals("1 3 5");
            intervals.Rotate(1);
            Assert.Equal("3 5 1", intervals.ToString());
        }

        [Fact]
        public void Test_pattern_with_root()
        {
            var intervals = new Intervals("1 3 5");
            intervals.SetRoot("C");
            Assert.Equal("C5 E5 G5", intervals.ToString());
        }

        [Fact]
        public void Create_intervals_with_whole_steps()
        {
            var intervals = Intervals.CreateIntervalsFromNotes("C5 E5 G5");
            Assert.Equal("1 3 5", intervals.ToString());
        }

        [Fact]
        public void Create_intervals_with_half_steps()
        {
            var intervals = Intervals.CreateIntervalsFromNotes("C5 Eb5 G5");
            Assert.Equal("1 b3 5", intervals.ToString());
        }

        [Fact]
        public void As_sequence_with_shorter_nites()
        {
            Intervals intervals = new Intervals("1 3 5").SetRoot("C");
            intervals.AsSequence = "$_i $0q $1h $2w";
            Assert.Equal("C5i E5i G5i C5q E5h G5w", intervals.GetPattern().ToString());
        }

        [Fact]
        public void As_sequence_with_longer_notes()
        {
            Intervals intervals = new Intervals("1 3 5").SetRoot("C");
            intervals.AsSequence = "$0q. $1q $2h";
            Assert.Equal("C5q. E5q G5h", intervals.GetPattern().ToString());
        }
    }
}
