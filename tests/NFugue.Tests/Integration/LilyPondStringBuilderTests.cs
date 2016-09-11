using FluentAssertions;
using NFugue.Integration.LilyPond;
using NFugue.Patterns;
using NFugue.Staccato;
using Xunit;

namespace NFugue.Tests.Integration
{
    public class LilyPondStringBuilderTests
    {
        private readonly StaccatoParser parser = new StaccatoParser();
        private readonly LilyPondStringBuilder lilyPondBuilder;
        private Pattern pattern;

        public LilyPondStringBuilderTests()
        {
            lilyPondBuilder = new LilyPondStringBuilder(parser);
        }

        [Fact]
        public void Single_note()
        {
            CompareStrings("A4q", "a'4");
        }

        [Theory]
        [InlineData("A4q B4w C4i", "a'4 b1 c,8")]
        [InlineData("A4q E4q", "a'4 e4")]
        [InlineData("G4i B4i", "g'8 b8")]
        [InlineData("G3i C4i E4i C4i G3i", "g8 c8 e8 c8 g8")]
        public void Multiple_notes(string nFugueString, string expectedLilyPondString)
        {
            CompareStrings(nFugueString, expectedLilyPondString);
        }

        [Fact]
        public void Sharps_and_flats()
        {
            CompareStrings("A#4 Bb4", "ais'4 bes4");
        }

        [Fact]
        public void Rest_notes()
        {
            CompareStrings("Rq Ri Rw", "r4 r8 r1");
        }

        [Fact]
        public void DottedNotes()
        {
            CompareStrings("Rq. B4h.", "r4. b'2.");
        }

        [Fact]
        public void Chord()
        {
            CompareStrings("C4maj", "<c e g>4");
        }

        [Theory]
        [InlineData("B3q+C#5q", "<b cis'>4")]
        [InlineData("A4q+B4q", "<a' b>4")]
        [InlineData("A4q+B4q+C4q", "<a' b c,>4")]
        [InlineData("C4h+C5q_B4i_E4i_G4i_B4i", "<< { c2 } \\\\ { c'4 b8 e,8 g8 b8 } >>")]
        [InlineData("Dq+Fq+Aq+Bbq+F#q", "<d' f a bes fis>4")]
        public void Parallel_notes(string nFugueString, string expectedLilyPondString)
        {
            CompareStrings(nFugueString, expectedLilyPondString);
        }

        [Fact]
        public void Polyphone_notes()
        {
            CompareStrings("A4w+B4q_C4q_D4h", "<< { a'1 } \\\\ { b4 c,4 d2 } >>");
        }

        [Fact]
        public void Dotted_chord()
        {
            CompareStrings("C4majq.", "<c e g>4.");
        }

        [Theory]
        [InlineData("E4minq G4i B4i", "<e g b>4 g8 b8")]
        [InlineData("E4 G4i B4i", "e4 g8 b8")]
        [InlineData("C3MAJq F3MAJq G3MAJq C3MAJq", "<c, e g>4 <f a c>4 <g b d>4 <c, e g>4")]
        public void Chord_with_octave(string nFugueString, string expectedLilyPondString)
        {
            CompareStrings(nFugueString, expectedLilyPondString);
        }

        [Fact]
        public void Notes_with_octaves()
        {
            CompareStrings("C3q F3q G3q C3q", "c,4 f4 g4 c,4");
        }

        [Fact]
        public void Octave_up_down()
        {
            CompareStrings("D4w+G5q_F#5q_E5i_A4i_D5q",
                "<< { d1 } \\\\ { g'4 fis4 e8 a,8 d4 } >>");
        }

        [Fact]
        public void Octave_with_rest()
        {
            CompareStrings("Rq E4i E4q E4q Ri Rq D4i D4q C4q Ri",
                "r4 e8 e4 e4 r8 r4 d8 d4 c4 r8");
        }

        [Fact]
        public void Octave_first_note_up()
        {
            CompareStrings("B5i ", "b''8");
        }

        [Fact]
        public void Octave_first_note_up_second_down()
        {
            CompareStrings("B5i D4i ", "b''8 d,,8");
        }

        [Fact]
        public void Single_pattern()
        {
            CompareStrings("F4w+E5q_A4i_D5i_E5q_G5q",
                "<< { f1 } \\\\ { e'4 a,8 d8 e4 g4 } >>");
        }

        [Theory]
        [InlineData("F4w+E5q_A4i_D5i_E5q_G5q  F4w+E5q_A4i_D5i_E5q_D5q",
                "<< { f1 } \\\\ { e'4 a,8 d8 e4 g4 } >><< { f,1 } \\\\ { e'4 a,8 d8 e4 d4 } >>")]
        [InlineData("C4h+G4q_E4q_G4q_E4i  B4w+Rq._E4q_G4q_E4q",
                "<< { c2 } \\\\ { g'4 e4 g4 e8 } >><< { b'1 } \\\\ { r4. e,4 g4 e4 } >>")]
        public void Two_patterns(string nFugueString, string expectedLilyPondString)
        {
            CompareStrings(nFugueString, expectedLilyPondString);
        }

