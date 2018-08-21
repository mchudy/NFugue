using NFugue.Extensions;
using NFugue.Midi;
using NFugue.Patterns;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NFugue.Staccato.Subparsers
{
    /// <summary>
    /// Parses Instrument, Voice, and Layer tokens. Each has values that are parsed as bytes. 
    /// </summary>
    public class IVLSubparser : ISubparser
    {
        public const char InstrumentChar = 'I';
        public const char LayerChar = 'L';
        public const char VoiceChar = 'V';

        public bool Matches(string music)
        {
            return music[0] == InstrumentChar ||
                   music[0] == LayerChar ||
                   music[0] == VoiceChar;
        }

        public TokenType GetTokenType(string tokenString)
        {
            switch (tokenString[0])
            {
                case InstrumentChar:
                    return TokenType.Instrument;
                case VoiceChar:
                    return TokenType.Voice;
                case LayerChar:
                    return TokenType.Layer;
                default:
                    return TokenType.UnknownToken;
            }
        }

        public int Parse(string music, StaccatoParserContext context)
        {
            if (!Matches(music)) return 0;
            int posNextSpace = music.FindNextOrEnd(' ');
            int value = -1;
            if (posNextSpace > 1)
            {
                string instrumentId = music.Substring(1, posNextSpace - 1);
                if (Regex.IsMatch(instrumentId, @"\d+"))
                {
                    value = int.Parse(instrumentId);
                }
                else
                {
                    if (instrumentId[0] == '[')
                    {
                        instrumentId = instrumentId.Substring(1, instrumentId.Length - 2);
                    }

                    value = Convert.ToInt32(context.Dictionary[instrumentId]);
                }
            }
            switch (music[0])
            {
                case InstrumentChar:
                    context.Parser.OnInstrumentParsed(value);
                    break;
                case LayerChar:
                    context.Parser.OnLayerChanged(value);
                    break;
                case VoiceChar:
                    context.Parser.OnTrackChanged(value);
                    break;
            }
            return posNextSpace + 1;
        }


        /** Given a string like "V0" or "I[Piano]", this method will return the value of the token */
        public byte GetValue(string ivl, StaccatoParserContext context)
        {
            string instrumentId = ivl.Substring(1, ivl.Length - 1);
            if (Regex.IsMatch(instrumentId, "\\d+"))
            {
                return byte.Parse(instrumentId);
            }

            if (instrumentId[0] == '[')
            {
                instrumentId = instrumentId.Substring(1, instrumentId.Length - 2);
            }
            return (byte) context.Dictionary[instrumentId];
        }

        public static void PopulateContext(StaccatoParserContext context)
        {
            // Voices
            context.Dictionary["PERCUSSION"] = (byte)9;

            // Instruments
            context.Dictionary.AddRange(Enum.GetValues(typeof(Instrument)).OfType<Instrument>()
                .ToDictionary(item => item.GetDescription().ToUpper(), item =>
                {
                    var @byte = (byte) item;
                    return (object) @byte;
                }));
        }
    }
}