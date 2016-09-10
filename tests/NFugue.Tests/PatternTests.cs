using FluentAssertions;
using NFugue.Patterns;
using System;
using Xunit;

namespace NFugue.Tests
{
    public class PatternTests
    {
        [Fact]
        public void Should_set_tempo_from_string()
        {
            Pattern pattern = new Pattern("A B C");
            pattern.SetTempo("Adagio");
            pattern.ToString().Should().Be("T60 A B C");
        }

        [Fact]
        public void Should_set_tempo_from_number()
        {
            Pattern pattern = new Pattern("A B C");
            pattern.SetTempo(60);
            pattern.ToString().Should().Be("T60 A B C");
        }

        [Fact]
        public void Given_invalid_tempo_should_throw()
        {
            Pattern pattern = new Pattern("A B C");
            Action act = () => pattern.SetTempo("Unknown");
            act.ShouldThrow<ApplicationException>();
        }

        [Fact]
        public void Should_set_instrument_from_string_ignoring_case()
        {
            Pattern pattern = new Pattern("A B C");
            pattern.SetInstrument("FlUtE");
            pattern.ToString().Should().Be("I[Flute] A B C");
        }

        [Fact]
        public void Should_set_instrument_from_number()
        {
            Pattern pattern = new Pattern("A B C");
            pattern.SetInstrument(60);
            pattern.ToString().Should().Be("I[French_Horn] A B C");
        }

        [Fact]
        public void Given_invalid_instrument_should_throw()
        {
            Pattern pattern = new Pattern("A B C");
            Action act = () => pattern.SetInstrument("Unknown");
            act.ShouldThrow<ApplicationException>();
        }

        [Fact]
        public void Should_set_voice()
        {
            Pattern pattern = new Pattern("A B C");
            pattern.SetVoice(1);
            pattern.ToString().Should().Be("V1 A B C");
        }

        [Fact]
        public void Should_set_tempo_instrument_and_voice_in_correct_order()
        {
            Pattern pattern = new Pattern("A B C").SetTempo("Allegro").SetInstrument(0).SetVoice(8);
            pattern.ToString().Should().Be("T120 V8 I[Piano] A B C");
        }

        [Fact]
        public void Should_add_token_to_each_note_given_one_decorator_which_should_be_applied_to_each_of_3_notes()
        {
            Pattern pattern = new Pattern("T60 A | B V1 C").AddToEachNoteToken("q");
            pattern.ToString().Should().Be("T60 Aq | Bq V1 Cq");
        }

        [Fact]
        public void Should_add_token_to_each_note_given_fewer_decorators_than_notes_in_the_pattern()
        {
            Pattern pattern = new Pattern("T60 A | B V1 C").AddToEachNoteToken("q i");
            pattern.ToString().Should().Be("T60 Aq | Bi V1 Cq");
        }

        [Fact]
        public void Should_add_token_to_each_note_given_an_equal_number_of_decorators_and_notes_in_the_pattern()
        {
            Pattern pattern = new Pattern("T60 A | B V1 C").AddToEachNoteToken("q i s");
            pattern.ToString().Should().Be("T60 Aq | Bi V1 Cs");
        }

        [Fact]
        public void Should_add_token_to_each_note_given_more_decorators_than_notes_in_the_pattern()
        {
            Pattern pattern = new Pattern("T60 A | B V1 C").AddToEachNoteToken("q i s w");
            pattern.ToString().Should().Be("T60 Aq | Bi V1 Cs");
        }

        [Fact]
        public void Should_add_token_to_each_note_when_pattern_contains_percussion_instruments()
        {
            Pattern pattern = new Pattern("[BASS_DRUM] [HI_HAT] [OPEN_CUICA]").AddToEachNoteToken("q i s w").SetVoice(9);
            pattern.ToString().Should().Be("V9 [BASS_DRUM]q [HI_HAT]i [OPEN_CUICA]s");
        }

        [Fact]
        public void Should_do_nothing_when_there_are_no_note_elements_in_the_pattern()
        {
            Pattern pattern = new Pattern("T120 V9 I[Piano]")
                .AddToEachNoteToken("q i s w");
            pattern.ToString().Should().Be("T120 V9 I[PIANO]");
        }

        [Fact]
        public void Test_repeat()
        {
            Pattern pattern = new Pattern("A B").Repeat(3);
            pattern.ToString().Should().Be("A B A B A B");
        }

        [Fact]
        public void Should_repeat_after_settings_tempo_and_voice()
        {
            Pattern pattern = new Pattern("A B").SetTempo(20).SetVoice(1).Repeat(3);
            pattern.ToString().Should().Be("T20 V1 A B A B A B");
        }

        [Fact]
        public void Should_repeat_before_setting_tempo_and_voice()
        {
            Pattern pattern = new Pattern("A B").Repeat(3).SetTempo(20).SetVoice(1);
            pattern.ToString().Should().Be("T20 V1 A B A B A B");
        }

        [Fact]
        public void Pattern_after_clear_should_not_contain_previous_elements()
        {
            Pattern pattern = new Pattern("A B").Clear().Add("C D");
            pattern.ToString().Should().Be("C D");
        }

        [Fact]
        public void Should_prepend_multiple_patterns()
        {
            Pattern pattern = new Pattern("D D");
            pattern.Prepend(new Pattern("A A"), new Pattern("B B"), new Pattern("C C"));
            pattern.ToString().Should().Be("A A B B C C D D");
        }
    }
}