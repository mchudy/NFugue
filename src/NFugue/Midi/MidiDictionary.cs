using System.Collections.Generic;
using System.Linq;

namespace NFugue.Midi
{
    public class MidiDictionary
    {
        static MidiDictionary()
        {
            InstrumentStringToInt = InstrumentIntToString.GroupBy(p => p.Value)
                .ToDictionary(g => g.Key.ToUpper(), g => g.Select(pp => pp.Key).FirstOrDefault());
        }

        public static IDictionary<string, int> InstrumentStringToInt { get; }

        public static IDictionary<string, int> TempoStringToInt { get; } = new Dictionary<string, int>()
        {
            {"ADAGIO", 60},
            {"ALLEGRO", 120 }
        };

        public static IDictionary<int, string> InstrumentIntToString { get; set; } = new Dictionary<int, string>
        {
            {0, "Piano"},
            {60, "French_Horn" },
            {73, "Flute" }
        };
    }

    //public enum MidiInstrument
    //{
    //    AcousticGrandPiano = 0,
    //    BrightAcousticPiano = 1,
    //    ElectricGrandPiano = 2,
    //    HonkyTonkPiano = 3,
    //    ElectricPiano1 = 4,
    //    ElectricPiano2 = 5,
    //    Harpsichord = 6,
    //    Clavinet = 7,

    //    Celesta = 8,
    //    Glockenspiel = 9,
    //    MusicBox = 10,
    //    Vibraphone = 11,
    //    Marimba = 12,
    //    Xylophone = 13,
    //    TubularBells = 14,
    //    Dulcimer = 15,

    //    DrawbarOrgan = 16,
    //    PercussiveOrgan = 17,
    //    RockOrgan = 18,
    //    ChurchOrgan = 19,
    //    ReedOrgan = 20,
    //    Accordion = 21,
    //    Harmonica = 22,
    //    TangoAccordion = 23,

    //    AcousticGuitarNylon = 24,
    //    AcousticGuitarSteel = 25,
    //    ElectricGuitarJazz = 26,
    //    ElectricGuitarClean = 27,
    //    ElectricGuitarMuted = 28,
    //    OverdrivenGuitar = 29,
    //    DistortionGuitar = 30,
    //    GuitarHarmonics = 31,

    //    AcousticBass = 32,
    //    ElectricBassFinger = 33,
    //    ElectricBassPick = 34,
    //    FretlessBass = 35,
    //    SlapBass1 = 36,
    //    SlapBass2 = 37,
    //    SynthBass1 = 38,
    //    SynthBass2 = 39,

    //    Violin = 40,
    //    Viola = 41,
    //    Cello = 42,
    //    Contrabass = 43,
    //    TremoloStrings = 44,
    //    PizzicatoStrings = 45,
    //    OrchestralHarp = 46,
    //    Timpani = 47,

    //    StringEnsemble1 = 48,
    //    StringEnsemble2 = 49,
    //    SynthStrings1 = 50,
    //    SynthStrings2 = 51,
    //    ChoirAahs = 52,
    //    VoiceOohs = 53,
    //    SynthVoice = 54,
    //    OrchestraHit = 55,

    //    Trumpet = 56,
    //    Trombone = 57,
    //    Tuba = 58,
    //    MutedTrumpet = 59,
    //    FrenchHorn = 60,
    //    BrassSection = 61,
    //    SynthBrass1 = 62,
    //    SynthBrass2 = 63,

    //    SopranoSax = 64,
    //    AltoSax = 65,
    //    TenorSax = 66,
    //    BaritoneSax = 67,
    //    Oboe = 68,
    //    EnglishHorn = 69,
    //    Bassoon = 70,
    //    Clarinet = 71,

    //    Piccolo = 72,
    //    Flute = 73,
    //    Recorder = 74,
    //    PanFlute = 75,
    //    BlownBottle = 76,
    //    Shakuhachi = 77,
    //    Whistle = 78,
    //    Ocarina = 79,

    //    Lead1Square = 80,
    //    Lead2Sawtooth = 81,
    //    Lead3Calliope = 82,
    //    Lead4Chiff = 83,
    //    Lead5Charang = 84,
    //    Lead6Voice = 85,
    //    Lead7Fifths = 86,
    //    Lead8BassPlusLead = 87,

    //    Pad1NewAge = 88,
    //    Pad2Warm = 89,
    //    Pad3Polysynth = 90,
    //    Pad4Choir = 91,
    //    Pad5Bowed = 92,
    //    Pad6Metallic = 93,
    //    Pad7Halo = 94,
    //    Pad8Sweep = 95,

    //    FX1Rain = 96,
    //    FX2Soundtrack = 97,
    //    FX3Crystal = 98,
    //    FX4Atmosphere = 99,
    //    FX5Brightness = 100,
    //    FX6Goblins = 101,
    //    FX7Echoes = 102,
    //    FX8SciFi = 103,

    //    Sitar = 104,
    //    Banjo = 105,
    //    Shamisen = 106,
    //    Koto = 107,
    //    Kalimba = 108,
    //    Bagpipe = 109,
    //    Fiddle = 110,
    //    Shanai = 111,

    //    TinkleBell = 112,
    //    Agogo = 113,
    //    SteelDrums = 114,
    //    Woodblock = 115,
    //    TaikoDrum = 116,
    //    MelodicTom = 117,
    //    SynthDrum = 118,
    //    ReverseCymbal = 119,

    //    GuitarFretNoise = 120,
    //    BreathNoise = 121,
    //    Seashore = 122,
    //    BirdTweet = 123,
    //    TelephoneRing = 124,
    //    Helicopter = 125,
    //    Applause = 126,
    //    Gunshot = 127
    //}
}