        [Fact]
        public void Octave_third_note_down()
        {
            CompareStrings("B4i E4i A4i B4i E4i A4i B4q C5i E4i A4i C5i E4i A4i C5q",
                "b'8 e,8 a8 b8 e,8 a8 b4 c8 e,8 a8 c8 e,8 a8 c4");
        }

        [Fact]
        public void Increasing_octaves()
        {
            CompareStrings("V1 I0 C2 C3 C4 C5 C6 C7 C8 Rh",
                "\\set Staff.instrumentName = \"Piano\" c,,4 c'4 c'4 c'4 c'4 c'4 c'4 r2");
        }

        [Theory]
        [InlineData("B4i E4i A4i B4i E4i A4i B4q C5i E4i A4i C5i E4i A4i C5q",
                "b'8 e,8 a8 b8 e,8 a8 b4 c8 e,8 a8 c8 e,8 a8 c4")]
        [InlineData("B4i D4i A4i", "b'8 d,8 a'8")]
        public void Octave_up(string nFugueString, string expectedLilyPondString)
        {
            CompareStrings(nFugueString, expectedLilyPondString);
        }

        [Fact]
        public void Octave_down()
        {
            CompareStrings("B4i E4i A4i B4i", "b'8 e,8 a8 b8");
        }

        [Fact]
        public void Two_parallel_plus_one_note()
        {
            CompareStrings("C4h+G4q  E4q+B4q A4q", "<c g'>4 <e b'>4 a4");
        }

        [Fact]
        public void One_note_plus_parallel_notes()
        {
            CompareStrings("A4q C4h+G4q", "a'4 <c, g'>4");
        }

        [Fact]
        public void Parallel_duration_change()
        {
            CompareStrings("E4q+A4q E4i+A4i", "<e a>4 <e a>8");
        }

        [Fact]
        public void Parallel_note_change()
        {
            CompareStrings("E4q+C5q Ri E4q+B4q", "<e c'>4 r8 <e, b'>4");
        }

        [Fact]
        public void Parallel_octave_change()
        {
            CompareStrings("B4i+D#5i+F#5i Ri D4q E4q", "<b' dis fis>8 r8 d,4 e4");
        }

        [Fact]
        public void Three_staves()
        {
            CompareWithMultipleStaves("V0 I16 A4q V1 I20 B4q V2 I24 C4q", "\\new Staff { \\set Staff.instrumentName = \"Drawbar_Organ\" a'4 }\n\\new Staff { \\set Staff.instrumentName = \"Reed_Organ\" b4 }\n\\new Staff { \\set Staff.instrumentName = \"Guitar\" c,4 }");
        }

        [Fact]
        public void Two_staves_in_parallel()
        {
            CompareWithMultipleStaves("V0 I16 A4q V1 I20 B4q+A4q V2 I24 C4q", "\\new Staff { \\set Staff.instrumentName = \"Drawbar_Organ\" a'4 }\n\\new Staff { \\set Staff.instrumentName = \"Reed_Organ\" <b a>4 }\n\\new Staff { \\set Staff.instrumentName = \"Guitar\" c,4 }");
        }

        [Fact]
        public void Applause()
        {
            CompareStrings("B6s Ri B6s Ri B6s Ri B6www", "b'''16 r8 b16 r8 b16 r8 b\\breve.");
        }

        [Theory]
        [InlineData("Rh G4h F#4q E4q D#4h- D#4-h Rh", "r2 g'2 fis4 e4 dis2 ~ dis2 r2")]
        [InlineData("D#4h- D#4-h", "dis2 ~ dis2")]
        public void Tie(string nFugueString, string expectedLilyPondString)
        {
            CompareStrings(nFugueString, expectedLilyPondString);
        }

        [Fact]
        public void Note_with_length_3()
        {
            CompareStrings("I[ROCK_ORGAN] B4www", "\\set Staff.instrumentName = \"Rock_Organ\" b'\\breve.");
        }

        private void CompareStrings(string nFugueString, string lilyPondString)
        {
            pattern = new Pattern(nFugueString);
            parser.Parse(pattern);
            lilyPondBuilder.GetLilyPondString().Should()
                .Be($"\\new Staff {{ {lilyPondString} }}");
        }

        private void CompareWithMultipleStaves(string nFugueString, string lilyPondString)
        {
            pattern = new Pattern(nFugueString);
            parser.Parse(pattern);
            lilyPondBuilder.GetLilyPondString().Should().Be(lilyPondString);
        }
    }
}