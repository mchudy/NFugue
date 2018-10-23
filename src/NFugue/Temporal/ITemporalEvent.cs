using NFugue.Parsing;

namespace NFugue.Temporal
{
    interface ITemporalEvent
    {
        void Execute(Parser parser);
    }
}
