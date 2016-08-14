using System.Collections.Generic;

namespace Staccato.Functions
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
            var bytes = new sbyte[splittedParams.Length];
            for (int i = 0; i < splittedParams.Length; i++)
            {
                bytes[i] = sbyte.Parse(splittedParams[i].Trim());
            }
            context.Parser.OnSystemExclusiveParsed(bytes);
        }
    }
}