namespace NFugue.Patterns
{
    public class Token : IPatternProducer
    {
        public Token(string tokenString, TokenType type)
        {
            TokenString = tokenString;
            Type = type;
        }

        public string TokenString { get; }
        public TokenType Type { get; }

        public Patterns.Pattern GetPattern()
        {
            return new Patterns.Pattern(TokenString);
        }

        public override string ToString() => TokenString;

        public enum TokenType
        {
            Voice,
            Layer,
            Instrument,
            Tempo,
            KeySignature,
            TimeSignature,
            BarLine,
            TrackTimeBookmark,
            TrackTimeBookmarkRequested,
            Lyric,
            Marker,
            Function,
            Note,
            Whitespace,
            UnknownToken
        }
    }
}