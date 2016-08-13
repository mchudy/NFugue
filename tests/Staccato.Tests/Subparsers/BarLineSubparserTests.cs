using FluentAssertions;
using Moq;
using NFugue.Parser;
using Staccato.Subparsers;
using Xunit;

namespace Staccato.Tests.Subparsers
{
    public class BarLineSubparserTests
    {
        private Mock<IParser> parser = new Mock<IParser>();
        private Mock<IParserContext> context = new Mock<IParserContext>();
        private BarLineSubparser subparser = new BarLineSubparser();

        public BarLineSubparserTests()
        {
            context.SetupGet(c => c.Parser).Returns(parser.Object);
        }

        [Fact]
        public void Should_raise_bar_line_parsed()
        {
            subparser.Parse("|", context.Object);
            parser.R
            parser.ShouldRaise(nameof(Parser.BarLineParsed))
                .WithArgs<BarLineParsedEventArgs>(e => e.Id == -1);
        }

        [Fact]
        public void Should_raise_bar_line_parsed_with_specifiedId()
        {
            subparser.Parse("|200", context.Object);
            parser.ShouldRaise(nameof(Parser.BarLineParsed))
                .WithArgs<BarLineParsedEventArgs>(e => e.Id == 200);
        }
    }
}