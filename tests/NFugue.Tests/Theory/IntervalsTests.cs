using FluentAssertions;
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
            intervals.GetNthInterval(0).Should().Be("1");
            intervals.GetNthInterval(1).Should().Be("3");
            intervals.GetNthInterval(2).Should().Be("5");
        }

        [Fact]
        public void Nth_interval_for_index_outside_of_range_should_throw()
        {
            var intervals = new Intervals("1 3 5");
            Action act = () => intervals.GetNthInterval(3);

            act.ShouldThrow<IndexOutOfRangeException>();
        }

        [Fact]
        public void Test_rotate()
        {
            var intervals = new Intervals("1 3 5");
            intervals.Rotate(1);
            intervals.ToString().Should().Be("3 5 1");
        }


        [Fact]
        public void Test_has()
        {
            var intervals = new Intervals("1 3 5");
            intervals.SetRoot("C");
            intervals.Has("E").Should().BeTrue();
            intervals.Has("F").Should().BeFalse();
        }

        [Fact]
        public void Test_has_with_octaves()
        {
            var intervals = new Intervals("1 3 5");
            intervals.SetRoot("D5");
            intervals.Has("F#2").Should().BeTrue();
            intervals.Has("G5").Should().BeFalse();
        }

        [Fact]
        public void Test_as1()
        {
            var intervals = new Intervals("1 3 5").SetRoot("C").As("$!i $0q $1h $2w");
            Assert.Equal(intervals.GetPattern().ToString(), "C5i E5i G5i C5q E5h G5w", true);
        }


        [Fact]
        public void Test_pattern_with_root()
        {
            var intervals = new Intervals("1 3 5");
            intervals.SetRoot("C");

            intervals.GetPattern().ToString().Should().Be("C5 E5 G5");
        }

        [Fact]
        public void Create_intervals_with_whole_steps()
        {
            var intervals = Intervals.CreateIntervalsFromNotes("C5 E5 G5");
            intervals.ToString().Should().Be("1 3 5");
        }

        [Fact]
        public void Create_intervals_with_half_steps()
        {
            var intervals = Intervals.CreateIntervalsFromNotes("C5 Eb5 G5");
            intervals.ToString().Should().Be("1 b3 5");
        }

        [Fact]
        public void As_sequence_with_shorter_notes()
        {
            Intervals intervals = new Intervals("1 3 5").SetRoot("C");
            intervals.AsSequence = "$!i $0q $1h $2w";
            intervals.GetPattern().ToString().Should().Be("C5i E5i G5i C5q E5h G5w");
        }

        [Fact]
        public void As_sequence_with_longer_notes()
        {
            Intervals intervals = new Intervals("1 3 5").SetRoot("C");
            intervals.AsSequence = "$0q. $1q $2h";
            intervals.GetPattern().ToString().Should().Be("C5q. E5q G5h");
        }
    }
}
