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
            player.Play("T400 C D E F G A B C6 Cmajh");
        }
    }
}
