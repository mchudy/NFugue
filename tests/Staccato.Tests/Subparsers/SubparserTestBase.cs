using Moq;
using NFugue.Parser;

namespace Staccato.Tests.Subparsers
{
    public class SubparserTestBase<TSubparser>
    {
        protected Mock<IParser> parser = new Mock<IParser>();
        protected Mock<StaccatoParserContext> context = new Mock<StaccatoParserContext>();
    }
}