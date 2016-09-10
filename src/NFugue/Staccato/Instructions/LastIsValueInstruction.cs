using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFugue.Staccato.Instructions
{
    public class LastIsValueInstruction : IInstruction
    {
        public const char ReplaceChar = '$';

        private readonly string instruction;

        public LastIsValueInstruction(string instruction)
        {
            this.instruction = instruction;
        }

        public string OnInstructionReceived(IEnumerable<string> instructions)
        {
            var sb = new StringBuilder();
            int posDollar = instruction.IndexOf(ReplaceChar);
            sb.Append(instruction.Substring(0, posDollar));
            sb.Append(instructions.Last());
            sb.Append(instruction.Substring(posDollar + 1, instruction.Length - posDollar - 1));
            return sb.ToString();
        }
    }
}