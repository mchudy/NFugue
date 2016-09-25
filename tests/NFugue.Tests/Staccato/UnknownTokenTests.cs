using FluentAssertions;
using NFugue.Parsing;
using NFugue.Staccato;
using System;
using Xunit;

namespace Staccato.Tests
{
    public class UnknownTokenTests
    {
        private readonly StaccatoParser parser = new StaccatoParser();

        [Fact]
        public void Should_ignore_unknown_token_by_default()
        {
            parser.Parse("UNKNOWN");
        }

        [Fact]
        public void Should_throw_exception_if_flag_set()
        {
            parser.ThrowsExceptionOnUnknownToken = true;
            Action action = () => parser.Parse("UNKNOWN");
            action.ShouldThrow<ParserException>();
        }
    }
}