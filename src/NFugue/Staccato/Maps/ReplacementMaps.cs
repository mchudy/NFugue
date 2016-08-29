using System.Collections.Generic;

namespace NFugue.Staccato.Maps
{
    public static class ReplacementMaps
    {
        public static readonly IDictionary<string, string> CarnaticReplacementMap = new Dictionary<string, string>
        {
            {"S", "m261.6256"},
            {"R1", "m275.6220"},
            {"R2", "m279.0673"},
            {"R3", "m290.6951"},
            {"R4", "m294.3288"},
            {"G1", "m310.0747"},
            {"G2", "m313.9507"},
            {"G3", "m327.0319"},
            {"G4", "m331.1198"},
            {"M1", "m348.8341"},
            {"M2", "m353.1945"},
            {"M3", "m367.9109"},
            {"M4", "m372.5098"},
            {"P", "m392.4383"},
            {"D1", "m413.4330"},
            {"D2", "m418.6009"},
            {"D3", "m436.0426"},
            {"D4", "m441.4931"},
            {"N1", "m465.1121"},
            {"N2", "m470.9260"},
            {"N3", "m490.5479"},
            {"N4", "m496.6798"}
        };

        public static readonly IDictionary<string, string> SolfegeReplacementMap = new Dictionary<string, string>
        {
            {"DO", "C" },
            {"RE", "D" },
            {"MI", "E" },
            {"FA", "F" },
            {"SO", "G" },
            {"SOL", "G" },
            {"LA", "A" },
            {"TI", "B" },
            {"TE", "B" }
        };
    }
}