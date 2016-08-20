﻿using System;
using System.Text;
using System.Text.RegularExpressions;

namespace NFugue.Staccato.Preprocessors
{
    /// <summary>
    /// The MicrotonePreprocess lets a user express a microtone
    /// using 'm' followed by the frequency - e.g., m440. The MicrotonePreprocessor takes this String,
    /// parses the frequency value, figures out what Pitch Wheel and Note events need to be called to
    /// generate this frequency in MIDI, and returns the full set of Staccato Pitch Wheel and Note 
    /// events.
    /// </summary>
    public class MicrotonePreprocessor : IPreprocessor
    {
        private static readonly Regex microtonePattern = new Regex(@"(^\s)[Mm]\S+", RegexOptions.Compiled);
        private static readonly Regex frequencyPattern = new Regex(@"[0-9.]+", RegexOptions.Compiled);
        private static readonly Regex qualifierPattern = new Regex(@"[WHQISTXOADwhqistxoad/]+[0-9.]*\S*");

        public string Preprocess(string musicString, StaccatoParserContext context)
        {
            var sb = new StringBuilder();
            int posPrev = 0;

            foreach (Match match in microtonePattern.Matches(musicString))
            {
                sb.Append(musicString, posPrev, match.Index);

                double frequency = 0.0d;
                Match frequencyMatch = frequencyPattern.Match(match.Groups[0].Value);
                if (frequencyMatch.Success)
                {
                    frequency = double.Parse(frequencyMatch.Groups[0].ToString());
                }
                else
                {
                    throw new ArgumentException("The following is not a valid microtone frequency: " + frequencyMatch.Groups[0].Value);
                }

                string qualifier = null;
                Match qualifierMatch = qualifierPattern.Match(match.Groups[0].Value);
                if (qualifierMatch.Success)
                {
                    qualifier = qualifierMatch.Groups[0].ToString();
                }
                if (qualifier == null)
                {
                    qualifier = $"/{DefaultNoteSettings.DefaultDuration}";
                }
                sb.Append(" ");
                sb.Append(ConvertFrequencyToStaccato(frequency, qualifier));
                posPrev = match.Index + match.Length;
            }
            sb.Append(musicString.Substring(posPrev, musicString.Length - posPrev));
            return sb.ToString().Trim();
        }

        /// <summary>
        /// Converts the given frequency to a music string that involves the Pitch Wheel and notes to 
        /// create the frequency
        /// </summary>
        /// <param name="frequency">the frequency</param>
        /// <param name="qualifier"></param>
        /// <returns>a MusicString that represents the frequency</returns>
        public static string ConvertFrequencyToStaccato(double frequency, string qualifier)
        {
            double totalCents = 1200 * Math.Log(frequency / 16.3515978312876) / Math.Log(2);
            double octave = Math.Round(totalCents / 1200.0);
            double semitoneCents = totalCents - (octave * 1200.0);
            double semitone = Math.Round(semitoneCents / 100.0);
            double microtonalAdjustment = semitoneCents - (semitone * 100.0);
            double pitches = 8192.0 + (microtonalAdjustment * 8192.0 / 100.0);

            double note = ((octave + 1) * 12) + semitone; // This gives a MIDI value, 0 - 128
            if (note > 127) note = 127;

            StringBuilder buddy = new StringBuilder();
            buddy.Append(":PitchWheel(");
            buddy.Append((int)pitches);
            buddy.Append(") ");
            buddy.Append((int)note);
            buddy.Append(qualifier);
            buddy.Append(" :PitchWheel(8192)"); // Reset the pitch wheel.  8192 = original pitch wheel position
            return buddy.ToString();
        }
    }
}