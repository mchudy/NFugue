using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NFugue.Midi;
using NFugue.Patterns;
using NFugue.Theory;
using Xunit;

namespace NFugue.Tests.Patterns
{
    public class AtomTests
    {
        [Fact]
        public void Test_atom()
        {
            var atom = new Atom(1, 3, Instrument.ElectricPiano1, "C");

            atom.ToString().Should().Be("&V1,L3,I4,C");
        }

        [Fact]
        public void Test_atom_member_variables()
        {
            var atom = new Atom(1, 3, Instrument.ElectricPiano1, "C");

            atom.Voice.Should().Be(1);
            atom.Layer.Should().Be(3);
            atom.Instrument.Should().Be(Instrument.ElectricPiano1);
            atom.Note.Should().Be(new Note("C"));
        }

        [Fact]
        public void Test_atom_with_context()
        {
            var atom = new Atom("V1", "L2", "I[Piano]", new Note("C"));

            atom.ToString().Should().Be("&V1,L2,I0,C");
        }

        [Fact]
        public void Test_atomize_pattern()
        {
            var pattern = new Pattern("C")
                .SetVoice(1)
                .SetInstrument(Instrument.ElectricGrandPiano);

            pattern.Atomize();

            pattern.ToString().Should().Be("&V1,L0,I2,C");
        }

        [Fact]
        public void Test_atomize_crazier_pattern()
        {
            // This pattern has:
            // - Unstated layers and instruments that should resolve to 0
            // - Layers and instruments stated at the end of one voice that shouldn't affect other voices
            // - Layers and instruments that should be recalled when parsed later in the line
            var pattern = new Pattern("V1 A V2 B I55 L6 V1 I7 C V3 L7 E I8 D L5 C I4 A V2 G");

            pattern.Atomize();

            pattern.ToString().Should().Be("&V1,L0,I0,A &V2,L0,I0,B &V1,L0,I7,C &V3,L7,I0,E &V3,L7,I8,D &V3,L5,I8,C &V3,L5,I4,A &V2,L6,I55,G");
        }
    }
}
