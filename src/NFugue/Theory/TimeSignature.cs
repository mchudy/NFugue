namespace NFugue.Theory
{
    public class TimeSignature
    {
        public static readonly TimeSignature CommonTime = new TimeSignature(4, 4);

        public TimeSignature(int beatsForMeasure, int durationForBeat)
        {
            BeatsForMeasure = beatsForMeasure;
            DurationForBeat = durationForBeat;
        }

        public int BeatsForMeasure { get; set; }
        public int DurationForBeat { get; set; }
    }
}