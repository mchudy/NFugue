using NFugue.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFugue.Staccato
{
    public interface IInstruction
    {
        string OnIstructionReceived(IEnumerable<string> instructions);
    }

    public class Choice : IInstruction
    {
        public List<string> Choices { get; } = new List<string>();

        public Choice(params string[] choices)
        {
            Choices.AddRange(choices);
        }

        public Choice(params int[] choices)
        {
            Choices.AddRange(choices.Select(c => c.ToString()));
        }

        public Choice(params IPatternProducer[] choices)
        {
            Choices.AddRange(choices.Select(c => c.GetPattern().ToString()));
        }

        public string OnIstructionReceived(IEnumerable<string> instructions)
        {
            int choice = int.Parse(instructions.Last());
            return Choices[choice];
        }
    }

    public class Switch : IInstruction
    {
        public const char ReplaceChar = '$';

        private readonly string instruction;
        private readonly string offValue;
        private readonly string onValue;

        public Switch(string instruction, string offValue, string onValue)
        {
            this.instruction = instruction;
            this.offValue = offValue;
            this.onValue = onValue;
        }

        public Switch(string instruction, int offValue, int onValue)
            : this(instruction, offValue.ToString(), onValue.ToString())
        {
        }

        public Switch(string instruction, Pattern offValue, Pattern onValue)
            : this(instruction, offValue.ToString(), onValue.ToString())
        {
        }

        public string OnIstructionReceived(IEnumerable<string> instructions)
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

    public class LastIsValue : IInstruction
    {
        public const char ReplaceChar = '$';

        private readonly string instruction;

        public LastIsValue(string instruction)
        {
            this.instruction = instruction;
        }

        public string OnIstructionReceived(IEnumerable<string> instructions)
        {
            var sb = new StringBuilder();
            int posDollar = instruction.IndexOf(ReplaceChar);
            sb.Append(instruction.Substring(0, posDollar));
            sb.Append(instructions.Last());
            sb.Append(instruction.Substring(posDollar + 1, instruction.Length - posDollar - 1);
            return sb.ToString();
        }
    }

    public class LastIsValueToSplit : IInstruction
    {
        private string instruction;
        private readonly Func<string, IDictionary<string, string>> splitter;

        public LastIsValueToSplit(string instruction, Func<string, IDictionary<string, string>> splitter)
        {
            this.instruction = instruction;
            this.splitter = splitter;
        }

        public string OnIstructionReceived(IEnumerable<string> instructions)
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