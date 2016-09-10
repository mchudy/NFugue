using System.ComponentModel;

namespace NFugue.Midi
{
    public enum Instrument
    {
        [Description("Piano")]
        AcousticGrandPiano = 0,

        [Description("Bright_Acoustic")]
        BrightAcousticPiano = 1,

        [Description("Electric_Grand")]
        ElectricGrandPiano = 2,

        [Description("Honkey_Tonk")]
        HonkyTonkPiano = 3,

        [Description("Electric_Piano")]
        ElectricPiano1 = 4,

        [Description("Electric_Piano_2")]
        ElectricPiano2 = 5,

        [Description("Harpsichord")]
        Harpsichord = 6,

        [Description("Clavinet")]
        Clavinet = 7,

        [Description("Celesta")]
        Celesta = 8,

        [Description("Glockenspiel")]
        Glockenspiel = 9,

        [Description("Music_Box")]
        MusicBox = 10,

        [Description("Vibraphone")]
        Vibraphone = 11,

        [Description("Marimba")]
        Marimba = 12,

        [Description("Xylophone")]
        Xylophone = 13,

        [Description("Tubular_Bells")]
        TubularBells = 14,

        [Description("Dulcimer")]
        Dulcimer = 15,

        [Description("Drawbar_Organ")]
        DrawbarOrgan = 16,

        [Description("Percussive_Organ")]
        PercussiveOrgan = 17,

        [Description("Rock_Organ")]
        RockOrgan = 18,

        [Description("Church_Organ")]
        ChurchOrgan = 19,

        [Description("Reed_Organ")]
        ReedOrgan = 20,

        [Description("Accordion")]
        Accordion = 21,

        [Description("Harmonica")]
        Harmonica = 22,

        [Description("Tango_Accordion")]
        TangoAccordion = 23,

        [Description("Guitar")]
        AcousticGuitarNylon = 24,

        [Description("Steel_String_Guitar")]
        AcousticGuitarSteel = 25,

        [Description("Electric_Jazz_Guitar")]
        ElectricGuitarJazz = 26,

        [Description("Electric_Clean_Guitar")]
        ElectricGuitarClean = 27,

        [Description("Electric_Muted_Guitar")]
        ElectricGuitarMuted = 28,

        [Description("Overdriven_Guitar")]
        OverdrivenGuitar = 29,

        [Description("Distortion_Guitar")]
        DistortionGuitar = 30,

        [Description("Guitar_Harmonics")]
        GuitarHarmonics = 31,

        [Description("Acoustic_Bass")]
        AcousticBass = 32,

        [Description("Electric_Bass_Finger")]
        ElectricBassFinger = 33,

        [Description("Electric_Bass_Pick")]
        ElectricBassPick = 34,

        [Description("Fretless_Bass")]
        FretlessBass = 35,

        [Description("Slap_Bass_1")]
        SlapBass1 = 36,

        [Description("Slap_Bass_2")]
        SlapBass2 = 37,

        [Description("Synth_Bass_1")]
        SynthBass1 = 38,

        [Description("Synth_Bass_2")]
        SynthBass2 = 39,

        [Description("Violin")]
        Violin = 40,

        [Description("Viola")]
        Viola = 41,

        [Description("Cello")]
        Cello = 42,

        [Description("Contrabass")]
        Contrabass = 43,

        [Description("Tremolo_Strings")]
        TremoloStrings = 44,

        [Description("Pizzicato_Strings")]
        PizzicatoStrings = 45,

        [Description("Orchestral_Strings")]
        OrchestralHarp = 46,

        [Description("Timpani")]
        Timpani = 47,

        [Description("String_Ensemble_1")]
        StringEnsemble1 = 48,

        [Description("String_Ensemble_2")]
        StringEnsemble2 = 49,

        [Description("Synth_Strings_1")]
        SynthStrings1 = 50,

        [Description("Synth_Strings_2")]
        SynthStrings2 = 51,

        [Description("Choir_Aahs")]
        ChoirAahs = 52,

        [Description("Voice_Oohs")]
        VoiceOohs = 53,

        [Description("Synth_Voice")]
        SynthVoice = 54,

        [Description("Orchestra_Hit")]
        OrchestraHit = 55,

        [Description("Trumpet")]
        Trumpet = 56,

        [Description("Trombone")]
        Trombone = 57,

        [Description("Tuba")]
        Tuba = 58,

        [Description("Muted_Trumpet")]
        MutedTrumpet = 59,

