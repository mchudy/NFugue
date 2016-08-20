namespace Staccato
{
    public static class StaccatoMessages
    {
        public static readonly string NoParserFound = "No parser was found for the following element: ";
        public static readonly string NoTimeSignatureSeparator = "In the following element, could not find a slash ('/') to separate the numerator from the denominator in the Time Signature:";
        public static readonly string OctaveOutOfRange = "The following value, parsed as an octave, is not in the expected range of 0 to 10:";
        public static readonly string CalculatedNoteOutOfRange = "The following value for a note, calculated by computing (octave*12)+noteValue, is not in the range 0 - 127: ";
        public static readonly string VelocityCharacterNotRecognized = "The following character, parsed as a note velocity, is not recognized: ";
    }
}
