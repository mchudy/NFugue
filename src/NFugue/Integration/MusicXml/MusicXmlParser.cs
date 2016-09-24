using NFugue.Extensions;
using NFugue.Integration.MusicXml.Internals;
using NFugue.Midi;
using NFugue.Parsing;
using NFugue.Theory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NFugue.Integration.MusicXml
{
    public class MusicXmlParser : Parser
    {
        private XDocument document;

        private readonly int curVelocity = DefaultNoteSettings.DefaultOnVelocity;
        private int beatsPerMeasure = 1;
        private int divisionsPerBeat = 1;
        private int currentVoice = -1;
        private int currentLayer;

        private KeySignature keySignature = new KeySignature(0, 0);

        // next available voice # for a new voice
        private byte nextVoice;
        private readonly VoiceDefinition[] voices = new VoiceDefinition[32];
        private PartContext currentPart;

        public void Parse(string musicXmlString)
        {
            Parse(XDocument.Parse(musicXmlString));
        }

        private void Parse(XDocument doc)
        {
            document = doc;
            Parse();
        }

        private void Parse()
        {
            OnBeforeParsingStarted();
            var root = document.Root;
            if (root == null)
            {
                throw new ArgumentException("Invalid MusicXML document");
            }
            if (root.Name.ToString().Equals("score-timewise", StringComparison.OrdinalIgnoreCase))
            {
                ParseTimeWise(root);
            }
            else if (root.Name.ToString().Equals("score-partwise", StringComparison.OrdinalIgnoreCase))
            {
                ParsePartWise(root);
            }
            else
            {
                throw new ArgumentException("Invalid MusicXML document");
            }
        }

        private void ParsePartWise(XElement root)
        {
            var partlist = root.Element("part-list");
            var parts = partlist.Elements().ToList();
            Dictionary<string, PartContext> partHeaders = ParsePartList(parts);
            parts = root.Elements("part").ToList();
            for (int childId = 0; childId < parts.Count; childId++)
            {
                var partElement = parts[childId];
                string partId = partElement?.Attribute("id")?.Value;
                SwitchPart(partHeaders, partId, childId);
                var measures = partElement?.Elements("measure").ToList();
                for (int measure = 0; measure < measures?.Count; measure++)
                {
                    ParseMusicData(childId, partId, partHeaders, measures[measure]);
                    OnBarLineParsed(0);
                }
            }
        }

        private void ParseMusicData(int partIndex, string partId, Dictionary<string, PartContext> partHeaders, XElement musicDataRoot)
        {
            var attributes = musicDataRoot.Element("attributes");
            if (attributes != null)
            {
                KeySignature ks = ParseKeySignature(attributes);
                if (!keySignature.Equals(ks))
                {
                    keySignature = ks;
                    OnKeySignatureParsed(keySignature.Key, keySignature.Scale);
                }

                int newDivisionsPerBeat;
                if (int.TryParse(attributes.Element("divisions")?.Value, out newDivisionsPerBeat))
                {
                    divisionsPerBeat = newDivisionsPerBeat;
                }
                int newBeatsPerMeasure;
                if (int.TryParse(GetRecursiveFirstChildElement(attributes, "time", "beats")?.Value, out newBeatsPerMeasure))
                {
                    beatsPerMeasure = newBeatsPerMeasure;
                }
            }

            var children = musicDataRoot.Elements().ToList();
            for (int i = 0; i < children.Count; i++)
            {
                var el = children[i];
                if (el.Name.LocalName == "harmony")
                {
                    ParseGuitarChord(el);
                }
                else if (el.Name.LocalName == "note")
                {
                    ParseNote(partIndex, el, partId, partHeaders);
                }
                else if (el.Name.LocalName == "direction")
                {
                    var sound = el.Element("sound");
                    if (sound != null)
                    {
                        string value = sound.Attribute("dynamics")?.Value;
                        if (value != null)
                        {
                            currentPart.CurrentVolume = byte.Parse(value);
                        }
                        value = sound.Attribute("tempo")?.Value;
                        if (value != null)
                        {
                            OnTempoChanged(int.Parse(value));
                        }
                    }
                }
            }
        }

        private XElement GetRecursiveFirstChildElement(XElement element, params string[] children)
        {
            XElement el = element;
            foreach (string c in children)
            {
                if (el == null)
                {
                    return null;
                }
                el = el.Element(c);
            }
            return el;
        }

        private void ParseNote(int p, XElement noteElement, string partId, Dictionary<string, PartContext> partHeaders)
        {
            Note newNote = new Note();
            newNote.IsFirstNote = true;

            bool isRest = false;
            bool isStartOfTie = false;
            bool isEndOfTie = false;
            byte noteNumber = 0;
            byte octaveNumber = 0;
            double decimalDuration;

            // skip grace notes
            // TODO : why do we skip grace notes ?
            if (noteElement.Element("grace") != null)
            {
                return;
            }
            var voice = noteElement.Element("voice");
            // TODO : !newNote.isHarmonicNote() is always true ...
            if (voice != null && !newNote.IsHarmonicNote)
            {
                if ((int.Parse(voice.Value) - 1) != currentLayer)
                {
                    currentLayer = byte.Parse(voice.Value);
                    currentLayer = (byte)(currentLayer - 1);
                    OnLayerChanged((sbyte)currentLayer);
                }
            }

            EnhanceFromChord(noteElement, newNote);

            var noteEls = noteElement.Elements().ToList();
            // See if note is part of a chord
            for (int i = 0; i < noteEls.Count; i++)
            {
                var element = noteEls[i];
                string tagName = element.Name.ToString();
                if (tagName == "instrument")
                {
                    PartContext header = partHeaders[partId];
                    MidiInstrument[] instruments = header.Instruments;
                    for (int y = 0; y < instruments.Length; ++y)
                    {
                        MidiInstrument ins = instruments[y];
                        if (ins != null && ins.Id == element.Attribute("id")?.Value)
                        {
                            ParseVoice(p, FindInstrument(ins));
                            ParseInstrumentAndFireChange(ins);
                        }
                    }
                }
                else if (tagName == "unpitched")
                {
                    // To Determine if Note is Percussive
                    newNote.IsPercussionNote = true;
                    var display_note = element.Element("display-step");
                    if (display_note != null)
                    {
                        noteNumber = GetNoteNumber(display_note.Value[0]);
                    }

                    var display_octave = element.Element("display-octave");
                    if (display_octave != null)
                    {
                        byte octave_byte = byte.Parse(display_octave.Value);
                        noteNumber += (byte)(octave_byte * 12);
                    }
                }
                else if (tagName == "pitch")
                {
                    string sStep = element.Element("step").Value;
                    noteNumber = GetNoteNumber(sStep[0]);
                    var alter = element.Element("alter");
                    if (alter != null)
                    {
                        noteNumber += (byte)int.Parse(alter.Value);
                        if (noteNumber > 11)
                        {
                            noteNumber = 0;
                        }
                        else if (noteNumber < 0)
                        {
                            noteNumber = 11;
                        }
                    }

                    byte.TryParse(element.Element("octave").Value, out octaveNumber);

                    // Compute the actual note number, based on octave and note
                    int intNoteNumber = ((octaveNumber) * 12) + noteNumber;
                    if (intNoteNumber > 127)
                    {
                        throw new ApplicationException("Note value " + intNoteNumber + " is larger than 127");
                    }
                    noteNumber = (byte)intNoteNumber;
                }
                else if (tagName == "rest")
                {
                    isRest = true;
                }
            }

            // duration
            var element_duration = noteElement.Element("duration");
            double durationValue = double.Parse(element_duration.Value);
            decimalDuration = durationValue / (divisionsPerBeat * beatsPerMeasure);

            // Tied Note
            var notations = noteElement.Element("notations");
            var tied = notations?.Element("tied");
            if (tied != null)
            {
                string tiedValue = tied.Attribute("type")?.Value;
                if (tiedValue.Equals("start", StringComparison.OrdinalIgnoreCase))
                {
                    isStartOfTie = true;
                }
                else if (tiedValue.Equals("stop", StringComparison.OrdinalIgnoreCase))
                {
                    isEndOfTie = true;
                }
            }

            int attackVelocity = currentPart.CurrentVolume;
            int decayVelocity = this.curVelocity;

            // Set up the note
            if (isRest)
            {
                newNote.IsRest = true;
                newNote.Duration = decimalDuration;

                // turn off sound for rest notes
                newNote.OnVelocity = 0;
                newNote.OffVelocity = 0;
            }
            else
            {
                newNote.Value = (sbyte)noteNumber;
                newNote.Duration = decimalDuration;
                newNote.IsStartOfTie = isStartOfTie;
                newNote.IsEndOfTie = isEndOfTie;
                newNote.OnVelocity = (sbyte)attackVelocity;
                newNote.OffVelocity = (sbyte)decayVelocity;
            }

            OnNoteParsed(newNote);

            // Add Lyric
            var lyric = noteElement.Element("lyric");
            var lyric_text_element = lyric?.Element("text");
            if (lyric_text_element != null)
            {
                OnLyricParsed(lyric_text_element.Value);
            }
        }

        private byte GetNoteNumber(char step)
        {
            byte note = 0;
            switch (step)
            {
                case 'C':
                    note = 0;
                    break;
                case 'D':
                    note = 2;
                    break;
                case 'E':
                    note = 4;
                    break;
                case 'F':
                    note = 5;
                    break;
                case 'G':
                    note = 7;
                    break;
                case 'A':
                    note = 9;
                    break;
                case 'B':
                    note = 11;
                    break;
            }
            return note;
        }

        private int FindInstrument(MidiInstrument ins)
        {
            if (ins.Name != null)
            {
                return (int)ins.Name.GetEnumValueFromDescription<Instrument>();
            }
            return 0;
        }

        private void EnhanceFromChord(XElement noteElement, Note note)
        {
            var note_elements = noteElement.Elements().ToList();
            foreach (var element in note_elements)
            {
                string tagName = element.Name.ToString();
                if (tagName == "chord")
                {
                    note.IsHarmonicNote = true;
                    note.IsFirstNote = false;
                }
            }
        }

        private void ParseGuitarChord(XElement harmony)
        {
            StringBuilder chordString = new StringBuilder(" ");
            AppendToChord(chordString, harmony, "root");
            var chordKind = harmony.Element("kind");
            if (chordKind != null)
            {
                string chordKindStr = Mappings.XmlToNFugueChordMap[chordKind.Value];
                if (chordKindStr != null)
                {
                    chordString.Append(chordKindStr);
                }
            }
            var chordInv = harmony.Element("inversion");
            if (chordInv != null)
            {
                int invValue = int.Parse(chordInv.Value);
                for (int i = 0; i < invValue; i++)
                {
                    chordString.Append("^");
                }
            }
            AppendToChord(chordString, harmony, "bass");
            OnChordParsed(new Chord(chordString.ToString()));
        }

        private void AppendToChord(StringBuilder chordString, XElement harmony, string @base)
        {
            var chord_root = harmony.Element(@base);
            if (chord_root == null) return;
            var chord_root_step = chord_root.Element(@base + "-step");
            if (chord_root_step != null)
            {
                chordString.Append(chord_root_step.Value);
            }
            var chord_root_alter = chord_root.Element(@base + "-alter");
            if (chord_root_alter != null)
            {
                if (chord_root_alter.Value == "-1")
                {
                    chordString.Append("b");
                }
                if (chord_root_alter.Value == "+1")
                {
                    chordString.Append("#");
                }
            }
        }

        private KeySignature ParseKeySignature(XElement attributes)
        {
            // scale 0 = minor, 1 = major
            sbyte key = keySignature.Key, scale = keySignature.Scale;
            var attr = attributes.Element("key");
            if (attr != null)
            {
                sbyte.TryParse(attr.Element("fifths")?.Value, out key);
                var eMode = attr.Element("mode");
                if (eMode != null)
                {
                    string mode = eMode.Value;
                    if (mode.Equals("major", StringComparison.OrdinalIgnoreCase))
                    {
                        scale = 0;
                    }
                    else if (mode.Equals("minor", StringComparison.OrdinalIgnoreCase))
                    {
                        scale = 1;
                    }
                    else
                    {
                        throw new ParserException("Error in key signature: " + mode);
                    }
                }
                else
                {
                    scale = 0;
                }
            }
            return new KeySignature(key, scale);
        }

        private void SwitchPart(Dictionary<string, PartContext> partHeaders, string partString, int partId)
        {
            currentPart = partHeaders[partString];
            if (currentPart.Voice >= 0)
            {
                OnTrackChanged((sbyte)currentPart.Voice);
            }
            else
            {
                // if there are no midi instruments for the part ie
                // the midi-instruments string length is 0
                if (currentPart.Instruments[0] == null)
                {
                    ParseVoice(partId, int.Parse(currentPart.Id));
                    // then pass the name of the part to the Instrument parser
                    ParseInstrumentNameAndFireChange(currentPart.Name);
                }
                else
                {
                    if (currentPart.Instruments[0]?.Channel != null)
                    {
                        ParseVoice(partId, int.Parse(currentPart.Instruments[0]?.Channel));
                    }
                    ParseInstrumentAndFireChange(currentPart.Instruments[0]);
                }
                currentLayer = 0;
                OnLayerChanged((sbyte)currentLayer);
            }
        }

        private void ParseInstrumentAndFireChange(MidiInstrument instrument)
        {
            if (instrument.Program >= 0)
            {
                OnInstrumentParsed((sbyte)instrument.Program);
            }
            else if (instrument.Name != null)
            {
                ParseInstrumentNameAndFireChange(instrument.Name);
            }
            else
            {
                throw new ParserException("Couldn't determine the instrument. Possibly and unhandled case. Please report with the musicXML data.");
            }
        }

        private void ParseInstrumentNameAndFireChange(string name)
        {
            sbyte instrumentNumber;
            if (!sbyte.TryParse(name, out instrumentNumber))
            {
                // otherwise map the midi_name to its byte code
                int? value = (int?)name.GetEnumValueFromDescription<Instrument>();
                instrumentNumber = (sbyte)((value == null) ? -1 : (sbyte)value);
            }
            if (instrumentNumber > -1)
            {
                OnInstrumentParsed(instrumentNumber);
            }
            else throw new ParserException();
        }

        private void ParseVoice(int part, int voice)
        {
            // This needs to be reworked as it probably should be stored in PartContext.
            if (voice == 10)
            {
                OnTrackChanged((sbyte)voice);
            }
            else
            {
                // scroll through voiceDef objects looking for this particular
                // combination of p v
                // XML part ID's are 1-based, JFugue voice numbers are 0-based
                sbyte voiceNumber = -1;

                for (byte x = 0; x < this.nextVoice; ++x)
                {
                    // class variable voices is an array of voiceDef objects. These
                    // objects match a part index to a voice index.
                    if (part == voices[x].Part && voice == voices[x].Voice)
                    {
                        voiceNumber = (sbyte)x;
                        break;
                    }
                }
                // if Voice not found, add a new voiceDef to the array
                if (voiceNumber == -1)
                {
                    voiceNumber = (sbyte)nextVoice;
                    voices[voiceNumber] = new VoiceDefinition(part, voice);
                    ++nextVoice;
                }
                if (voiceNumber != this.currentVoice)
                {
                    OnTrackChanged(voiceNumber);
                }
                currentVoice = voiceNumber;
            }
        }

        private Dictionary<string, PartContext> ParsePartList(IEnumerable<XElement> parts)
        {
            return parts.Select(ParsePartHeader)
                .Where(header => header != null)
                .ToDictionary(header => header.Id);
        }

        private PartContext ParsePartHeader(XElement part)
        {
            if (part.Name.LocalName == "part-group")
                return null;
            PartContext partHeader = new PartContext(part.Attribute("id").Value, part.Element("part-name").Value);
            var midiInstruments = part.Elements("midi-instrument").ToList();
            for (int x = 0; x < midiInstruments.Count; x++)
            {
                var instrument = midiInstruments[x];
                string instrumentId = instrument?.Attribute("id")?.Value;
                string channel = instrument?.Element("midi-channel")?.Value;
                string name = instrument?.Element("midi-name")?.Value;
                string bank = instrument?.Element("midi-bank")?.Value;
                int program = 0;
                int.TryParse(instrument?.Element("midi-program")?.Value, out program);
                string unpitched = instrument?.Element("midi-unpitched")?.Value;
                partHeader.Instruments[x] = new MidiInstrument(instrumentId, channel, name, bank, (byte)program, unpitched);
            }
            return partHeader;
        }

        private void ParseTimeWise(XElement root)
        {
            var partlist = root.Element("part-list");
            var scoreParts = partlist.Elements().ToList();
            Dictionary<string, PartContext> partHeaders = ParsePartList(scoreParts);
            var measures = root.Elements("measure").ToList();
            for (int measureIndex = 0; measureIndex < measures.Count; measureIndex++)
            {
                var measureElement = measures[measureIndex];
                var parts = measureElement.Elements("part").ToList();
                for (int partIndex = 0; partIndex < parts.Count; partIndex++)
                {
                    var partElement = parts[partIndex];
                    string partId = partElement.Attribute("id").Value;
                    SwitchPart(partHeaders, partId, measureIndex);
                    ParseMusicData(partIndex, partId, partHeaders, partElement);
                    OnBarLineParsed(0);
                }
            }

        }
    }
}