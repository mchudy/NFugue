using NFugue;
using System;

namespace Staccato.Functions
{
    public class DefaultPreprocessorFunction : IPreprocessorFunction
    {
        private const string Octave = "OCTAVE";
        private const string BaseOctave = "BASS_OCTAVE";
        private const string Duration = "DURATION";
        private const string Attack = "ATTACK";
        private const string Decay = "DECAY";

        public string[] GetNames()
        {
            return new[] { "DEFAULT", "DEFAULTS" };
        }

        public string Apply(string parameters, StaccatoParserContext context)
        {
            string[] defaultSettings = parameters.Split(',');
            foreach (string defaultSetting in defaultSettings)
            {
                string[] defaultValues = defaultSetting.Split('=');
                if (defaultValues.Length != 2)
                {
                    throw new ApplicationException("DefaultProcessor found this setting, which is not in the form KEY=VALUE: " + defaultSetting);
                }
                string key = defaultValues[0];
                string value = defaultValues[1];

                if (key.Equals(Octave, StringComparison.OrdinalIgnoreCase))
                {
                    DefaultNoteSettings.DefaultOctave = sbyte.Parse(value);
                }
                else if (key.Equals(BaseOctave, StringComparison.OrdinalIgnoreCase))
                {
                    DefaultNoteSettings.DefaultBassOctave = sbyte.Parse(value);
                }
                else if (key.Equals(Duration, StringComparison.OrdinalIgnoreCase))
                {
                    double dur = 0.0d;
                    if (double.TryParse(value, out dur))
                    {
                        DefaultNoteSettings.DefaultDuration = dur;
                    }
                    else
                    {
                        throw new ApplicationException("Currently, default duration must be specified as a decimal. For example, please use 0.5 for 'h', 0.25 for 'q', and so on. You had entered: " + value);
                    }
                }
                else if (key.Equals(Attack, StringComparison.OrdinalIgnoreCase))
                {
                    DefaultNoteSettings.DefaultOnVelocity = sbyte.Parse(value);
                }
                else if (key.Equals(Decay, StringComparison.OrdinalIgnoreCase))
                {
                    DefaultNoteSettings.DefaultOffVelocity = sbyte.Parse(value);
                }
                else
                {
                    throw new ApplicationException("DefaultProcessor found this setting where the key is not recognized: " + defaultSetting + " (key should be one of the following: " + Octave + ", " + BaseOctave + ", " + Duration + ", " + Attack + ", or " + Decay);
                }
            }

            return "";
        }
    }
}