using FluentAssertions;
using NFugue.Parsing;
using NFugue.Staccato.Subparsers;
using NFugue.Theory;
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
        public void Key_signature_should_change_subsequent_note()
        {
            var expectedNote = new Note(66, 0.25f) { IsOctaveExplicitlySet = true };
            VerifySubsequentNote("KEY:GMaj F5q", expectedNote);
        }

        [Fact]
        public void Key_signature_with_sharps_should_change_subsequent_note()
        {
            var expectedNote = new Note(66, 0.25f) { IsOctaveExplicitlySet = true };
            VerifySubsequentNote("KEY:K# F5q", expectedNote);
        }

        [Fact]
        public void Key_signature_with_many_sharps_should_change_subsequent_note()
        {
            var expectedNote = new Note(63, 0.25f) { IsOctaveExplicitlySet = true };
            VerifySubsequentNote("KEY:K#### D5q", expectedNote);
        }

        [Fact]
        public void Key_signature_with_flats_should_change_subsequent_note()
        {
            var expectedNote = new Note(70, 0.25f) { IsOctaveExplicitlySet = true };
            VerifySubsequentNote("KEY:Kb B5q", expectedNote);
        }

        [Fact]
        public void Key_signature_with_many_flats_should_change_subsequent_note()
        {
            var expectedNote = new Note(61, 0.25f) { IsOctaveExplicitlySet = true };
            VerifySubsequentNote("KEY:Kbbbb D5q", expectedNote);
        }

        [Fact]
        public void Should_raise_time_signature_parsed()
        {
            ParseWithSubparser("TIME:6/8");
            VerifyEventRaised(nameof(Parser.TimeSignatureParsed))
                .WithArgs<TimeSignatureParsedEventArgs>(e => e.Numerator == 6 && e.PowerOfTwo == 8);
        }

        private void VerifySubsequentNote(string s, Note expectedNote)
        {
            ParseWithParser(s);
            VerifyEventRaised(nameof(Parser.NoteParsed))
                .WithArgs<NoteEventArgs>(e => e.Note.Equals(expectedNote));
        }

    }
}