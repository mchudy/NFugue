using NFugue.Extensions;
using NFugue.Midi;
using NFugue.Parsing;
using NFugue.Patterns;
using NFugue.Providers;
using NFugue.Theory;
using System;
using System.Linq;

namespace NFugue.Staccato.Subparsers.NoteSubparser
{
    public class NoteSubparser : ISubparser, INoteProvider, IChordProvider
    {
        private readonly char[] charList = { 'C', 'D', 'E', 'F', 'G', 'A', 'B', 'R', '[', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        private static NoteSubparser instance;

        //TODO
        //private NoteSubparser() { }
        public static NoteSubparser Instance => instance ?? (instance = new NoteSubparser());

        public bool Matches(string music)
        {
            return charList.Contains(music[0]);
        }

        public TokenType GetTokenType(string tokenString)
        {
            if (Matches(tokenString))
            {
                return TokenType.Note;
            }
            return TokenType.UnknownToken;
        }

        public int Parse(string music, StaccatoParserContext context)
        {
            return ParseNoteElement(music, 0, context);
        }

        private int ParseNoteElement(string music, int index, StaccatoParserContext parserContext)
        {
            bool repeat = false;
            var noteContext = new NoteContext();
            do
            {
                index = ParseNoteElement(music, index, noteContext, parserContext);
                if (noteContext.IsChord)
                {
                    var chord = noteContext.CreateChord(parserContext);
                    parserContext.Parser.OnChordParsed(chord);
                }
                else
                {
                    var note = noteContext.CreateNote(parserContext);
                    parserContext.Parser.OnNoteParsed(note);
                }
                repeat = noteContext.IsThereAnother;
                noteContext = noteContext.CreateNextNoteContext();
            } while (repeat);
            return index;
        }

        private int ParseNoteElement(string music, int index, NoteContext noteContext, StaccatoParserContext parserContext)
        {
            music = music.ToUpper();
            index = ParseRoot(music, index, noteContext);
            int startInternalInterval = ParseOctave(music, index, noteContext);
            int startChord = ParseInternalInterval(music, startInternalInterval, noteContext);
            int startChordInversion = ParseChord(music, startChord, noteContext);
            if (index == startInternalInterval)
            {
                SetDefaultOctave(noteContext);
            }
            ComputeNoteValue(noteContext, parserContext);
            index = ParseChordInversion(music, startChordInversion, noteContext);
            index = ParseDuration(music, index, noteContext, parserContext);
            index = ParseVelocity(music, index, noteContext);
            index = ParseConnector(music, index, noteContext);
            return index;
        }

        private int ParseRoot(string music, int index, NoteContext noteContext)
        {
            if (music[index] >= 'A' && music[index] <= 'G')
            {
                return ParseLetterNote(music, index, noteContext);
            }
            if (music[index] == 'R')
            {
                return ParseRest(music, index, noteContext);
            }
            if (music[index] == '[')
            {
                return ParseBracketedNote(music, index, noteContext);
            }
            if (music[index] >= '0' && music[index] <= '9')
            {
                return ParseNumericNote(music, index, noteContext);
            }
            return 0;
        }

        private int ParseNumericNote(string music, int index, NoteContext noteContext)
        {
            int numCharsInNumber = 0;
            while (numCharsInNumber < music.Length && music[index + numCharsInNumber] >= '0' &&
                   music[index + numCharsInNumber] <= '9')
            {
                numCharsInNumber++;
            }
            string numericNoteString = music.Substring(index, numCharsInNumber);
            noteContext.NoteNumber = int.Parse(numericNoteString);
            noteContext.IsNumericNote = true;
            return index + numCharsInNumber;
        }

        private int ParseBracketedNote(string music, int index, NoteContext noteContext)
        {
            int endBracketIndex = music.IndexOf(']', index);
            string stringInBrackets = music.Substring(index + 1, endBracketIndex - index - 1);
            noteContext.NoteValueAsString = stringInBrackets;
            noteContext.IsNumericNote = true;
            return endBracketIndex + 1;
        }

        private int ParseRest(string music, int index, NoteContext noteContext)
        {
            noteContext.IsRest = true;
            return index + 1;
        }

        private int ParseLetterNote(string music, int index, NoteContext noteContext)
        {
            noteContext.IsNumericNote = false;
            int originalIndex = index;
            switch (music[index])
            {
                case 'C':
                    noteContext.NoteNumber = 0;
                    break;
                case 'D':
                    noteContext.NoteNumber = 2;
                    break;
                case 'E':
                    noteContext.NoteNumber = 4;
                    break;
                case 'F':
                    noteContext.NoteNumber = 5;
                    break;
                case 'G':
                    noteContext.NoteNumber = 7;
                    break;
                case 'A':
                    noteContext.NoteNumber = 9;
                    break;
                case 'B':
                    noteContext.NoteNumber = 11;
                    break;
            }
            index++;
            bool checkForModifiers = true;
            while (checkForModifiers)
            {
                if (index < music.Length)
                {
                    switch (music[index])
                    {
                        case '#':
                            index++;
                            noteContext.NoteNumber++;
                            if (noteContext.NoteNumber == 12)
                            {
                                noteContext.NoteNumber = 0;
                                noteContext.OctaveBias++;
                            }
                            break;
                        case 'B':
                            index++;
                            noteContext.NoteNumber--;
                            if (noteContext.NoteNumber == -1)
                            {
                                noteContext.NoteNumber = 11;
                                noteContext.OctaveBias--;
                            }
                            break;
                        case 'N':
                            index++;
                            noteContext.IsNatural = true;
                            checkForModifiers = false;
                            break;
                        default:
                            checkForModifiers = false;
                            break;
                    }
                }
                else
                {
                    checkForModifiers = false;
                }
            }
            noteContext.OriginalString = music.Substring(originalIndex, index - originalIndex);
            return index;
        }

        private int ParseChordInversion(string s, int index, NoteContext context)
        {
            if (!context.IsChord)
            {
                return index;
            }

            int inversionCount = 0;
            bool bassNote = false;
            int startIndex = index;
            bool checkForInversion = true;
            while (checkForInversion)
            {
                if (index < s.Length)
                {
                    switch (s[index])
                    {
                        case '^': index++; inversionCount++; break;
                        case 'C': index++; bassNote = true; break;
                        case 'D': index++; bassNote = true; break;
                        case 'E': index++; bassNote = true; break;
                        case 'F': index++; bassNote = true; break;
                        case 'G': index++; bassNote = true; break;
                        case 'A': index++; bassNote = true; break;
                        case 'B': index++; bassNote = true; break;
                        case '#': index++; break; // presumably the sharp mark followed a note
                                                  // For '0', need to differentiate between initial 0 and 0 as a second digit (i.e., 10)
                        case '0': index++; inversionCount = (inversionCount == -1) ? 0 : inversionCount + 10; break;
                        case '1': index++; inversionCount = 1; break;
                        case '2': index++; inversionCount = 2; break;
                        case '3': index++; inversionCount = 3; break;
                        case '4': index++; inversionCount = 4; break;
                        case '5': index++; inversionCount = 5; break;
                        case '6': index++; inversionCount = 6; break;
                        case '7': index++; inversionCount = 7; break;
                        case '8': index++; inversionCount = 8; break;
                        case '9': index++; inversionCount = 9; break;
                        // For '[', we're checking for a note number after the inversion marker
                        case '[':
                            int indexEndBracket = s.IndexOf(']', index);
                            context.InversionBassNote = Note.GetToneString(int.Parse(s.Substring(index + 1, indexEndBracket - 2 - index)));
                            index = indexEndBracket + 1;
                            break;
                        default:
                            checkForInversion = false;
                            break;
                    }
                }
                else
                {
                    checkForInversion = false;
                }
            }
            // Modify the note values based on the inversion
            if (bassNote)
            {
                context.InversionBassNote = s.Substring(startIndex + 1, index - startIndex - 1);
            }
            else if (inversionCount > 0)
            {
                context.InversionCount = inversionCount;
            }
            return index;
        }

        private int ParseDuration(string s, int index, NoteContext noteContext, StaccatoParserContext parserContext)
        {
            if (index < s.Length)
            {
                switch (s[index])
                {
                    case '/': index = ParseNumericDuration(s, index, noteContext); break;
                    case 'W':
                    case 'H':
                    case 'Q':
                    case 'I':
                    case 'S':
                    case 'T':
                    case 'X':
                    case 'O':
                    case '-': index = ParseLetterDuration(s, index, noteContext, parserContext); break;
                    default:
                        noteContext.DecimalDuration = DefaultNoteSettings.DefaultDuration;
                        noteContext.IsDurationExplicitlySet = false;
                        break; // Could get here if the next character is a velocity char ("a" or "d")
                }
                index = ParseTuplet(s, index, noteContext);
            }
            else
            {
                noteContext.DecimalDuration = DefaultNoteSettings.DefaultDuration;
                noteContext.IsDurationExplicitlySet = false;
            }
            return index;
        }

        private int ParseTuplet(string s, int index, NoteContext context)
        {
            if (index >= s.Length) return index;
            if (s[index] == '*')
            {
                index++;
                // Figure out tuplet ratio, or figure out when to stop looking for tuplet info
                bool stopTupletParsing = false;
                int indexOfUnitsToMatch = 0;
                int indexOfNumNotes = 0;
                int counter = -1;
                while (!stopTupletParsing)
                {
                    counter++;
                    if (s.Length > index + counter)
                    {
                        if (s[index + counter] == ':')
                        {
                            indexOfNumNotes = index + counter + 1;
                        }
                        else if ((s[index + counter] >= '0') && (s[index + counter] <= '9'))
                        {
                            if (indexOfUnitsToMatch == 0)
                            {
                                indexOfUnitsToMatch = index + counter;
                            }
                        }
                        else if (s[index + counter] == '*')
                        {
                            // no op... artifact of parsing
                        }
                        else
                        {
                            stopTupletParsing = true;
                        }
                    }
                    else
                    {
                        stopTupletParsing = true;
                    }
                }
                index += counter;

                double numerator = 3.0d;
                double denominator = 2.0d;
                if ((indexOfUnitsToMatch > 0) && (indexOfNumNotes > 0))
                {
                    numerator = double.Parse(s.Substring(indexOfUnitsToMatch, indexOfNumNotes - 1 - indexOfUnitsToMatch));
                    denominator = double.Parse(s.Substring(indexOfNumNotes, index - indexOfNumNotes));
                }
                double tupletRatio = numerator / denominator;
                context.DecimalDuration = context.DecimalDuration * (1.0d / tupletRatio);
                context.IsDurationExplicitlySet = true;
            }
            return index;
        }

        private int ParseLetterDuration(string s, int index, NoteContext context, StaccatoParserContext parserContext)
        {
            bool moreDurationCharsToParse = true;
            bool isDotted = false;

            while (moreDurationCharsToParse)
            {
                int durationNumber = 0;
                // See if the note has a duration. Duration is optional for a note.
                if (index < s.Length)
                {
                    char durationChar = s[index];
                    switch (durationChar)
                    {
                        case '-':
                            if ((context.DecimalDuration == 0.0) && (!context.IsEndOfTie))
                            {
                                context.IsEndOfTie = true;
                            }
                            else
                            {
                                context.IsStartOfTie = true;
                            }
                            break;
                        case 'W': durationNumber = 1; break;
                        case 'H': durationNumber = 2; break;
                        case 'Q': durationNumber = 4; break;
                        case 'I': durationNumber = 8; break;
                        case 'S': durationNumber = 16; break;
                        case 'T': durationNumber = 32; break;
                        case 'X': durationNumber = 64; break;
                        case 'O': durationNumber = 128; break;
                        default: index--; moreDurationCharsToParse = false; break;
                    }
                    index++;
                    if ((index < s.Length) && (s[index] == '.'))
                    {
                        isDotted = true;
                        index++;
                    }
                    if (durationNumber > 0)
                    {
                        context.IsDurationExplicitlySet = true;
                        double d = 1.0 / durationNumber;
                        if (isDotted)
                        {
                            context.DecimalDuration += d + (d / 2.0);
                        }
                        else
                        {
                            context.DecimalDuration += d;
                        }
                    }
                    context.MostRecentDuration = durationNumber;
                    if ((index < s.Length) && (s[index] >= '0') && (s[index] <= '9'))
                    {
                        index = ParseQuantityDuration(s, index, context);
                    }
                }
                else
                {
                    moreDurationCharsToParse = false;
                }
            }
            return index;
        }

        private int ParseQuantityDuration(string s, int index, NoteContext context)
        {
            // A quantity is associated with the duration, like the '24' in "w24"
            int endingIndex = SeekToEndOfDecimal(s, index);
            string quantityNumberString = s.Substring(index, endingIndex - index);
            context.DecimalDuration += 1.0f / context.MostRecentDuration * (double.Parse(quantityNumberString) - 1.0D); // Subtract 1, because mostRecentDuration has already been added to the total duration
            return endingIndex;
        }

        private int ParseNumericDuration(string s, int index, NoteContext context)
        {
            // The duration has come in as a number, like 0.25 for a quarter note.
            // Advance pointer past the initial slash (/)
            index++;

            // If first character before the numeric value is a dash, we're ending a tie
            if (s[index] == '-')
            {
                context.IsEndOfTie = true;
                index++;
            }

            context.IsDurationExplicitlySet = true;

            // Get the duration value
            int endingIndex = SeekToEndOfDecimal(s, index);
            string durationNumberString = s.Substring(index, endingIndex - index);
            context.DecimalDuration += double.Parse(durationNumberString);
            index = endingIndex;

            // If the character after all of the value parsing is a dash, we're starting a tie
            if ((index < s.Length) && (s[index] == '-'))
            {
                context.IsStartOfTie = true;
                index++;
            }
            return index;
        }

        private int SeekToEndOfDecimal(string s, int index)
        {
            int cursor = index;
            while (cursor < s.Length && (s[cursor] == '.' || ((s[cursor] >= '0') && (s[cursor] <= '9'))))
            {
                cursor++;
            }
            return cursor;
        }

        private int ParseVelocity(string s, int index, NoteContext context)
        {
            if (context.IsRest) return index;

            // Process velocity attributes, if they exist
            while (index < s.Length)
            {
                int startPoint = index + 1;
                int endPoint = startPoint;

                char velocityChar = s[index];
                int lengthOfByte = 0;
                if ((velocityChar == '+') || (velocityChar == '_') || (velocityChar == ' ')) break;
                bool byteDone = false;
                while (!byteDone && (index + lengthOfByte + 1 < s.Length))
                {
                    char possibleByteChar = s[index + lengthOfByte + 1];
                    if ((possibleByteChar >= '0') && (possibleByteChar <= '9'))
                    {
                        lengthOfByte++;
                    }
                    else
                    {
                        byteDone = true;
                    }
                }
                endPoint = index + lengthOfByte + 1;
                if (startPoint == endPoint)
                {
                    return endPoint;
                }

                int velocityNumber = int.Parse(s.Substring(startPoint, endPoint - startPoint));
                // Or maybe a bracketed string was passed in, instead of a byte
                string velocityString = null;
                if ((index + 1 < s.Length) && (s[index + 1] == '['))
                {
                    endPoint = s.IndexOf(']', startPoint) + 1;
                    velocityString = s.Substring(startPoint, endPoint - startPoint);
                }

                switch (velocityChar)
                {
                    case 'A':
                        if (velocityString == null)
                        {
                            context.NoteOnVelocity = velocityNumber;
                        }
                        else
                        {
                            context.NoteOnVelocityValueAsString = velocityString;
                        }
                        context.HasNoteOnVelocity = true;
                        break;
                    case 'D':
                        if (velocityString == null)
                        {
                            context.NoteOffVelocity = velocityNumber;
                        }
                        else
                        {
                            context.NoteOffVelocityValueAsString = velocityString;
                        }
                        context.HasNoteOffVelocity = true;
                        break;
                    default: throw new ParserException(StaccatoMessages.VelocityCharacterNotRecognized + s.Substring(startPoint, endPoint - startPoint));
                }
                index = endPoint;
            }
            return index;
        }

        private int ParseConnector(string s, int index, NoteContext context)
        {
            context.IsThereAnother = false;
            // See if there's another note to process
            if ((index < s.Length) && ((s[index] == '+') || (s[index] == '_')))
            {
                if (s[index] == '_')
                {
                    context.AnotherNoteIsMelodic = true;
                }
                else
                {
                    context.AnotherNoteIsHarmonic = true;
                }
                index++;
                context.IsThereAnother = true;
            }
            return index;
        }

        private void ComputeNoteValue(NoteContext noteContext, StaccatoParserContext parserContext)
        {
            if (noteContext.IsRest) return;

            // Adjust for Key Signature
            if (DefaultNoteSettings.AdjustNotesByKeySignature)
            {
                if (parserContext.Key != null)
                {
                    int keySig = KeyProviderFactory.GetKeyProvider().ConvertKeyToInt(parserContext.Key);
                    if ((keySig != 0) && (!noteContext.IsNatural))
                    {
                        if ((keySig <= -1) && (noteContext.NoteNumber == 11)) noteContext.NoteNumber = 10;
                        if ((keySig <= -2) && (noteContext.NoteNumber == 4)) noteContext.NoteNumber = 3;
                        if ((keySig <= -3) && (noteContext.NoteNumber == 9)) noteContext.NoteNumber = 8;
                        if ((keySig <= -4) && (noteContext.NoteNumber == 2)) noteContext.NoteNumber = 1;
                        if ((keySig <= -5) && (noteContext.NoteNumber == 7)) noteContext.NoteNumber = 6;
                        if ((keySig <= -6) && (noteContext.NoteNumber == 0)) { noteContext.NoteNumber = 11; noteContext.OctaveNumber--; }
                        if ((keySig <= -7) && (noteContext.NoteNumber == 5)) noteContext.NoteNumber = 4;
                        if ((keySig >= +1) && (noteContext.NoteNumber == 5)) noteContext.NoteNumber = 6;
                        if ((keySig >= +2) && (noteContext.NoteNumber == 0)) noteContext.NoteNumber = 1;
                        if ((keySig >= +3) && (noteContext.NoteNumber == 7)) noteContext.NoteNumber = 8;
                        if ((keySig >= +4) && (noteContext.NoteNumber == 2)) noteContext.NoteNumber = 3;
                        if ((keySig >= +5) && (noteContext.NoteNumber == 9)) noteContext.NoteNumber = 10;
                        if ((keySig >= +6) && (noteContext.NoteNumber == 4)) noteContext.NoteNumber = 5;
                        if ((keySig >= +7) && (noteContext.NoteNumber == 11)) { noteContext.NoteNumber = 0; noteContext.OctaveNumber++; }
                    }
                }
            }

            // Compute the actual note number, based on octave and note
            if (!noteContext.IsNumericNote)
            {
                int intNoteNumber = noteContext.OctaveNumber * 12 + noteContext.NoteNumber + noteContext.InternalInterval;
                if (intNoteNumber > 127)
                {
                    throw new ParserException(StaccatoMessages.CalculatedNoteOutOfRange + intNoteNumber);
                }
                noteContext.NoteNumber = (byte)intNoteNumber;
            }
        }

        private void SetDefaultOctave(NoteContext noteContext)
        {
            if (noteContext.IsChord)
            {
                noteContext.OctaveNumber = DefaultNoteSettings.DefaultBassOctave + noteContext.OctaveBias;
            }
            else
            {
                noteContext.OctaveNumber = DefaultNoteSettings.DefaultOctave + noteContext.OctaveBias;
            }
        }

        private int ParseChord(string s, int index, NoteContext context)
        {
            if (context.IsRest) return index;

            int lengthOfChordString = 0;
            string[] chordNames = Chord.GetChordNames();
            foreach (string chordName in chordNames)
            {
                if ((s.Length >= index + chordName.Length) && chordName == s.Substring(index, chordName.Length))
                {
                    lengthOfChordString = chordName.Length;
                    context.IsChord = true;
                    context.Intervals = Chord.GetIntervals(chordName);
                    context.ChordName = chordName;
                    break;
                }
            }
            return index + lengthOfChordString;
        }

        private int ParseInternalInterval(string s, int index, NoteContext context)
        {
            if (context.IsRest) return index;

            // An internal interval is indicated by a single quote
            if ((index < s.Length) && (s[index] == '\''))
            {
                int intervalLength = 0;
                // Verify that index+1 is a number representing the interval.
                if (index + 1 < s.Length && IsValidIntervalChar(s[index + 1]))
                {
                    intervalLength = 1;
                }
                // We'll allow for the possibility of double-sharps and double-flats. 
                if ((intervalLength == 1) && (index + 2 < s.Length) && IsValidIntervalChar(s[index + 2]))
                {
                    intervalLength = 2;
                }
                if ((intervalLength == 2) && (index + 3 < s.Length) && IsValidIntervalChar(s[index + 3]))
                {
                    intervalLength = 3;
                }
                context.InternalInterval = Intervals.GetHalfsteps(s.Substring(index + 1, intervalLength));
                context.OriginalString = Note.ToneStringWithoutOctave((context.NoteNumber + context.InternalInterval)) + (context.IsOctaveExplicitlySet ? context.OctaveNumber.ToString() : "");
                return index + intervalLength + 1;
            }
            return index;
        }

        private bool IsValidIntervalChar(char ch) => ((ch >= '0') && (ch <= '9')) || (ch == '#') || (ch == 'B');

        private int ParseOctave(string music, int index, NoteContext context)
        {
            context.IsOctaveExplicitlySet = false;
            // Don't parse an actave for a rest or a numeric note
            if (context.IsRest || context.IsNumericNote)
            {
                return index;
            }
            // Check for octave.  Remember that octaves are optional.
            // Octaves can be two digits, which is what this next bit is testing for.
            // But, there could be no octave here as well. 
            char possibleOctave1 = '.';
            char possibleOctave2 = '.';
            if (index < music.Length)
            {
                possibleOctave1 = music[index];
            }
            if (index + 1 < music.Length)
            {
                possibleOctave2 = music[index + 1];
            }

            byte definiteOctaveLength = 0;
            if (possibleOctave1 >= '0' && possibleOctave1 <= '9')
            {
                definiteOctaveLength = 1;
                if (possibleOctave2 == '0')
                {
                    definiteOctaveLength = 2;
                }
                string octaveNumberString = music.Substring(index, definiteOctaveLength);
                int octave;
                if (int.TryParse(octaveNumberString, out octave) && octave >= Note.MinOctave && octave <= Note.MaxOctave)
                {
                    context.OctaveNumber = octave;
                    context.IsOctaveExplicitlySet = true;
                }
                else
                {
                    throw new ParserException(StaccatoMessages.OctaveOutOfRange + music);
                }
                context.OriginalString = context.OriginalString + octaveNumberString;
            }
            return index + definiteOctaveLength;
        }

        #region INoteProvider

        public Note MiddleC => CreateNote("C");

        public Note CreateNote(string noteString)
        {
            StaccatoParserContext parserContext = new StaccatoParserContext(new StaccatoParser());
            NoteContext noteContext = new NoteContext();
            ParseNoteElement(noteString, 0, noteContext, parserContext);
            return noteContext.CreateNote(parserContext);
        }

        public double GetDurationForString(string s)
        {
            NoteContext noteContext = new NoteContext();
            StaccatoParserContext parserContext = new StaccatoParserContext(new StaccatoParser());
            ParseDuration(s, 0, noteContext, parserContext);
            return noteContext.DecimalDuration;
        }
        #endregion

        #region IChordProvider

        public Chord CreateChord(string chordString)
        {
            // If the user requested a chord like "C" or "Ab" without providing any additional details, assume it's MAJOR
            if (chordString.Length <= 2)
            {
                chordString = chordString + "MAJ";
            }

            StaccatoParserContext parserContext = new StaccatoParserContext(new StaccatoParser());
            NoteContext noteContext = new NoteContext();
            ParseNoteElement(chordString, 0, noteContext, parserContext);
            return noteContext.CreateChord(parserContext);
        }
        #endregion

        public static void PopulateContext(StaccatoParserContext context)
        {
            foreach (PercussionInstrument percussionInstrument in Enum.GetValues(typeof(PercussionInstrument))
                .OfType<PercussionInstrument>())
            {
                context.Dictionary[percussionInstrument.GetDescription()] = (int)percussionInstrument;
            }

            foreach (string key in Chord.ChordMap.Keys)
            {
                context.Dictionary[key] = Chord.ChordMap[key];
            }
        }
    }
}