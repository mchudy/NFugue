using System.Collections.Generic;

namespace NFugue.Staccato.Functions
{
    public class PolyPressureFunction : ISubparserFunction
    {
        public IEnumerable<string> GetNames()
        {
            return new[] { "PP", "POLYPRESSURE", "POLY", "POLYPHONIC", "POLYPHONICPRESSURE" };
        }

        public void Apply(string parameters, StaccatoParserContext context)
        {
            var splittedParams = parameters.Split(',');
            if (splittedParams.Length == 2)
            {
                context.Parser.OnPolyphonicPressureParsed(int.Parse(splittedParams[0].Trim()),
                    int.Parse(splittedParams[1].Trim()));
            }
        }
    }
}