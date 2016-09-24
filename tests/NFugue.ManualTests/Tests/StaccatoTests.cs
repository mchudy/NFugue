using NFugue.ManualTests.Utils;
using NFugue.Patterns;
using NFugue.Playing;
using NFugue.Staccato.Functions;
using NFugue.Staccato.Preprocessors;
using NFugue.Theory;
using System;

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

            player.Play(":ARPEGGIATED(Cmajw) Cmaj Emaj Gmaj Amin");
        }

        [ManualTest("Attack and decay", "You should hear one note in five different ways")]
        public void AttackDecayTest()
        {
            player.Play("V1 I[Flute] Cq Rq Dwwa10d10 Rq Ewwa100d10 Rq Fwwa10d100 Rq Gwwa100d100");
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
                "#no_inversion  C4majw                  | " +
                "#first_inversion   C4maj^w     C4maj^Ew    | " +
                "#second_inversion  C4maj^^w    C4maj^Gw      ");
            player.Play(pattern);
        }

        [ManualTest("Layers",
         @"You should hear the same sequence of three chords, played twice.
         The first group of three chords should sound identical to the second group of three chords.
        ")]
        public void LayersTest()
        {
            player.Play("V1 L0 C L1 E L2 G L0 D L1 F L2 A L0 E L1 G L2 B " +
                "#(That previous half should sound the same as this upcoming half) " +
                "V1 L0 C D E L1 E F G L2 G A B");
        }

        [ManualTest("Pitch wheel", "The following notes should all sound different")]
        public void PitchWheelTest()
        {
            Pattern pattern = new Pattern("V0 60w");
            for (int i = 0; i < 16384; i += 150)
            {
                pattern.Add(":PW(" + i + ") 60t");
            }
            player.Play(pattern);
        }

        [ManualTest("Voices",
            @"You should hear the same sequence of three chords, played twice.
            The first group of three chords should sound identical to the second group of three chords.
        ")]
        public void VoicesTest()
        {
            player.Play("V1 C V2 E V3 G V1 D V2 F V3 A V1 E V2 G V3 B " +
                "#(That previous half should sound the same as this upcoming half) " +
                "V1 C D E V2 E F G V3 G A B");
        }

        [ManualTest("Microtonal")]
        public void MicrotonalTest()
        {
            Pattern normalScale = new Pattern("(A4 A#4 B4 C5 C#5 D5 D#5 E5 F5 F#5 G5 G#5 A5 A#5 B5 C6)s");
            Pattern microtoneScale = new Pattern();
            var preprocessor = new MicrotonePreprocessor();

            for (double freq = Note.FrequencyForNote("A4"); freq < Note.FrequencyForNote("C6"); freq += 10.5)
            {
                microtoneScale.Add("m" + freq + "s");
            }

            Console.WriteLine("First, playing normal scale: " + normalScale);
            player.Play(normalScale);

            player.Play("Rw5");

            Console.WriteLine($"Second, playing microtone scale. You should hear middle ranges in these notes: {microtoneScale}");
            player.Play(microtoneScale);

            Console.WriteLine("The expanded version of the microtone string is: " + preprocessor.Preprocess(microtoneScale.ToString(), null));

            player.Play("Rw5");

            Pattern pitchWheelDemo = new Pattern();
            Console.WriteLine("Now, let's do a pitch wheel demo. Same note (Middle-C, note 60), played with 163 different pitch wheel settings, each 100 cents apart.");
            for (int i = 0; i < 16383; i += 100)
            {
                pitchWheelDemo.Add(":PW(" + i + ") 60t");
            }
            Console.WriteLine("This should sound like it gradually ramps up.");
            player.Play(pitchWheelDemo);

            player.Play("Rw5");

            Console.WriteLine("Now playing four notes. The first two should sound different from each other, and the second two should sound the same as the first two.");
            string microtone = "m346.0w m356.5w";
            player.Play(microtone);
            player.Play(preprocessor.Preprocess(microtone, null));
        }
    }
}