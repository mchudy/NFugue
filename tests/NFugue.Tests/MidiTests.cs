using NFugue.Extensions;
using NFugue.Staccato;
using Sanford.Multimedia.Midi;
using System.Threading;
using Xunit;

namespace NFugue.Tests
{
    public class MidiTests
    {
        [Fact]
        public void MidiTest()
        {
            using (OutputDevice outDevice = new OutputDevice(0))
            {
                var parser = new StaccatoParser();

                Track track = new Track();
                track.Add(new MetaMessage(MetaType.Copyright, new byte[] { }));

                track.Add(new MetaMessage(MetaType.Copyright, new byte[] { }));
                track.Add(new MetaMessage(MetaType.Copyright, new byte[] { }));
                track.Add(new MetaMessage(MetaType.Copyright, new byte[] { }));
                track.Add(new MetaMessage(MetaType.Copyright, new byte[] { }));

                ChannelMessageBuilder builder = new ChannelMessageBuilder();

                builder.Command = ChannelCommand.NoteOn;
                builder.MidiChannel = 0;
                builder.Data1 = 60;
                builder.Data2 = 127;
                builder.Build();

                outDevice.Send(builder.Result);

                Thread.Sleep(1000);

                builder.Command = ChannelCommand.NoteOff;
                builder.Data2 = 0;
                builder.Build();

                outDevice.Send(builder.Result);
            }
        }
    }
}
