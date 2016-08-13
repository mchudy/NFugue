using NFugue.Patterns;
using Staccato.Extensions;
using System.Text.RegularExpressions;

namespace Staccato.Subparsers
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
            sbyte value = -1;
            if (posNextSpace > 1)
            {
                string instrumentId = music.Substring(1, posNextSpace - 1);
                if (Regex.IsMatch(instrumentId, @"\d+"))
                {
                    value = sbyte.Parse(instrumentId);
                }
                else
                {
                    if (instrumentId[0] == '[')
                    {
                        instrumentId = instrumentId.Substring(1, instrumentId.Length - 2);
                    }
                    value = (sbyte)context.Dictionary[instrumentId];
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

        public void PopulateContext(StaccatoParserContext context)
        {
            //TODO
        }
    }
}