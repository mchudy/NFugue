using FluentAssertions;
using NFugue.Parsing;
using NFugue.Staccato.Subparsers;
using Xunit;

namespace Staccato.Tests.Subparsers
{
    public class TempoSubparserTests : SubparserTestBase<TempoSubparser>
    {
        [Fact]
        public void Should_match_T_sign()
        {
            subparser.Matches("T").Should().BeTrue();
        }

        [Fact]
        public void Should_raise_event_with_given_tempo_value()
        {
            ParseWithSubparser("T180");
            VerifyEventRaised(nameof(Parser.TempoChanged))
                .WithArgs<TempoChangedEventArgs>(e => e.TempoBPM == 180);
        }

        [Fact]
        public void Should_raise_event_with_minus_one_value_when_no_BPM_specified()
        {
            ParseWithSubparser("T");
            VerifyEventRaised(nameof(Parser.TempoChanged))
                .WithArgs<TempoChangedEventArgs>(e => e.TempoBPM == -1);
        }

    }
}