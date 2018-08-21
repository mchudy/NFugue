using FluentAssertions;
using NFugue.Theory;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NFugue.Tests.Theory
{
    public class ChordProgressionTests
    {
        [Fact]
        public void Create_progression_without_key()
        {
            var cp = new ChordProgression("IV V I");
            var pattern = cp.GetPattern();
            pattern.ToString().Should().BeEquivalentTo("F4MAJ G4MAJ C4MAJ");
        }

        [Fact]
        public void Create_progression_with_lower_case()
        {
            var cp = new ChordProgression("iv v i").SetKey(Key.Default);
            var pattern = cp.GetPattern();
            Assert.Equal("F4MIN G4MIN C4MIN", pattern.ToString(), StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Create_progression_with_dashes()
        {
            var cp = new ChordProgression("I-vi7-ii-V7").SetKey(new Key("Amajw"));
            var pattern = cp.GetPattern();
            // pattern.ToString().Should().BeEquivalentTo("A4MAJw F#5MIN7w B4MINw E5MAJ7w");
        }

        [Fact]
        public void Test_get_chord()
        {
            var cp = new ChordProgression("I-vi7-ii-V7"); // This is a turnaround
            Chord[] chords = cp.SetKey(new Key("Amaj")).GetChords();
            var checklist = new List<Chord> { new Chord("A4maj"), new Chord("F#5min7"), new Chord("B4min"), new Chord("E5maj7") };
            checklist.SequenceEqual(chords).Should().BeTrue();
        }

        [Fact]
        public void Test_each_chord_as()
        {
            ChordProgression cp = new ChordProgression("iv v i").EachChordAs("$0q $1q $2q");
            cp.GetPattern().ToString().Should().Be("F4q G#4q C5q G4q Bb4q D5q C4q Eb4q G4q");

            cp = new ChordProgression("I IV V").EachChordAs("$0q $1h $2w");
            cp.GetPattern().ToString().Should().Be("C4q E4h G4w F4q A4h C5w G4q B4h D5w");
        }

        [Fact]
        public void Test_each_chord_as_with_underscore()
        {
            ChordProgression cp = new ChordProgression("I IV V").EachChordAs("$!q $0q $1h $2w");
            cp.GetPattern().ToString().Should().Be("C4MAJq C4q E4h G4w F4MAJq F4q A4h C5w G4MAJq G4q B4h D5w");
        }

        [Fact]
        public void Test_all_chords_as()
        {
            ChordProgression cp = new ChordProgression("iv v i").AllChordsAs("$0q $1q $2q");
            cp.GetPattern().ToString().Should().Be("F4MINq G4MINq C4MINq");

            cp = new ChordProgression("I IV V").AllChordsAs("$0q $1h $2w");
            cp.GetPattern().ToString().Should().Be("C4MAJq F4MAJh G4MAJw");
        }

        [Fact]
        public void Test_all_chords_as_with_underscore()
        {
            ChordProgression cp = new ChordProgression("I IV V").AllChordsAs("$!i $0q $1h $2w");
            cp.GetPattern().ToString().Should().Be("C4MAJi F4MAJi G4MAJi C4MAJq F4MAJh G4MAJw");

            cp = new ChordProgression("I IV V").AllChordsAs("$0q $1h $2w $!i");
            cp.GetPattern().ToString().Should().Be("C4MAJq F4MAJh G4MAJw C4MAJi F4MAJi G4MAJi");
        }

        [Fact]
        public void Test_all_chords_as_with_inversion()
        {
            ChordProgression cp = new ChordProgression("iv v i").AllChordsAs("$0q $0^q $0^^q $1q $1^q $1^^q $2q $2^q $2^^q");
            cp.GetPattern().ToString().Should().Be("F4MINq F4MIN^q F4MIN^^q G4MINq G4MIN^q G4MIN^^q C4MINq C4MIN^q C4MIN^^q");
        }

        [Fact]
        public void Test_each_chord_as_with_inversion()
        {
            ChordProgression cp = new ChordProgression("iv v i").EachChordAs("$!q $!^q $!^^q");
            cp.GetPattern().ToString().Should().Be("F4MINq F4MIN^q F4MIN^^q G4MINq G4MIN^q G4MIN^^q C4MINq C4MIN^q C4MIN^^q");
        }

        [Fact]
        public void Test_all_chords_as_with_underscore_and_inversions()
        {
            ChordProgression cp = new ChordProgression("iv v i").AllChordsAs("$!q $!^q $!^^q");
            cp.GetPattern().ToString().Should().Be("F4MINq G4MINq C4MINq F4MIN^q G4MIN^q C4MIN^q F4MIN^^q G4MIN^^q C4MIN^^q");
        }

        [Fact]
        public void Test_all_chords_as_and_each_chord_as()
        {
            ChordProgression cp = new ChordProgression("I IV V").AllChordsAs("$2 $1 $0").EachChordAs("$2 $1 $0");
            cp.GetPattern().ToString().Should().Be("D5 B4 G4 C5 A4 F4 G4 E4 C4");
        }

        [Fact]
        public void Test_chord_progression_with_inversions()
        {
            ChordProgression cp = new ChordProgression("I II^ III^^ IV^^^ v^^^ vi^^ vii^");
            cp.GetPattern().ToString().Should().Be("C4MAJ D4MAJ^ E4MAJ^^ F4MAJ^^^ G4MIN^^^ A4MIN^^ B4MIN^");
        }

    }
}
