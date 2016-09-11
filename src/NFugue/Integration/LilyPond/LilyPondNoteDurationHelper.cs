namespace NFugue.Integration.LilyPond
{
    internal static class LilyPondNoteDurationHelper
    {
        public static string GetDuration(string duration)
        {
            string durationLy = "4";
            double durationVal = double.Parse(duration);
            if (durationVal == 0.0625)
            {
                durationLy = "16";
            }
            else if (durationVal == 0.125)
            {
                durationLy = "8";
            }
            else if (durationVal == 0.25)
            {
                durationLy = "4";
            }
            else if (durationVal == 0.375)
            {
                durationLy = "4.";
            }
            else if (durationVal == 0.5)
            {
                durationLy = "2";
            }
            else if (durationVal == 0.75)
            {
                durationLy = "2.";
            }
            else if (durationVal == 1.0)
            {
                durationLy = "1";
            }
            else if (durationVal == 2.0)
            {
                durationLy = "\\breve";
            }
            else if (durationVal == 3.0)
            {
                durationLy = "\\breve.";
            }
            else if (durationVal == 4.0)
            {
                durationLy = "\\longa";
            }
            else
            {
                durationLy = "4";
            }
            return durationLy;
        }
    }
}