using NFugue.Theory;
using System;
using Xunit;

namespace NFugue.Tests.Theory
{
    public class ChordTests
    {
        [Fact]
        public void Create_chord_with_string()
        {
            var chord = new Chord("Cmaj");
            var pattern = chord.GetPattern();
            Assert.Equal("CMAJ", pattern.ToString(), StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Create_chord_with_numbered_root()
        {
            var chord = new Chord("60maj");
            var pattern = chord.GetPattern();
            Assert.Equal("CMAJ", pattern.ToString(), StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Create_chord_with_intervals()
        {
            var chord = new Chord(new Note("D5h"), new Intervals("1 3 5"));
            var pattern = chord.GetPattern();
            Assert.Equal("D5MAJh", pattern.ToString(), StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Chord_first_inversion_using_number()
        {
            var chord = new Chord("C4maj") { Inversion = 1 };
            VerifyFirstInversion(chord);
        }

        [Fact]
        public void Chord_second_inversion_using_number()
        {
            var chord = new Chord("C4maj") { Inversion = 2 };
            VerifySecondInversion(chord);
        }

        [Fact]
        public void Chord_first_inversion_using_bass_note()
        {
            var chord = new Chord("C4maj");
            chord.SetBassNote("E");
            VerifyFirstInversion(chord);
        }

        [Fact]
        public void Chord_second_inversion_using_bass_note()
        {
            var chord = new Chord("C4maj");
            chord.SetBassNote("G");
            VerifySecondInversion(chord);
        }

        [Fact]
        public void Create_chord_with_notes_using_string_constructor()
        {
            var chord = Chord.FromNotes("C E G");
            Assert.Equal(new Chord("Cmaj"), chord);
        }

        [Fact]
        public void Create_chord_with_notes_using_string_array_constructor()
        {
            var chord = Chord.FromNotes(new[] { "Bb", "Db", "F" });
            Assert.Equal(new Chord("Bbmin^"), chord);
        }

        [Fact]
        public void Create_chord_with_notes_using_note_array_constructor()
        {
            var chord = Chord.FromNotes(new Note[]
            {
                new Note("D"),
                new Note("F#"),
                new Note("A"),
            });
            Assert.Equal(new Chord("Dmaj"), chord);
        }

        [Fact]
        public void Get_chord_type_for_sus4()
        {
            var chord = new Chord("C5sus4");
            Assert.Equal("sus4", chord.GetChordType(), StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Create_three_note_chord_with_notes_in_wrong_order()
        {
            var chord = Chord.FromNotes("E G C");
            Assert.Equal(0, chord.Inversion);
            Assert.Equal(new Chord("Cmaj"), chord);
        }

        [Fact]
        public void Create_three_note_chord_with_inverted_notes()
        {
            var chord = Chord.FromNotes("E4 G4 C5");
            Assert.Equal(1, chord.Inversion);
            Assert.Equal(new Chord("C5maj"), chord);
        }

        [Fact]
        public void Create_four_note_chord_with_inverted_notes_first_inversion()
        {
            var chord = Chord.FromNotes("E4 G4 B4 C5");
            Assert.Equal(1, chord.Inversion);
            Assert.Equal(new Chord("C5maj7^"), chord);
        }

        [Fact]
        public void Create_four_note_chord_with_inverted_notes_second_inversion()
        {
            var chord = Chord.FromNotes("G4 C5 E5 B5");
            Assert.Equal(2, chord.Inversion);
            Assert.Equal(new Chord("Cmaj7^^"), chord);
        }

        [Fact]
        public void Create_four_note_chord_with_inverted_notes_third_inversion()
        {
            var chord = Chord.FromNotes("B4 C5 E5 G5");
            Assert.Equal(3, chord.Inversion);
            Assert.Equal(new Chord("C5maj7^^^"), chord);
            var expectedBassNote = new Note("B4") { IsOctaveExplicitlySet = false };
            Assert.Equal(expectedBassNote, chord.GetBassNote());
        }

        [Fact]
        public void Test_get_bass_note()
        {
            var chord = new Chord("Cmaj^");
            var expectedBassNote = new Note("E3") { IsOctaveExplicitlySet = false };
            Assert.Equal(expectedBassNote, chord.GetBassNote());
        }

        [Fact]
        public void Create_chord_with_notes_in_different_octaves()
        {
            var chord = Chord.FromNotes("C3 E5 G7");
            Assert.Equal(new Chord("Cmaj"), chord);
        }

        [Fact]
        public void Create_chord_with_many_similar_notes()
        {
            var chord = Chord.FromNotes("F3 F4 F5 A6 A5 C4 C3");
            Assert.Equal(new Chord("Fmaj^^"), chord);
        }

        [Fact]
        public void Test_adding_new_chord_type()
        {
            Chord.chordMap["POW"] = new Intervals("1 5");
            var chord = new Chord("Cpow");
            var notes = chord.GetNotes();

            Assert.Equal(48, notes[0].Value); // C3
            Assert.Equal(55, notes[1].Value); // G3
        }

        private static void VerifyFirstInversion(Chord chord)
        {
            var notes = chord.GetNotes();
            Assert.Equal(60, notes[0].Value); // C4
            Assert.Equal(52, notes[1].Value); // E3
            Assert.Equal(55, notes[2].Value); // G3
        }

        private static void VerifySecondInversion(Chord chord)
        {
            var notes = chord.GetNotes();

            Assert.Equal(60, notes[0].Value); // C4
            Assert.Equal(64, notes[1].Value); // E4
            Assert.Equal(55, notes[2].Value); // G3
        }
    }
}
