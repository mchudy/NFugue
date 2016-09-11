using System.ComponentModel;

namespace NFugue.Midi
{
    public enum PercussionInstrument
    {
        [Description("ACOUSTIC_BASS_DRUM")]
        AcousticBassDrum = 35,

        [Description("BASS_DRUM")]
        BassDrum = 36,

        [Description("SIDE_STICK")]
        SideStick = 37,

        [Description("ACOUSTIC_SNARE")]
        AcousticSnare = 38,

        [Description("HAND_CLAP")]
        HandClap = 39,

        [Description("ELECTRIC_SNARE")]
        ElectricSnare = 40,

        [Description("LO_FLOOR_TOM")]
        LoFloorTom = 41,

        [Description("CLOSED_HI_HAT")]
        ClosedHiHat = 42,

        [Description("HI_FLOOR_TOM")]
        HighFloorTom = 43,

        [Description("PEDAL_HI_HAT")]
        PedalHiHat = 44,

        [Description("LO_TOM")]
        LoTom = 45,

        [Description("OPEN_HI_HAT")]
        OpenHiHat = 46,

        [Description("LO_MID_TOM")]
        LoMidTom = 47,

        [Description("HI_MID_TOM")]
        HiMidTom = 48,

        [Description("CRASH_CYMBAL_1")]
        CrashCymbal1 = 49,

        [Description("HI_TOM")]
        HiTom = 50,

        [Description("RIDE_CYMBAL_1")]
        RideCymbal1 = 51,

        [Description("CHINESE_CYMBAL")]
        ChineseCymbal = 52,

        [Description("RIDE_BELL")]
        RideBell = 53,

        [Description("TAMBOURINE")]
        Tambourine = 54,

        [Description("SPLASH_CYMBAL")]
        SplashCymbal = 55,

        [Description("COWBELL")]
        Cowbell = 56,

        [Description("CRASH_CYMBAL_2")]
        CrashCymbal2 = 57,

        [Description("VIBRASLAP")]
        Vibraslap = 58,

        [Description("RIDE_CYMBAL")]
        RideCymbal = 59,

        [Description("HI_BONGO")]
        HiBongo = 60,

        [Description("LO_BONGO")]
        LoBongo = 61,

        [Description("MUTE_HI_CONGA")]
        MuteHiConga = 62,

        [Description("OPEN_HI_CONGA")]
        OpenHiConga = 63,

        [Description("LO_CONGA")]
        LoConga = 64,

        [Description("HI_TIMBALE")]
        HiTimbale = 64,

        [Description("LO_TIMBALE")]
        LoTimbale = 66,

        [Description("HI_AGOGO")]
        HiAgogo = 67,

        [Description("LO_AGOGO")]
        LoAgogo = 68,

        [Description("CABASA")]
        Cabasa = 69,

        [Description("MARACAS")]
        Maracas = 70,

        [Description("SHORT_WHISTLE")]
        ShortWhistle = 71,

        [Description("LONG_WHISTLE")]
        LongWhistle = 72,

        [Description("SHORT_GUIRO")]
        ShortGuiro = 73,

        [Description("LONG_GUIRO")]
        LongGuiro = 74,

        [Description("CLAVES")]
        Claves = 75,

        [Description("HI_WOOD_BLOCK")]
        HiWoodBlock = 76,

        [Description("LO_WOOD_BLOCK")]
        LoWoodBlock = 77,

        [Description("MUTE_CUICA")]
        MuteCuica = 78,

        [Description("OPEN_CUICA")]
        OpenCuica = 79,

        [Description("MUTE_TRIANGLE")]
        MuteTriangle = 80,

        [Description("OPEN_TRIANGLE")]
        OpenTriangle = 81
    }
}