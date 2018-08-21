using NFugue.Extensions;
using NFugue.Midi;
using NFugue.Patterns;
using NFugue.Staccato.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFugue.Rhythms
{
    public class Rhythm : IPatternProducer
    {
        public static readonly IDictionary<char, string> DefaultRhythmKit = new Dictionary<char, string>() {
                {'.', "Ri"},
                {'O', "[BASS_DRUM]i"},
                {'o', "Rs [BASS_DRUM]s"},
                {'S', "[ACOUSTIC_SNARE]i"},
                {'s', "Rs [ACOUSTIC_SNARE]s"},
                {'^', "[PEDAL_HI_HAT]i"},
                {'`', "[PEDAL_HI_HAT]s Rs"},
                {'*', "[CRASH_CYMBAL_1]i"},
                {'+', "[CRASH_CYMBAL_1]s Rs"},
                {'X', "[HAND_CLAP]i"},
                {'x', "Rs [HAND_CLAP]s"},
                {' ', "Ri"}
        };

        private IList<string> layers = new List<string>();
        private IDictionary<int, List<AltLayer>> altLayers = new Dictionary<int, List<AltLayer>>();

        public Rhythm()
        {
        }

        public Rhythm(params string[] layers)
        {
            foreach (string layer in layers)
            {
                AddLayer(layer);
            }
        }

        /// <summary>
        /// Length of the rhythm i.e. the number of times that a single pattern is repeated.
        /// For example, creating a layer of "S...S...S...O..." and a length of 3 would result 
        /// in a Rhythm pattern of "S...S...S...O...S...S...S...O...S...S...S...O..."
        /// </summary>
        public int Length { get; set; } = 1;

        /// <summary>
        /// All layers that have been added with the traditional AddLayer() method - but to
        /// truly find out what the layer will sound like at a given segment, use GetLayersForSegment(),
        /// which takes alt layers into account.
        /// </summary>
        public IList<string> Layers
        {
            get { return layers; }
            set
            {
                if (value.Count > MidiDefaults.Layers)
                {
                    throw new ArgumentException($"Size of the list of layers is greater than {MidiDefaults.Layers}");
                }
                layers = value;
            }
        }

        public IDictionary<char, string> RhythmKit { get; set; } = DefaultRhythmKit;

        public Pattern GetPattern()
        {
            Pattern fullPattern = new Pattern();
            for (int segment = 0; segment < Length; segment++)
            {
                fullPattern.Add(GetPatternAt(segment));
            }
            return fullPattern;
        }

        public string GetLayer(int layer) => Layers[layer];

        /// <summary>
        /// Returns all the layers, including altlayers for the given segment
        /// <seealso cref="Layers"/>
        /// </summary>
        public IList<string> GetLayersAt(int segment)
        {
            List<string> retVal = new List<string>(layers.Count);
            for (int i = 0; i < layers.Count; i++)
            {
                retVal.Add(null);
            }
            for (int layer = 0; layer < layers.Count; layer++)
            {
                List<AltLayer> altLayersForLayer = GetSortedAltLayersForLayer(layer);
                // Start with the base layer
                retVal[layer] = GetLayer(layer);

                // See if the base layer should be replaced by any of the alt layers
                foreach (AltLayer altLayer in altLayersForLayer)
                {
                    if (altLayer.ShouldProvideAltLayer(segment))
                    {
                        // Remember that RhythmAltLayerProvider is allowed to return null if there is nothing to add
                        string rhythmOrNull = altLayer.GetAltLayer(segment);
                        if (rhythmOrNull != null)
                        {
                            retVal[layer] = rhythmOrNull;
                        }
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// Returns all AltLayers for the given layer; the resulting list is unsorted by z-order
        /// </summary>
        public List<AltLayer> GetAltLayersForLayer(int layer)
        {
            List<AltLayer> value;
            if (!altLayers.TryGetValue(layer, out value))
            {
                altLayers[layer] = new List<AltLayer>();
                return altLayers[layer];
            }
            return value;
        }

        /// <summary>
        /// Returns all AltLayers for the given layer sorted by each AltLayer's z-order
        /// </summary>
        public List<AltLayer> GetSortedAltLayersForLayer(int layer)
        {
            return GetAltLayersForLayer(layer)
                .OrderBy(a => a.ZOrder)
                .ToList();
        }

        /// <summary>
        /// Adds a layer to this Rhythm, if the rhythm already has MaxLayers layers does nothing.
        /// </summary>
        public Rhythm AddLayer(string layer)
        {
            if (Layers.Count < MidiDefaults.Layers)
            {
                Layers.Add(layer);
            }
            return this;
        }

        public bool CanAddLayer => Layers.Count < MidiDefaults.Layers;

        /// <summary>
        /// Sets an alt layer that will recur every recurrence times *after* the start index is reached.
        /// If the start index is 2 and the recurrence is 5, this alt layer will be used every time 
        /// the segment % recurrence == start. By default, this has a Z-Order of 1.
        /// </summary>
        public Rhythm AddRecurringAltLayer(int layer, int start, int end, int recurrence, string rhythmString)
        {
            return AddRecurringAltLayer(layer, start, end, recurrence, rhythmString, 1);
        }

        ///  <summary>
        /// Sets an alt layer that will recur every recurrence times *after* the start index is reached.
        /// If the start index is 2 and the recurrence is 5, this alt layer will be used every time 
        /// the segment % recurrence == start
        /// </summary>
        public Rhythm AddRecurringAltLayer(int layer, int start, int end, int recurrence, string rhythmString, int zOrder)
        {
            GetAltLayersForLayer(layer).Add(new AltLayer(start, end, recurrence, rhythmString, null, zOrder));
            return this;
        }

        /// <summary>
        /// Sets an alt layer that will play between and including the start and end indices. 
        /// By default, this has a Z-Order of 2.
        /// </summary>
        public Rhythm AddRangedAltLayer(int layer, int start, int end, string rhythmString)
        {
            return AddRangedAltLayer(layer, start, end, rhythmString, 2);
        }

        /// <summary>
        /// Sets an alt layer that will play between and including the start and end indices. 
        /// </summary>
        public Rhythm AddRangedAltLayer(int layer, int start, int end, string rhythmString, int zOrder)
        {
            GetAltLayersForLayer(layer).Add(new AltLayer(start, end, -1, rhythmString, null, zOrder));
            return this;
        }

        /// <summary>
        /// Sets an alt layer that will play one time, at the given segment.
        /// By default, this has a Z-Order of 3.
        /// </summary>
        public Rhythm AddOneTimeAltLayer(int layer, int oneTime, string rhythmString)
        {
            return AddOneTimeAltLayer(layer, oneTime, rhythmString, 3);
        }

        /// <summary>
        /// Sets an alt layer that will play one time, at the given segment.
        /// </summary>
        public Rhythm AddOneTimeAltLayer(int layer, int oneTime, string rhythmString, int zOrder)
        {
            GetAltLayersForLayer(layer).Add(new AltLayer(oneTime, oneTime, -1, rhythmString, null, zOrder));
            return this;
        }

        /// <summary>
        /// Gives a RhythmAltLayerProvider, which will make its own determination about what type of 
        /// alt layer to play, and when to play it.
        /// By default, this has a Z-Order of 4.
        /// </summary>
        public Rhythm AddAltLayerProvider(int layer, AltLayer.RhythmAltLayerProvider altLayerProvider)
        {
            return AddAltLayerProvider(layer, altLayerProvider, 4);
        }

        /// <summary>
        /// Gives a RhythmAltLayerProvider, which will make its own determination about what type of 
        /// alt layer to play, and when to play it.
        /// </summary>
        public Rhythm AddAltLayerProvider(int layer, AltLayer.RhythmAltLayerProvider altLayerProvider, int zOrder)
        {
            GetAltLayersForLayer(layer).Add(new AltLayer(0, Length, -1, null, altLayerProvider, zOrder));
            return this;
        }

        public Pattern GetPatternAt(int segment)
        {
            var pattern = new Pattern(StaccatoElementsFactory.CreateTrackElement(9));
            int layerCounter = 0;
            foreach (string layer in GetLayersAt(segment))
            {
                pattern.Add(StaccatoElementsFactory.CreateLayerElement(layerCounter));
                layerCounter++;
                pattern.Add(GetStaccatoStringForRhythm(layer));
            }
            return pattern;
        }

        public string GetStaccatoStringForRhythm(string rhythm)
        {
            var sb = new StringBuilder();
            foreach (char ch in rhythm)
            {
                string val;
                if (RhythmKit.TryGetValue(ch, out val))
                {
                    sb.Append(val);
                    sb.Append(" ");
                }
                else
                {
                    throw new ApplicationException("The character '" + ch + "' used in the rhythm layer \"" + rhythm + "\" is not associated with a Staccato music string in the RhythmKit " + RhythmKit);
                }
            }
            return sb.ToString().Trim();
        }

        /// <summary>
        /// Returns the full rhythm, including alt layers, but not translated into Staccato 
        /// music strings by looking up rhythm entries into the RhythmKit
        /// </summary>
        public IEnumerable<string> GetRhythm()
        {
            // Create the full rhythm for each layer and each segment
            StringBuilder[] builders = new StringBuilder[layers.Count];
            for (int i = 0; i < layers.Count; i++)
            {
                builders[i] = new StringBuilder();
                for (int segment = 0; segment < Length; segment++)
                {
                    builders[i].Append(GetLayersAt(segment)[i]);
                }
            }

            // Get strings from the builders
            string[] retVal = new string[layers.Count];
            for (int i = 0; i < layers.Count; i++)
            {
                retVal[i] = builders[i].ToString();
            }

            return retVal;
        }

        /// <summary>
        /// Combines rhythms into multiple layers. If there are
        /// more than MAX_LAYERS layers in the provided rhythms, 
        /// only the first MAX_LAYERS are used (for example, if you
        /// pass five rhythms that each have four layers, the combined
        /// rhythm will only contain the layers from the first four rhythms).
        /// This method also ensures that the Rhythm Kit for each of the
        /// provided Rhythms is added to the return value's Rhythm Kit.
        /// </summary>
        /// <param name="rhythms">the rhythms to combine</param>
        /// <returns>the combined rhythm</returns>
        public static Rhythm Combine(params Rhythm[] rhythms)
        {
            Rhythm retVal = new Rhythm();
            foreach (var rhythm in rhythms)
            {
                // Add the rhythm's Rhythm Kit to the return value's rhythm kit 
                retVal.RhythmKit.AddRange(rhythm.RhythmKit);

                // Add the rhythm data
                foreach (string layer in rhythm.Layers)
                {
                    if (retVal.CanAddLayer)
                    {
                        retVal.AddLayer(layer);
                    }
                    else
                    {
                        return retVal;
                    }
                }

                // Add the alt layer into
                foreach (int key in rhythm.altLayers.Keys)
                {
                    retVal.GetAltLayersForLayer(key).AddRange(rhythm.GetAltLayersForLayer(key));
                }

                // Figure out the length of the new rhythm
                if (retVal.Length < rhythm.Length)
                {
                    retVal.Length = rhythm.Length;
                }
            }

            return retVal;
        }
    }
}