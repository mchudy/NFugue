using System.Collections.Generic;
using System.Linq;

namespace NFugue.Extensions
{
    public static class DictionaryExtensions
    {
        public static IDictionary<TValue, TKey> ReverseDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            return dict.GroupBy(p => p.Value)
                .ToDictionary(g => g.Key, g => g.Select(pp => pp.Key).FirstOrDefault());
        }

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            IDictionary<TKey, TValue> dictToAdd)
        {
            foreach (var entry in dictToAdd)
            {
                dict.Add(entry.Key, entry.Value);
            }
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : default(TValue);
        }
    }
}
