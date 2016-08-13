using FluentAssertions;
using NFugue.Parser;
using Xunit;

namespace Staccato.Tests
{
    public class BeatTimeSubparserTests
    {
        private readonly StaccatoParser parser = new StaccatoParser();

        public BeatTimeSubparserTests()
        {
            parser.MonitorEvents();
        }

        [Fact]
        public void Track_beat_time_requested_should_be_raised()
        {
            parser.Parse("@200");
            parser.ShouldRaise(nameof(Parser.TrackBeatTimeRequested))
                .WithArgs<TrackBeatTimeRequestedEventArgs>(e => e.Time == 200);
        }

        [Fact]
        public void Track_beat_time_bookmarked_should_be_raised()
        {
            parser.Parse("@#mark");
            parser.ShouldRaise(nameof(Parser.TrackBeatTimeBookmarkRequested))
                .WithArgs<TrackBeatTimeBookmarkEventArgs>(e => e.TimeBookmarkId == "mark");
        }
    }
}
