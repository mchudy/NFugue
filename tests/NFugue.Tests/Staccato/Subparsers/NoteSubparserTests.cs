using FluentAssertions;
using NFugue.Staccato.Subparsers.NoteSubparser;
using NFugue.Theory;
using System;
using NFugue.Parsing;
using Xunit;

namespace Staccato.Tests.Subparsers
{
    public class NoteSubparserTests : SubparserTestBase<NoteSubparser>
    {
        public static TheoryData<string> ValidNoteStrings
            = new TheoryData<string> { "A", "R", "Cb", "B#", "A%", "Ebb", "A##", "A#b", "Eb5" };

        public static TheoryData<string> InvalidNoteStrings
            = new TheoryData<string> { "S", "bC", "I&&" };

        [Theory]
        [MemberData(nameof(ValidNoteStrings))]
        public void Should_match_simple_notes(string s)
        {
            subparser.Matches(s).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(InvalidNoteStrings))]
        public void Should_not_match_invalid_strings(string s)
        {
            subparser.Matches(s).Should().BeFalse();
        }

        [Fact]
        public void Should_parse_simple_notes()
        {
            VerifyNoteParsed("C", new Note(60) { IsOctaveExplicitlySet = false });
            VerifyNoteParsed("C5q", new Note(60) { IsOctaveExplicitlySet = true, Duration = 0.25 });
            VerifyNoteParsed("AWHa90", new Note(69, 1.5d) { OnVelocity = 90 });
        }

        [Fact]
        public void Should_parse_numeric_note()
        {
            VerifyNoteParsed("69", new Note(69));
        }


        [Fact]
        public void Should_parse_numeric_note_with_duration()
        {
            VerifyNoteParsed("60q", new Note(60) { Duration = 0.25 });
        }

        [Fact]
        public void Should_parse_lookup_note()
        {
            VerifyNoteParsed("[BASS_DRUM]q", new Note(36) { Duration = 0.25 });
        }

        [Fact]
        public void Should_parse_lookup_note_in_harmony()
        {
            VerifyNoteParsed("[CLOSED_HI_HAT]q+[BASS_DRUM]q", new Note(36)
            {
                Duration = 0.25,
                IsHarmonicNote = true,
                IsFirstNote = false
            });
        }

        [Fact]
        public void Should_use_default_octave_if_not_specified()
        {
            VerifyNoteParsed("C", new Note(60));
        }

        [Fact]
        public void Should_parse_lowest_note()
        {
            VerifyNoteParsed("C0", new Note(0) { IsOctaveExplicitlySet = true });
        }

        [Fact]
        public void Should_parse_hightest_note()
        {
            VerifyNoteParsed("G10", new Note(127) { IsOctaveExplicitlySet = true });
        }

        [Fact]
        public void Should_throw_given_invalid_octave()
        {
            Action act = () => ParseWithParser("A10");
            act.ShouldThrow<ParserException>();
        }

        [Fact]
        public void Should_parse_triplets()
        {
            VerifyNoteParsed("Cq*5:4", new Note(60, 0.2));
            VerifyNoteParsed("D/0.25*4:5", new Note(62, 0.3125));
            VerifyNoteParsed("C#h*2:4", new Note(61, 1));
        }

        [Fact]
        public void Should_parse_triplets_with_parentheses()
        {
            VerifyNoteParsed("(E C G)q*5:4", new Note(60, 0.2));
        }

        [Fact]
        public void Should_use_default_duration_for_triplets()
        {
            VerifyNoteParsed("C*5:4", new Note(60, 0.2));
        }

        [Fact]
        public void Should_use_default_tuplet_for_triplets()
        {
            VerifyNoteParsed("C*", new Note(60, 0.166666666666666666666d));
            VerifyNoteParsed("C*3:2", new Note(60, 0.166666666666666666666d));
        }

        [Fact]
        public void Should_parse_ties_with_letter_duration()
        {
            VerifyNoteParsed("Cw-", new Note(60, 1)
            {
                IsStartOfTie = true,
                IsEndOfTie = false
            });
            VerifyNoteParsed("C-w", new Note(60, 1)
            {
                IsStartOfTie = false,
                IsEndOfTie = true
            });
            VerifyNoteParsed("C-w-", new Note(60, 1)
            {
                IsStartOfTie = true,
                IsEndOfTie = true
            });
        }

