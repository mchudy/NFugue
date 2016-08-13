using FluentAssertions;
using NFugue.Parser;
using Staccato.Subparsers;
using Xunit;

namespace Staccato.Tests
{
    public class BarLineSubparserTests : SubparserTestBase<BarLineSubparser>
    {
        [Fact]
        public void Should_match_barline()
        {
            subparser.Matches("|").Should().BeTrue();
        }

        [Fact]
        public void Should_raise_bar_line_parsed()
        {
            ParseWithSubparser("|");
            VerifyEventRaised(nameof(Parser.BarLineParsed))
                .WithArgs<BarLineParsedEventArgs>(e => e.Id == -1);
        }

        [Fact]
        public void Should_raise_bar_line_parsed_with_specifiedId()
        {
            ParseWithSubparser("|200");
            VerifyEventRaised(nameof(Parser.BarLineParsed))
                .WithArgs<BarLineParsedEventArgs>(e => e.Id == 200);
        }
    }
}