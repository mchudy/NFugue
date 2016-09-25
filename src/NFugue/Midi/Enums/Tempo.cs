using System.ComponentModel;

namespace NFugue.Midi
{
    public enum Tempo
    {
        [Description("GRAVE")]
        Grave = 40,

        [Description("LARGO")]
        Largo = 45,

        [Description("LARGHETTO")]
        Larghetto = 50,

        [Description("LENTO")]
        Lento = 55,

        [Description("ADAGIO")]
        Adagio = 60,

        [Description("ADAGIETTO")]
        Adagietto = 65,

        [Description("ANDANTE")]
        Andante = 70,

        [Description("ANDANTINO")]
        Andantino = 80,

        [Description("MODERATO")]
        Moderato = 95,

        [Description("ALLEGRETTO")]
        Allegretto = 110,

        [Description("ALLEGRO")]
        Allegro = 120,

        [Description("VIVACE")]
        Vivace = 145,

        [Description("PRESTO")]
        Presto = 180,

        [Description("PRESTISSIMO")]
        Prestissmo = 220
    }
}