        [Fact]
        public void Should_parse_ties_with_numeric_duration()
        {
            VerifyNoteParsed("C/1.0-", new Note(60, 1)
            {
                IsStartOfTie = true,
                IsEndOfTie = false
            });
            VerifyNoteParsed("C/-1.0", new Note(60, 1)
            {
                IsStartOfTie = false,
                IsEndOfTie = true
            });
            VerifyNoteParsed("C/-1.0-", new Note(60, 1)
            {
                IsStartOfTie = true,
                IsEndOfTie = true
            });
        }

        [Fact]
        public void Should_parse_quantity_duration()
        {
            VerifyNoteParsed("Cw9", new Note(60, 9));
        }

        [Fact]
        public void Should_parse_dynamics_with_letters()
        {
            VerifyNoteParsed("Ca20", new Note(60)
            {
                OnVelocity = 20
            });
            VerifyNoteParsed("D4d20", new Note(50)
            {
                IsOctaveExplicitlySet = true,
                OffVelocity = 20
            });
            VerifyNoteParsed("Bb3a10d20", new Note(46)
            {
                IsOctaveExplicitlySet = true,
                OnVelocity = 10,
                OffVelocity = 20
            });
        }

        [Fact]
        public void Should_parse_dynamics_with_durations()
        {
            VerifyNoteParsed("Ch.a20", new Note(60, 0.75)
            {
                OnVelocity = 20
            });
            VerifyNoteParsed("D4/0.25d20", new Note(50, 0.25)
            {
                IsOctaveExplicitlySet = true,
                OffVelocity = 20
            });
            VerifyNoteParsed("c2w2a2", new Note(24, 2)
            {
                IsOctaveExplicitlySet = true,
                OnVelocity = 2
            });
            VerifyNoteParsed("Bb3/-1.0-a10d20", new Note(46, 1)
            {
                IsOctaveExplicitlySet = true,
                IsStartOfTie = true,
                IsEndOfTie = true,
                OnVelocity = 10,
                OffVelocity = 20
            });
        }

        [Fact]
        public void Should_parse_simple_major_chord()
        {
            VerifyChordParsed("C4maj", new Note(48) { IsOctaveExplicitlySet = true }, "MAJ");
        }

        [Fact]
        public void Should_parse_simple_minor_chord()
        {
            VerifyChordParsed("AMIN/1.5", new Note(57, 1.5), "MIN");
        }

        [Fact]
        public void Should_use_default_octave_for_chords()
        {
            VerifyChordParsed("Cmaj", new Note(48), "MAJ");
        }

        [Fact]
        public void Should_parse_combined_letter_notes()
        {
            VerifyNoteParsed("E4q+C4q", new Note(48, 0.25)
            {
                IsOctaveExplicitlySet = true,
                IsFirstNote = false,
                IsHarmonicNote = true
            });
        }

        [Fact]
        public void Should_parse_combined_numeric_notes()
        {
            VerifyNoteParsed("65/0.25+60/0.25", new Note(60, 0.25)
            {
                IsFirstNote = false,
                IsHarmonicNote = true
            });
        }

        [Fact]
        public void Should_recognize_internal_intervals()
        {
            VerifyNoteParsed("C'6q", new Note(69, 0.25d));
        }

        [Fact]
        public void Should_recognize_internal_intervals_with_flat()
        {
            VerifyNoteParsed("C'b3q", new Note("Ebq"));
        }

        [Fact]
        public void Should_recognize_internal_intervals_with_double_flat()
        {
            VerifyNoteParsed("C'6bbq", new Note(67, 0.25d));
        }

        [Fact]
        public void Should_recognize_internal_intervals_with_sharp()
        {
            VerifyNoteParsed("C'#6q", new Note(70, 0.25d));
        }

        [Fact]
        public void Should_recognize_internal_intervals_with_double_sharp()
        {
            VerifyNoteParsed("C'6##q", new Note(71, 0.25d));
        }

        private void VerifyNoteParsed(string s, Note note)
        {
            ParseWithParser(s);
            VerifyEventRaised(nameof(Parser.NoteParsed))
                .WithArgs<NoteEventArgs>(e => e.Note.Equals(note));
        }

        private void VerifyChordParsed(string s, Note note, string intervalsString)
        {
            ParseWithParser(s);
            var intervals = Chord.GetIntervals(intervalsString);
            var chord = new Chord(note, intervals);
            VerifyEventRaised(nameof(Parser.ChordParsed))
                .WithArgs<ChordParsedEventArgs>(e => e.Chord.Equals(chord));
        }
    }
}