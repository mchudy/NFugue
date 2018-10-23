namespace NFugue.Temporal
{
    interface IDurationTemporalEvent : ITemporalEvent
    {
        double GetDuration();
    }
}
