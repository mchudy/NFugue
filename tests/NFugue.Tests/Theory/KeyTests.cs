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
            Assert.Equal(key.Root.OriginalString, "A", StringComparer.OrdinalIgnoreCase);
        }
    }
}
