using FluentAssertions;
using NFugue.Parser;
using Staccato.Subparsers;
using Xunit;

namespace Staccato.Tests
{
    public class LyricMarkerSubparserTests : SubparserTestBase<LyricMarkerSubparser>
    {
        [Fact]
        public void Should_parse_lyric_without_parentheses()
        {
            ParseWithSubparser("'lyric");
            VerifyEventRaised(nameof(Parser.LyricParsed))
                .WithArgs<LyricParsedEventArgs>(e => e.Lyric == "lyric");
        }

        [Fact]
        public void Should_parse_lyric_with_parentheses()
        {
            ParseWithSubparser("'(three word lyric)");
            VerifyEventRaised(nameof(Parser.LyricParsed))
                .WithArgs<LyricParsedEventArgs>(e => e.Lyric == "three word lyric");
        }

        [Fact]
        public void Should_parse_marker_without_parentheses()
        {
            ParseWithSubparser("#marker");
            VerifyEventRaised(nameof(Parser.MarkerParsed))
                .WithArgs<MarkerParsedEventArgs>(e => e.Marker == "marker");
        }

        [Fact]
        public void Should_parse_marker_with_parentheses()
        {
            ParseWithSubparser("#(three word marker)");
            VerifyEventRaised(nameof(Parser.MarkerParsed))
                .WithArgs<MarkerParsedEventArgs>(e => e.Marker == "three word marker");
        }
    }
}