using FluentAssertions;
using NFugue.Parser;
using Staccato.Subparsers;
using Xunit;

namespace Staccato.Tests.Subparsers
{
    public class SignatureSubparserTests : SubparserTestBase<SignatureSubparser>
    {
        [Fact]
        public void Should_raise_key_signature_parsed_on_major_key()
        {
            ParseWithSubparser("KEY:Gmaj");
            VerifyEventRaised(nameof(Parser.KeySignatureParsed))
                .WithArgs<KeySignatureParsedEventArgs>(e => e.Key == 7 && e.Scale == 1);
        }

        [Fact]
        public void Should_raise_key_signature_parsed_on_minor_key()
        {
            ParseWithSubparser("KEY:Abmin");
            VerifyEventRaised(nameof(Parser.KeySignatureParsed))
                .WithArgs<KeySignatureParsedEventArgs>(e => e.Key == 8 && e.Scale == -1);
        }

        [Fact]
        public void Should_raise_time_signature_parsed()
        {
            ParseWithSubparser("TIME:6/8");
            VerifyEventRaised(nameof(Parser.TimeSignatureParsed))
                .WithArgs<TimeSignatureParsedEventArgs>(e => e.Numerator == 6 && e.PowerOfTwo == 8);
        }
    }
}