        [Description("French_Horn")]
        FrenchHorn = 60,

        [Description("Brass_Section")]
        BrassSection = 61,

        [Description("Synth_Brass_1")]
        SynthBrass1 = 62,

        [Description("Synth_Brass_2")]
        SynthBrass2 = 63,

        [Description("Soprano_Sax")]
        SopranoSax = 64,

        [Description("Alto_Sax")]
        AltoSax = 65,

        [Description("Tenor_Sex")]
        TenorSax = 66,

        [Description("Baritone_Sax")]
        BaritoneSax = 67,

        [Description("Oboe")]
        Oboe = 68,

        [Description("English_Horn")]
        EnglishHorn = 69,

        [Description("Basoon")]
        Bassoon = 70,

        [Description("Clarinet")]
        Clarinet = 71,

        [Description("Piccolo")]
        Piccolo = 72,

        [Description("Flute")]
        Flute = 73,

        [Description("Recorder")]
        Recorder = 74,

        [Description("Pan_Flute")]
        PanFlute = 75,

        [Description("Blown_Bottle")]
        BlownBottle = 76,

        [Description("Shakuhachi")]
        Shakuhachi = 77,

        [Description("Whistle")]
        Whistle = 78,

        [Description("Ocarina")]
        Ocarina = 79,

        [Description("Square")]
        Lead1Square = 80,

        [Description("Sawtooth")]
        Lead2Sawtooth = 81,

        [Description("Calliope")]
        Lead3Calliope = 82,

        [Description("Chiff")]
        Lead4Chiff = 83,

        [Description("Charang")]
        Lead5Charang = 84,

        [Description("Voice")]
        Lead6Voice = 85,

        [Description("Fifths")]
        Lead7Fifths = 86,

        [Description("Bass_Lead")]
        Lead8BassPlusLead = 87,

        [Description("New_Age")]
        Pad1NewAge = 88,

        [Description("Warm")]
        Pad2Warm = 89,

        [Description("Poly_Synth")]
        Pad3Polysynth = 90,

        [Description("Choir")]
        Pad4Choir = 91,

        [Description("Bowed")]
        Pad5Bowed = 92,

        [Description("Metallic")]
        Pad6Metallic = 93,

        [Description("Halo")]
        Pad7Halo = 94,

        [Description("Sweep")]
        Pad8Sweep = 95,

        [Description("Rain")]
        FX1Rain = 96,

        [Description("Soundtrack")]
        FX2Soundtrack = 97,

        [Description("Crystal")]
        FX3Crystal = 98,

        [Description("Atmosphere")]
        FX4Atmosphere = 99,

        [Description("Brightness")]
        FX5Brightness = 100,

        [Description("Goblins")]
        FX6Goblins = 101,

        [Description("Echoes")]
        FX7Echoes = 102,

        [Description("Sci_Fi")]
        FX8SciFi = 103,

        [Description("Sitar")]
        Sitar = 104,

        [Description("Banjo")]
        Banjo = 105,

        [Description("Shamisen")]
        Shamisen = 106,

        [Description("Koto")]
        Koto = 107,

        [Description("Kalimba")]
        Kalimba = 108,

        [Description("Bagpipe")]
        Bagpipe = 109,

        [Description("Fiddle")]
        Fiddle = 110,

        [Description("Shanai")]
        Shanai = 111,

        [Description("Tinkle_Bell")]
        TinkleBell = 112,

        [Description("Agogo")]
        Agogo = 113,

        [Description("Steel_Drums")]
        SteelDrums = 114,

        [Description("Woodblock")]
        Woodblock = 115,

        [Description("Taiko_Drum")]
        TaikoDrum = 116,

        [Description("Melodic_Tom")]
        MelodicTom = 117,

        [Description("Synth_Drum")]
        SynthDrum = 118,

        [Description("Reverse_Cymbal")]
        ReverseCymbal = 119,

        [Description("Guitar_Fret_Noise")]
        GuitarFretNoise = 120,

        [Description("Breath_Noise")]
        BreathNoise = 121,

        [Description("Seashore")]
        Seashore = 122,

        [Description("Bird_Tweet")]
        BirdTweet = 123,

        [Description("Telephone_Ring")]
        TelephoneRing = 124,

        [Description("Helicopter")]
        Helicopter = 125,

        [Description("Applause")]
        Applause = 126,

        [Description("Gunshot")]
        Gunshot = 127
    }
}