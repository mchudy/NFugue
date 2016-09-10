using System;
using System.Collections.Generic;
using System.Linq;

namespace NFugue.Staccato.Instructions
{
    public class LastIsValueToSplitInstruction : IInstruction
    {
        private string instruction;
        private readonly Func<string, IDictionary<string, string>> splitter;

        public LastIsValueToSplitInstruction(string instruction, Func<string, IDictionary<string, string>> splitter)
        {
            this.instruction = instruction;
            this.splitter = splitter;
        }

        public string OnInstructionReceived(IEnumerable<string> instructions)
        {
            IDictionary<string, string> map = splitter(instructions.Last());
            foreach (string key in map.Keys)
            {
                instruction = instruction.Replace(key, map[key]);
            }
            return instruction;
        }
    }
}