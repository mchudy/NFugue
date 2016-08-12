using Staccato.Subparsers;
using Xunit;

namespace Staccato.Tests
{
    public class BeatTieSubparserTests
    {
        private BeatTimeSubparser subparser = new BeatTimeSubparser();
        private StaccatoParserContext context = new StaccatoParserContext(null);

        [Fact]
        public void Track_beat_time_requested_should_be_raised()
        {
            //Assert.Equal("");
        }
    }
}
