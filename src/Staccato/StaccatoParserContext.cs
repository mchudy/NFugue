using NFugue.Parser;
using NFugue.Theory;
using System.Collections.Generic;
using System.IO;

namespace Staccato
{
    public class StaccatoParserContext
    {
        public StaccatoParserContext(Parser parser)
        {
            Parser = parser;
        }

        public IDictionary<string, object> Dictionary = new Dictionary<string, object>();
        public Parser Parser { get; }
        public TimeSignature TimeSignature { get; set; } = TimeSignature.CommonTime;
        public Key Key { get; set; } = Key.Default;

        public StaccatoParserContext LoadDictionary(string filePath)
        {
            using (StreamReader sr = File.OpenText(filePath))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null && line.Length > 1)
                {
                    if (IsComment(line)) continue;
                    if (IsDefinition(line))
                    {

                        int equalsIndex = line.IndexOf('=');
                        string key = line.Substring(1, equalsIndex).Trim();
                        string value = line.Substring(equalsIndex + 1, line.Length).Trim();
                        this.Dictionary[key] = value;
                    }
                }
            }
            return this;
        }

        private static bool IsComment(string line) => line[0] == '#';
        private static bool IsDefinition(string line) => line[0] == '$';
    }
}