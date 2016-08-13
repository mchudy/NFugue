using FluentAssertions;
using NFugue.Parser;
using Xunit;

namespace Staccato.Tests
{
    public class BarLineSubparserTests
    {
        private StaccatoParser parser = new StaccatoParser();

        public BarLineSubparserTests()
        {
            parser.MonitorEvents();
        }

        [Fact]
        public void Should_raise_bar_line_parsed()
        {
            parser.Parse("|");
            parser.ShouldRaise(nameof(Parser.BarLineParsed))
                .WithArgs<BarLineParsedEventArgs>(e => e.Id == -1);
        }

        [Fact]
        public void Should_raise_bar_line_parsed_with_specifiedId()
        {
            parser.Parse("|200");
            parser.ShouldRaise(nameof(Parser.BarLineParsed))
                .WithArgs<BarLineParsedEventArgs>(e => e.Id == 200);
        }
    }
}