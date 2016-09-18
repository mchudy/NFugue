using NFugue.ManualTests.Utils;
using NFugue.Patterns;
using NFugue.Playing;
using NFugue.Staccato.Functions;

namespace NFugue.ManualTests.Tests
{
    public class StaccatoTests
    {
        private readonly Player player = new Player();

        [ManualTest("A440", "These next four notes should sound exactly identical")]
        public void A440Test()
        {
            player.Play("A5 69 m440 A");
        }

        [ManualTest("Arpeggio", "Should play arpeggiated C major chord")]
        public void ArpeggiatedChordTest()
        {
            FunctionManager.Instance.AddPreprocessorFunction(new ArpeggiatedChordFunction());

            player.Play(":ARPEGGIATED(Cmajw)");
        }

        [ManualTest("Attack and decay", "You should hear one note in five different ways")]
        public void AttackDecayTest()
        {
            player.Play("V1 I[Flute] Cq Rw Dwwa10d10 Rw Ewwa100d10 Rw Fwwa10d100 Rw Gwwa100d100");
        }

        [ManualTest("Chord inversions",
            @"The C Major note should play through its three inversions:
             - One chord without an inversion.
             - Two identical-sounding chords for the first inversion.
             - Two identical-sounding chords for the second inversion.
        ")]
        public void ChordInversionsTest()
        {
            Pattern pattern = new Pattern(
                "T70 #no_inversion      C4majw                  | " +
                "#first_inversion   C4maj^w     C4maj^Ew    | " +
                "#second_inversion  C4maj^^w    C4maj^Gw      ");
            player.Play(pattern);
        }
    }
}