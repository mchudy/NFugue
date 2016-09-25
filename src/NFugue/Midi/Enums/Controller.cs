using System.ComponentModel;

namespace NFugue.Midi
{
    public enum Controller
    {
        [Description("BANK_SELECT_COARSE")]
        BankSelectCoarse = 0,

        [Description("MOD_WHEEL_COARSE")]
        ModulationWheelCoarse = 1,
        [Description("BREATH_COARSE")]
        BreathCoarse = 2,
        [Description("FOOT_PEDAL_COARSE")]
        FootPedalCoarse = 4,
        [Description("PORTAMENTO_TIME_COARSE")]
        PortamentoTimeCoarse = 5,
        [Description("DATA_ENTRY_COARSE")]
        DataEntryCoarse = 6,
        [Description("VOLUME_COARSE")]
        VolumeCoarse = 7,
        [Description("BALANCE_COARSE")]
        BalanceCoarse = 8,
        [Description("PAN_POSITION_COARSE")]
        PanPositionCoarse = 10,
        [Description("EXPRESSION_COARSE")]
        ExpressionCoarse = 11,

        [Description("EFFECT_CONTROL_1_COARSE")]
        EffectControl1Coarse = 12,
        [Description("EFFECT_CONTROL_2_COARSE")]
        EffectControl2Coarse = 13,

        [Description("SLIDER_1")]
        Slider1 = 16,
        [Description("SLIDER_2")]
        Slider2 = 17,
        [Description("SLIDER_3")]
        Slider3 = 18,
        [Description("SLIDER_4")]
        Slider4 = 19,

        [Description("BANK_SELECT_FINE")]
        BankSelectFine = 32,
        [Description("MODE_WHEEL_FINE")]
        ModulationWheelFine = 33,
        [Description("BREATH_FINE")]
        BreathFine = 34,
        [Description("FOOT_PEDAL_FINE")]
        FootPedalFine = 36,
        [Description("PORTAMENTO_TIME_FINE")]
        PortamentoTimeFine = 37,
        [Description("DATA_ENTRY_FINE")]
        DataEntryFine = 38,
        [Description("VOLUME_FINE")]
        VolumeFine = 39,
        [Description("BALANCE_FINE")]
        BalanceFine = 40,
        [Description("PAN_POSITION_FINE")]
        PanPositionFine = 42,
        [Description("EXPRESSION_FINE")]
        ExpressionFine = 43,

        [Description("EFFECT_CONTROL_1_FINE")]
        EffectControl1Fine = 44,
        [Description("EFFECT_CONTROL_2_FINE")]
        EffectControl2Fine = 45,

        [Description("HOLD_PEDAL")]
        HoldPedal = 64,
        [Description("PORTAMENTO")]
        Portamento = 65,
        [Description("SOSTENUTO_PEDAL")]
        SustenutoPedal = 66,
        [Description("SOFT_PEDAL")]
        SoftPedal = 67,
        [Description("LEGATO_PEDAL")]
        LegatoPedal = 68,
        [Description("HOLD_2_PEDAL")]
        Hold2Pedal = 69,

        [Description("SOUND_VARIATION")]
        SoundVariation = 70,
        [Description("SOUND_TIMBRE")]
        SoundTimbre = 71,
        [Description("SOUND_RELEASE_TIME")]
        SoundReleaseTime = 72,
        [Description("SOUND_ATTACK_TIME")]
        SoundAttackTime = 73,
        [Description("SOUND_BRIGHTNESS")]
        SoundBrightness = 74,

        [Description("SOUND_CONTROL_6")]
        SoundControl6 = 75,
        [Description("SOUND_CONTROL_7")]
        SoundControl7 = 76,
        [Description("SOUND_CONTROL_8")]
        SoundControl8 = 77,
        [Description("SOUND_CONTROL_9")]
        SoundControl9 = 78,
        [Description("SOUND_CONTROL_10")]
        SoundControl10 = 79,

        [Description("GENERAL_PURPOSE_BUTTON_1")]
        GeneralPurposeButton1 = 80,
        [Description("GENERAL_PURPOSE_BUTTON_2")]
        GeneralPurposeButton2 = 81,
        [Description("GENERAL_PURPOSE_BUTTON_3")]
        GeneralPurposeButton3 = 82,
        [Description("GENERAL_PURPOSE_BUTTON_4")]
        GeneralPurposeButton4 = 83,

        [Description("EFFECTS_LEVEL")]
        EffectsLevel = 91,
        [Description("TREMOLO_LEVEL")]
        TremoloLevel = 92,
        [Description("CHORUS_LEVEL")]
        ChorusLevel = 93,
        [Description("CELESTE_LEVEL")]
        CelesteLevel = 94,
        [Description("PHASER_LEVEL")]
        PhaserLevel = 95,

        [Description("DATA_BUTTON_INCREMENT")]
        DataButtonIncrement = 96,
        [Description("DATA_BUTTON_DECREMENT")]
        DataButtonDecrement = 97,

        [Description("NON_REGISTERED_COARSE")]
        NonRegisteredCoarse = 98,
        [Description("NON_REGISTERED_FINE")]
        NonRegisteredFine = 99,
        [Description("REGISTERED_COARSE")]
        RegisteredCoarse = 100,
        [Description("REGISTERED_FINE")]
        RegisteredFine = 101,

        [Description("ALL_SOUND_OFF")]
        AllSoundOff = 120,
        [Description("ALL_CONTROLLERS_OFF")]
        AllControllersOff = 121,
        [Description("LOCAL_KEYBOARD")]
        LocalKeyboard = 122,
        [Description("ALL_NOTES_OFF")]
        AllNotesOff = 123,
        [Description("OMNI_MODE_OFF")]
        OmniModeOff = 124,
        [Description("OMNI_MODE_ON")]
        OmniModeOn = 125,
        [Description("MONO_OPERATION")]
        MonoOperation = 126,
        [Description("POLY_OPERATION")]
        PolyOperation = 127,

        // Combined Controller names
        // (index = coarse_controller_index * 128 + fine_controller_index)
        [Description("BANK_SELECT")]
        BankSelect = 16383,
        [Description("MOD_WHEEL")]
        ModWheel = 161,
        [Description("BREATH")]
        Breath = 290,
        [Description("FOOT_PEDAL")]
        FootPedal = 548,
        [Description("PORTAMENTO_TIME")]
        PortamentoTime = 677,
        [Description("DATA_ENTRY")]
        DataEntry = 806,
        [Description("VOLUME")]
        Volume = 935,
        [Description("BALANCE")]
        Balance = 1064,
        [Description("PAN_POSITION")]
        PanPosition = 1322,
        [Description("EXPRESSION")]
        Expression = 1451,
        [Description("EFFECT_CONTROL_1")]
        EffectControl1 = 1580,
        [Description("EFFECT_CONTROL_2")]
        EffectControl2 = 1709,
        [Description("NON_REGISTERED")]
        NonRegistered = 12770,
        [Description("REGISTERED")]
        Registered = 13028,

        // values for controllers
        //On = 127,
        //Off = 0,
        // Default = 64

    }
}