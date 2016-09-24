using NFugue.ManualTests.Utils;
using NFugue.Patterns;
using NFugue.Playing;
using NFugue.Rhythms;

namespace NFugue.ManualTests.Tests
{
    public class RhythmTests
    {
        private readonly Rhythm rhythm = new Rhythm();
        private readonly Player player = new Player();

        [ManualTest("Rhythm timing", "The beats in this rhythm should not sound choppy or out-of-synch.")]
        public void RhythmTimingTest()
        {
            rhythm.AddLayer("O..oO...O..oOO..");
            rhythm.AddLayer("..S...S...S...S.");
            rhythm.AddLayer("````````````````");
            rhythm.AddLayer("...............+");

            Pattern pattern = rhythm.GetPattern().Repeat(4);
            player.Play(pattern);
        }

        [ManualTest("Two tone rhythm", "The sounds in this beat should not sound choppy or out-of-synch.")]
        public void TwoToneRhythmTest()
        {
            rhythm.AddLayer("oxoxoxoxoxoxoxoxoxoxoxoxoxoxoxoxoxoxo");
            player.Play(rhythm);
        }
    }
}