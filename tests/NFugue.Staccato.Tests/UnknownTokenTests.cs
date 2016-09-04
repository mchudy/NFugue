using FluentAssertions;
using NFugue.Parser;
using NFugue.Playing;
using System;
using Xunit;

namespace Staccato.Tests
{
    public class UnknownTokenTests
    {
        private readonly Player player = new Player();

        [Fact]
        public void Should_ignore_unknown_token_by_default()
        {
            player.Play("UKNOWN");
        }

        [Fact]
        public void Should_throw_exception_if_flag_set()
        {
            player.Parser.ThrowsExceptionOnUnknownToken = true;
            Action action = () => player.Play("UNKNOWN");
            action.ShouldThrow<ParserException>();
        }
    }
}