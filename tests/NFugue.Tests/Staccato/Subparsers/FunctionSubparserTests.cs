using FluentAssertions;
using Moq;
using NFugue.Parsing;
using NFugue.Staccato;
using NFugue.Staccato.Functions;
using NFugue.Staccato.Subparsers;
using Xunit;

namespace Staccato.Tests.Subparsers
{
    public class FunctionSubparserTests : SubparserTestBase<FunctionSubparser>
    {
        private readonly Mock<ISubparserFunction> function = new Mock<ISubparserFunction>();

        public FunctionSubparserTests()
        {
            function.Setup(f => f.GetNames()).Returns(new[] { "CP" });
            FunctionManager.Instance.AddSubparserFunction(function.Object);
        }

        [Fact]
        public void Should_match_colon()
        {
            subparser.Matches(":ab").Should().BeTrue();
        }

        [Fact]
        public void Should_parse_function_with_single_argument()
        {
            ParseWithSubparser(":CP(7)");

            VerifyEventRaised(nameof(Parser.FunctionParsed))
                .WithArgs<FunctionParsedEventArgs>(e => e.Id == "CP" && e.Message.Equals("7"));
        }

        [Fact]
        public void Should_parse_function_with_multiple_argument()
        {
            ParseWithSubparser(":CP(7,8)");

            VerifyEventRaised(nameof(Parser.FunctionParsed))
                .WithArgs<FunctionParsedEventArgs>(e => e.Id == "CP" && e.Message.Equals("7,8"));
        }
    }
}