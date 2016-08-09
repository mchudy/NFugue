using NFugue.Theory;
using System;
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
            Assert.Equal("Emaj", key.KeySignature, StringComparer.OrdinalIgnoreCase);
            Assert.Equal("E", key.Root.OriginalString, StringComparer.OrdinalIgnoreCase);
            Assert.Equal(Scale.Major, key.Scale);
        }

        [Fact]
        public void Create_key_with_key_signature_with_flats()
        {
            var key = new Key("Kbbb");
            Assert.Equal("Ebmaj", key.KeySignature, StringComparer.OrdinalIgnoreCase);
            Assert.Equal("Eb", key.Root.OriginalString, StringComparer.OrdinalIgnoreCase);
            Assert.Equal(Scale.Major, key.Scale);
        }

        private static void CheckIfAMajorKey(Key key)
        {
            Assert.Equal("Amaj", key.KeySignature, StringComparer.OrdinalIgnoreCase);
            Assert.Equal("A", key.Root.OriginalString, StringComparer.OrdinalIgnoreCase);
            Assert.Equal(Scale.Major, key.Scale);
        }

        private static void CheckIfAMinorKey(Key key)
        {
            Assert.Equal("Amin", key.KeySignature, StringComparer.OrdinalIgnoreCase);
            Assert.Equal("A", key.Root.OriginalString, StringComparer.OrdinalIgnoreCase);
            Assert.Equal(Scale.Minor, key.Scale);
        }
    }
}
