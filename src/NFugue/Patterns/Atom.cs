using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFugue.Midi;
using NFugue.Staccato;
using NFugue.Staccato.Subparsers;
using NFugue.Theory;

namespace NFugue.Patterns
{
    /// <summary>
    /// An Atom represents a single entity of a Voice+Layer+Instrument+Note
    /// and is useful especially when using the Realtime Player, so all of
    /// the information about a specific note is conveyed at the same time.
    /// Pattern now has an atomize() method that will turn the Pattern into
    /// a collection of atoms.
    /// </summary>
    public class Atom : IPatternProducer
    {
        private string contents;

        public Atom(byte voice, byte layer, Instrument instrument, Note note)
        {
            CreateAtom(voice, layer, instrument, note);
        }

        public Atom(byte voice, byte layer, Instrument instrument, string note)
         : this(voice, layer, instrument, new Note(note))
        {
        }

        public Atom(string voice, string layer, string instrument, string note)
            : this(voice, layer, instrument, new Note(note))
        {
        }

        public Atom(string voice, string layer, string instrument, Note note)
        {
            var context = new StaccatoParserContext(null);
            IVLSubparser.PopulateContext(context);
            var subparser = new IVLSubparser();

            CreateAtom(subparser.GetValue(voice.ToUpper(), context),
                subparser.GetValue(layer.ToUpper(), context),
                (Instrument)subparser.GetValue(instrument.ToUpper(), context),
                new Note(note));
        }

        public byte Voice { get; private set; }
        public byte Layer { get; private set; }
        public Instrument Instrument { get; private set; }
        public Note Note { get; private set; }

        public Pattern GetPattern() => new Pattern(contents);
        public override string ToString() => contents;

        private void CreateAtom(byte voice, byte layer, Instrument instrument, Note note)
        {
            Voice = voice;
            Layer = layer;
            Instrument = instrument;
            Note = note;

            var sb = new StringBuilder();
            sb.Append(AtomSubparser.AtomChar);
            sb.Append(IVLSubparser.VoiceChar);
            sb.Append(voice);
            sb.Append(AtomSubparser.QuarkSeparator);
            sb.Append(IVLSubparser.LayerChar);
            sb.Append(layer);
            sb.Append(AtomSubparser.QuarkSeparator);
            sb.Append(IVLSubparser.InstrumentChar);
            sb.Append(((byte)instrument).ToString());
            sb.Append(AtomSubparser.QuarkSeparator);
            sb.Append(note);
            contents = sb.ToString();
        }
    }
}
