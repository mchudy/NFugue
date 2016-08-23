using NFugue.Playing;
using Xunit;

namespace NFugue.Tests
{
    public class MidiTests
    {
        [Fact]
        public void MidiTest()
        {
            var player = new Player();
            player.Play("C D E F G A B C6");
        }
    }
}
