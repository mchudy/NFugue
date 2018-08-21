using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFugue.Extensions;
using NFugue.Patterns;
using NFugue.Theory;

namespace NFugue.Staccato.Preprocessors
{
    public class BrokenChordPreprocessor : IPreprocessor
    {
        public string Preprocess(string musicString, StaccatoParserContext context)
        {
            var retVal = new StringBuilder();

            var splitsville = musicString.Split(' ');
            foreach (string s in splitsville)
            {
                int posColon = 0;
                if ((posColon = s.IndexOf(':')) != -1 && (posColon > 0))
                {
                    string candidateChord = s.Substring(0, posColon);

                    // We don't want to think we have a chord when we really have a key signature or time signature, or 
                    // we have a tuplet (indicated by the asterisk)
                    if (Chord.IsValid(candidateChord))
                    {
                        Chord chord = new Chord(candidateChord);

                        // Get the replacement description
                        int posColon2 = s.FindNextOrEnd(':', posColon + 1);
                        string replacementDescription = s.Substring(posColon + 1, posColon2 - posColon - 1);
                        string dynamics = (posColon2 == s.Length ? "" : s.Substring(posColon2 + 1, s.Length - posColon2 - 1));

                        // If the replacement description starts with a bracket, look up the value in the context's dictionary
                        if (replacementDescription[0] == '[')
                        {
                            var replacementLookup = replacementDescription.Substring(1, replacementDescription.Length - 2);
                            replacementDescription = context.Dictionary[replacementLookup] as string;
                        }

                        var specialReplacers = new Dictionary<string, IPatternProducer>
                        {
                            ["ROOT"] = chord.Root,
                            ["BASS"] = chord.GetBassNote(),
                            ["NOTROOT"] = WrapInParens(chord.GetPatternWithNotesExceptRoot())
                        };

                        specialReplacers["NOTBASS"] = WrapInParens(chord.GetPatternWithNotesExceptBass());

                        var result = ReplacementFormatUtil.ReplaceDollarsWithCandidates(
                            replacementDescription, 
                            chord.GetNotes(),
                            WrapInParens(chord.GetPatternWithNotes()), 
                            specialReplacers, 
                            ",", 
                            " ", 
                            dynamics
                        );
                        retVal.Append(result);
                    }
                    else
                    {
                        retVal.Append(s);
                    }
                }
                else
                {
                    retVal.Append(s);
                }
                retVal.Append(" ");
            }
            return retVal.ToString().Trim();
        }

        private Pattern WrapInParens(Pattern s) => new Pattern($"({s})");
    }
}
