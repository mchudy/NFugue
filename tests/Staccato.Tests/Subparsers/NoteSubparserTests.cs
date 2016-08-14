using FluentAssertions;
using NFugue.Parser;
using NFugue.Theory;
using Staccato.Subparsers.NoteSubparser;
using Xunit;

namespace Staccato.Tests.Subparsers
{
    public class NoteSubparserTests : SubparserTestBase<NoteSubparser>
    {
        [Fact]
        public void Should_match_simple_notes()
        {
            subparser.Matches("A").Should().BeTrue();
            subparser.Matches("R").Should().BeTrue();
            subparser.Matches("S").Should().BeFalse();

            subparser.Matches("Cb").Should().BeTrue();
            subparser.Matches("B#").Should().BeTrue();
            subparser.Matches("bC").Should().BeFalse();
            subparser.Matches("A%").Should().BeTrue();

            subparser.Matches("Ebb").Should().BeTrue();
            subparser.Matches("A##").Should().BeTrue();
            subparser.Matches("A#b").Should().BeTrue();
            subparser.Matches("I&&").Should().BeFalse();

            subparser.Matches("Eb5").Should().BeTrue();
        }

        [Fact]
        public void Should_raise_note_parsed_on_simple_notes()
        {
            ParseWithSubparser("C");
            VerifyEventRaised(nameof(Parser.NoteParsed))
                .WithArgs<NoteEventArgs>(e => e.Note.Equals(new Note(60) { IsOctaveExplicitlySet = false }));
        }
    }
}