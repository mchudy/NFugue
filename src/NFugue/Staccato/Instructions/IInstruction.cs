using System.Collections.Generic;

namespace NFugue.Staccato.Instructions
{
    public interface IInstruction
    {
        string OnInstructionReceived(IEnumerable<string> instructions);
    }
}