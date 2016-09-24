namespace NFugue.Integration.MusicXml.Internals
{
    internal class VoiceDefinition
    {
        public int Part { get; set; }
        public int Voice { get; set; }

        public VoiceDefinition(int part, int voice)
        {
            Part = part;
            Voice = voice;
        }
    }
}