namespace NFugue.Integration.MusicXml.Internals
{
    internal class PartContext
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public MidiInstrument[] Instruments { get; set; } = new MidiInstrument[16];
        public int CurrentVolume { get; set; } = DefaultNoteSettings.DefaultOnVelocity;
        public int Voice { get; set; }

        public PartContext(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}