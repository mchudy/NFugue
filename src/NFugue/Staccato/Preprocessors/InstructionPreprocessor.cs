using NFugue.Patterns;
using NFugue.Staccato.Instructions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NFugue.Staccato.Preprocessors
{
    public class InstructionPreprocessor : IPreprocessor
    {
        private static readonly Regex keyPattern = new Regex(@"\{\p{IsBasicLatin}*?\}", RegexOptions.Compiled);
        private readonly IDictionary<string, IInstruction> instructions = new Dictionary<string, IInstruction>();

        public string Preprocess(string musicString, StaccatoParserContext context)
        {
            StringBuilder sb = new StringBuilder();
            int posPrev = 0;

            // Sort all of the instruction keys by length, so we'll deal with the longer ones first
            string[] sizeSortedInstructions = instructions.Keys.OrderByDescending(k => k.Length).ToArray();
            bool matchFound = false;

            foreach (Match match in keyPattern.Matches(musicString))
            {
                string key = match.Groups[0].Value;
                key = key.Substring(1, key.Length - 2);
                foreach (string possibleMatch in sizeSortedInstructions)
                {
                    if (!matchFound)
                    {
                        if (key.StartsWith(possibleMatch))
                        {
                            IInstruction instruction;
                            string value = key;
                            if (instructions.TryGetValue(possibleMatch, out instruction))
                            {
                                value = instruction.OnInstructionReceived(key.Split(' '));
                            }
                            sb.Append(musicString.Substring(posPrev, match.Index - posPrev));
                            sb.Append(value);
                            posPrev = match.Index + match.Length;
                            matchFound = true;
                        }
                    }
                }
                if (!matchFound)
                {
                    posPrev = match.Index + match.Length;
                }
            }
            sb.Append(musicString.Substring(posPrev, musicString.Length - posPrev));
            return sb.ToString();
        }

        public void AddInstruction(string key, IInstruction value)
        {
            instructions.Add(key, value);
        }

        public void AddInstruction(string key, IPatternProducer value)
        {
            AddInstruction(key, value.GetPattern().ToString());
        }

        public void AddInstruction(string key, string value)
        {
            instructions.Add(key, new Instruction { Value = value });
        }

        private class Instruction : IInstruction
        {
            public string Value { private get; set; }
            public string OnInstructionReceived(IEnumerable<string> instructions) => Value;
        }
    }
}
