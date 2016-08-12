using FluentAssertions;
using NFugue.Theory;
using Xunit;

namespace NFugue.Tests.Theory
{
    public class KeyTests
    {
        [Fact]
        public void Create_major_key_with_string()
        {
            var key = new Key("Amaj");
            CheckIfAMajorKey(key);
        }

        [Fact]
        public void Create_major_key_with_string_without_major_indicator()
        {
            var key = new Key("A");
            CheckIfAMajorKey(key);
        }

        [Fact]
        public void Create_minor_key_with_string()
        {
            var key = new Key("Amin");
            CheckIfAMinorKey(key);
        }

        [Fact]
        public void Create_key_with_chord()
        {
            var key = new Key(new Chord("Amaj"));
            CheckIfAMajorKey(key);
        }

        [Fact]
        public void Create_key_with_key_signature_with_sharps()
        {
            var key = new Key("K####");

            key.KeySignature.Should().BeEquivalentTo("Emaj");
            key.Root.OriginalString.Should().BeEquivalentTo("E");
            key.Scale.Should().Be(Scale.Major);
        }

        [Fact]
        public void Create_key_with_key_signature_with_flats()
        {
            var key = new Key("Kbbb");

            key.KeySignature.Should().BeEquivalentTo("Ebmaj");
            key.Root.OriginalString.Should().BeEquivalentTo("Eb");
            key.Scale.Should().Be(Scale.Major);
        }

        private static void CheckIfAMajorKey(Key key)
        {
            key.KeySignature.Should().BeEquivalentTo("Amaj");
            key.Root.OriginalString.Should().BeEquivalentTo("A");
            key.Scale.Should().Be(Scale.Major);
        }

        private static void CheckIfAMinorKey(Key key)
        {
            key.KeySignature.Should().BeEquivalentTo("Amin");
            key.Root.OriginalString.Should().BeEquivalentTo("A");
            key.Scale.Should().Be(Scale.Minor);
        }
    }
}
