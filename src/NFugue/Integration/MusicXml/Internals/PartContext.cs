namespace NFugue.Integration.MusicXml.Internals
{
    internal class PartContext
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public MidiInstrument[] Instruments { get; set; } = new MidiInstrument[16];
        public byte CurrentVolume { get; set; } = (byte)DefaultNoteSettings.DefaultOnVelocity;
        public byte Voice { get; set; }

        public PartContext(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}