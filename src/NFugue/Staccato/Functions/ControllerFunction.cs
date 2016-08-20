using NFugue.Midi;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Staccato.Functions
{
    public class ControllerFunction : ISubparserFunction
    {
        public IEnumerable<string> GetNames()
        {
            return new[] { "CE", "CON", "CONTROLLER", "CONTROLLEREVENT" };
        }

        public void Apply(string parameters, StaccatoParserContext context)
        {
            var splittedParams = parameters.Split(',');
            if (splittedParams.Length == 2)
            {
                int controllerNumber = 0;
                string controllerId = splittedParams[0].Trim();
                if (Regex.IsMatch(controllerId, @"\d+"))
                {
                    controllerNumber = int.Parse(controllerId);
                }
                else
                {
                    if (controllerId[0] == '[')
                    {
                        controllerId = controllerId.Substring(1, controllerId.Length - 2);
                    }
                    controllerNumber = (int)context.Dictionary[controllerId];
                }
                if (controllerNumber > sbyte.MaxValue)
                {
                    context.Parser.OnControllerEventParsed(MidiTools.GetLSB(controllerNumber),
                        MidiTools.GetLSB(int.Parse(splittedParams[1].Trim())));
                    context.Parser.OnControllerEventParsed(MidiTools.GetMSB(controllerNumber),
                        MidiTools.GetMSB(int.Parse(splittedParams[1].Trim())));
                }
                else
                {
                    context.Parser.OnControllerEventParsed((sbyte)controllerNumber,
                        sbyte.Parse(splittedParams[1].Trim()));
                }
            }
        }
    }
}
