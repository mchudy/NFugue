namespace NFugue.Integration.MusicXml.Internals
{
    internal class MidiInstrument
    {
        public string Id { get; }
        public string Channel { get; }
        public string Name { get; }
        public string Bank { get; }
        public int Program { get; }
        public string Unpitched { get; set; }

        public MidiInstrument(string id, string channel, string name, string bank, int program, string unpitched)
        {
            Id = id;
            Channel = channel;
            Name = name;
            Bank = bank;
            Program = program;
            Unpitched = unpitched;
        }
    }
}