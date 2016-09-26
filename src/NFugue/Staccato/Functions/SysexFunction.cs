using System.Collections.Generic;

namespace NFugue.Staccato.Functions
{
    public class SysexFunction : ISubparserFunction
    {
        public IEnumerable<string> GetNames()
        {
            return new[] { "SYSEX", "SE", "SYSTEMEXCLUSIVE" };
        }

        public void Apply(string parameters, StaccatoParserContext context)
        {
            var splittedParams = parameters.Split(',');
            var bytes = new byte[splittedParams.Length];
            for (int i = 0; i < splittedParams.Length; i++)
            {
                bytes[i] = byte.Parse(splittedParams[i].Trim());
            }
            context.Parser.OnSystemExclusiveParsed(bytes);
        }
    }
}