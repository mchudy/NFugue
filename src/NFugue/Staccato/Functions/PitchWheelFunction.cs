using System.Collections.Generic;
using NFugue.Midi;

namespace NFugue.Staccato.Functions
{
    public class PitchWheelFunction : ISubparserFunction
    {
        public IEnumerable<string> GetNames()
        {
            return new[] { "PW", "PITCHWHEEL", "PB", "PITCHBEND" };
        }

        public void Apply(string parameters, StaccatoParserContext context)
        {
            var splittedParams = parameters.Split(',');
            if (splittedParams.Length == 2)
            {
                context.Parser.OnPitchWheelParsed(sbyte.Parse(splittedParams[0].Trim()),
                    sbyte.Parse(splittedParams[1].Trim()));
            }
            else if (splittedParams.Length == 1)
            {
                int pitch = int.Parse(splittedParams[0]);
                context.Parser.OnPitchWheelParsed(MidiTools.GetLSB(pitch), MidiTools.GetMSB(pitch));
            }
        }
    }
}