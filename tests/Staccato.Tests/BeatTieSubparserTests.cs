using FluentAssertions;
using NFugue.Parser;
using Staccato.Subparsers;
using Xunit;

namespace Staccato.Tests
{
    public class BeatTieSubparserTests
    {
        private StaccatoParser parser = new StaccatoParser();
        private BeatTimeSubparser subparser = new BeatTimeSubparser();
        private StaccatoParserContext context = new StaccatoParserContext(null);

        public BeatTieSubparserTests()
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
    }
}
