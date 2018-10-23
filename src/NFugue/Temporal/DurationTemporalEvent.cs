namespace NFugue.Temporal
{
    public interface IDurationTemporalEvent : ITemporalEvent
    {
        double Duration { get; }
    }
}
