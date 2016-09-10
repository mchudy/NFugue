using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFugue.Patterns;

namespace NFugue.Staccato.Instructions
{
    public class SwitchInstruction : IInstruction
    {
        public const char ReplaceChar = '$';

        private readonly string instruction;
        private readonly string offValue;
        private readonly string onValue;

        public SwitchInstruction(string instruction, string offValue, string onValue)
        {
            this.instruction = instruction;
            this.offValue = offValue;
            this.onValue = onValue;
        }

        public SwitchInstruction(string instruction, int offValue, int onValue)
            : this(instruction, offValue.ToString(), onValue.ToString())
        {
        }

        public SwitchInstruction(string instruction, Pattern offValue, Pattern onValue)
            : this(instruction, offValue.ToString(), onValue.ToString())
        {
        }

        public string OnInstructionReceived(IEnumerable<string> instructions)
        {
            var sb = new StringBuilder();
            int posDollar = instruction.IndexOf(ReplaceChar);
            sb.Append(instruction.Substring(0, posDollar));
            string lastInstruction = instructions.Last();
            if (lastInstruction.Equals("ON", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append(onValue);
            }
            else if (lastInstruction.Equals("OFF", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append(offValue);
            }
            else
            {
                sb.Append(ReplaceChar);
            }
            sb.Append(instruction.Substring(posDollar + 1, instruction.Length - posDollar - 1));
            return sb.ToString();
        }
    }
}