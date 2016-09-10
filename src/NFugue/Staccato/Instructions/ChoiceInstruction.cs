using System.Collections.Generic;
using System.Linq;
using NFugue.Patterns;

namespace NFugue.Staccato.Instructions
{
    public class ChoiceInstruction : IInstruction
    {
        public List<string> Choices { get; } = new List<string>();

        public ChoiceInstruction(params string[] choices)
        {
            Choices.AddRange(choices);
        }

        public ChoiceInstruction(params int[] choices)
        {
            Choices.AddRange(choices.Select(c => c.ToString()));
        }

        public ChoiceInstruction(params IPatternProducer[] choices)
        {
            Choices.AddRange(choices.Select(c => c.GetPattern().ToString()));
        }

        public string OnInstructionReceived(IEnumerable<string> instructions)
        {
            int choice = int.Parse(instructions.Last());
            return Choices[choice];
        }
    }
}