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
    }
}
