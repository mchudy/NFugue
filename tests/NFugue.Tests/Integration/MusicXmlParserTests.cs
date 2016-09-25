using FluentAssertions;
using NFugue.Integration.MusicXml;
using NFugue.Staccato;
using System.IO;
using Xunit;

namespace NFugue.Tests.Integration
{
    public class MusicXmlParserTests
    {
        private readonly MusicXmlParser xmlParser = new MusicXmlParser();

        [Fact]
        public void Hello_world_part_wise()
        {
            ParseMusicXmlFile("Data/HelloWorldPartWise.xml").Should().Be("V0 C4w |");
        }

        [Fact]
        public void Hello_world_time_wise()
        {
            ParseMusicXmlFile("Data/HelloWorldTimeWise.xml").Should().Be("V0 C4w |");
        }

        [Fact]
        public void Schumann_frage_beginning()
        {
            ParseMusicXmlFile("Data/Frage.xml").Should().Be(
                "V0 KEY:Ebmaj G4q 'Warst | F4q. 'du Eb4i 'nicht, Eb4q 'heil Bb4i 'ger G4i |");
        }

        private string ParseMusicXmlFile(string path)
        {
            string xml = File.ReadAllText(path);
            var patternBuilder = new StaccatoPatternBuilder(xmlParser);

            xmlParser.Parse(xml);

            return patternBuilder.Pattern.ToString();
        }
    }
}