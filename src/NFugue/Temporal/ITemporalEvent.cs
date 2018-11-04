using NFugue.Parsing;

namespace NFugue.Temporal
{
    public interface ITemporalEvent
    {
        void Execute(Parser parser);
    }
}
