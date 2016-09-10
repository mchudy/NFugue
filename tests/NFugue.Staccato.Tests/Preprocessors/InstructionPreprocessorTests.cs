using FluentAssertions;
using NFugue.Patterns;
using NFugue.Staccato;
using NFugue.Staccato.Instructions;
using NFugue.Staccato.Preprocessors;
using System.Collections.Generic;
using Xunit;

namespace Staccato.Tests.Preprocessors
{
    public class InstructionPreprocessorTests
    {
        protected readonly StaccatoParser parser = new StaccatoParser();
        protected StaccatoParserContext context;
        protected InstructionPreprocessor preprocessor = new InstructionPreprocessor();

        public InstructionPreprocessorTests()
        {
            context = new StaccatoParserContext(null);
        }

        [Fact]
        public void Test_simple_instruction()
        {
            preprocessor.AddInstruction("letter", "c");
            preprocessor.Preprocess("{letter}", context).Should().Be("c");
        }

        [Fact]
        public void Test_switch_instruction()
        {
            preprocessor.AddInstruction("turn", new SwitchInstruction("$", "c", "d"));

            preprocessor.Preprocess("{turn off}", context).Should().Be("c");
            preprocessor.Preprocess("{turn on}", context).Should().Be("d");
        }

        [Fact]
        public void Test_choice_instruction()
        {
            preprocessor.AddInstruction("chorus", new ChoiceInstruction("c", "d", "e"));

            preprocessor.Preprocess("{chorus 0}", context).Should().Be("c");
            preprocessor.Preprocess("{chorus 1}", context).Should().Be("d");
            preprocessor.Preprocess("{chorus 2}", context).Should().Be("e");
        }

        [Fact]
        public void Test_last_is_value_instruction()
        {
            preprocessor.AddInstruction("volume", new LastIsValueInstruction(":CON(7,$)"));

            preprocessor.Preprocess("{volume should now be set to the glorious value of 2}", context)
                .Should().Be(":CON(7,2)");
        }

        [Fact]
        public void Test_last_value_to_split_instruction()
        {
            var instruction = new LastIsValueToSplitInstruction(":CON(7,$1) :CON(7,$2)",
                x => new Dictionary<string, string>()
                {
                    {"$1", (int.Parse(x) /120).ToString() },
                    {"$2", (int.Parse(x) % 128).ToString() },
                });
            preprocessor.AddInstruction("set total volume", instruction);

            preprocessor.Preprocess("{set total volume to 1600}", context)
                .Should().Be($":CON(7,{1600 / 120}) :CON(7,{1600 % 128})");
        }

        [Fact]
        public void Test_pattern_as_instruction()
        {
            var pattern = new Pattern("C");
            preprocessor.AddInstruction("pattern", pattern);

            preprocessor.Preprocess("{pattern}", context).Should().BeEquivalentTo("c");
        }

        [Fact]
        public void Should_use_longest_matching_instruction()
        {
            preprocessor.AddInstruction("life", new LastIsValueInstruction(":CON(6,$)"));
            preprocessor.AddInstruction("life is a lemon and i want my money back", new LastIsValueInstruction(":CON(7,$)"));
            preprocessor.AddInstruction("life is a lemon", new LastIsValueInstruction(":CON(8,$)"));

            preprocessor.Preprocess("{life is a lemon and i want my money back because meat loaf said so 42}", context)
                .Should().Be(":CON(7,42)");
        }
    }
}