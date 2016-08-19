using Sanford.Multimedia.Midi;

namespace NFugue.Extensions
{
    public static class TrackExtensions
    {
        public static void Add(this Track track, IMidiMessage message)
        {
            track.Insert(track.Length - 1, message);
        }
    }
}