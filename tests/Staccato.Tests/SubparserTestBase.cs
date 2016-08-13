using FluentAssertions;
using FluentAssertions.Events;

namespace Staccato.Tests
{
    //TODO: use mocks
    public class SubparserTestBase<TSubparser> where TSubparser : ISubparser, new()
    {
        protected readonly StaccatoParser parser = new StaccatoParser();
        protected StaccatoParserContext context;
        protected TSubparser subparser = new TSubparser();

        public SubparserTestBase()
        {
            context = new StaccatoParserContext(parser);
            parser.MonitorEvents();
        }

        protected void ParseWithSubparser(string s)
        {
            subparser.Parse(s, context);
        }

        protected IEventRecorder VerifyEventRaised(string name)
        {
            return parser.ShouldRaise(name);
        }
    }
}
