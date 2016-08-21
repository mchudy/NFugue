using NFugue.Extensions;
using NFugue.Playing;
using Sanford.Multimedia.Midi;
using System;
using System.Threading;
using Xunit;

namespace NFugue.Tests
{
    public class MidiTests
    {
        [Fact]
        public void MidiTest()
        {
            OutputDevice outDevice = new OutputDevice(0);
            var player = new Player();
            player.Play("C");

            ChannelMessageBuilder builder = new ChannelMessageBuilder();

            builder.Command = ChannelCommand.NoteOn;
            builder.MidiChannel = 0;
            builder.Data1 = 60;
            builder.Data2 = 127;
            builder.Build();

            var noteOn = builder.Result;

            builder.Command = ChannelCommand.NoteOff;
            builder.Data2 = 0;
            builder.Build();

            var noteOff = builder.Result;

            Sequencer sequencer = new Sequencer();
            var sequence = new Sequence(120);

            var track = new Track();
            for (int i = 0; i < 200; i++)
            {
                track.Add(noteOn);
                track.Add(noteOff);
            }

            sequence.Add(track);

            sequencer.Sequence = sequence;
            sequencer.Chased += (s, e) =>
            {
                foreach (ChannelMessage msg in e.Messages)
                {
                    outDevice.Send(msg);
                }
            };
            sequencer.ChannelMessagePlayed += (s, e) =>
            {
                Console.WriteLine(DateTime.Now);
                outDevice.Send(e.Message);
            };
            sequencer.Stopped += (s, e) =>
            {
                outDevice.Close();
            };
            sequencer.Start();
            Thread.Sleep(1000);
        }
    }
}
