using System.Collections.Generic;

namespace NFugue.Patterns
{
    public interface ITokenProducer
    {
        IEnumerable<Token> GetTokens();
    }
}