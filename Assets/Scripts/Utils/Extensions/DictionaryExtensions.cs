using System.Collections.Generic;

namespace Utils.Extensions {
    public static class DictionaryExtensions {
        public static void Merge<TKey, TValue>(this Dictionary<TKey, TValue> me, Dictionary<TKey, TValue> other) {
            foreach (var item in other) {
                me[item.Key] = item.Value;
            }
        }
        
        public static Dictionary<TKey, TValue> MergeIntoNew<TKey, TValue>(this Dictionary<TKey, TValue> dict1, Dictionary<TKey, TValue> dict2, bool overwriteDuplicates = true) {
            var maxCapacity = dict1.Count + dict2.Count;
            var result = new Dictionary<TKey, TValue>(maxCapacity, dict1.Comparer);

            foreach (var kvp in dict1) {
                result[kvp.Key] = kvp.Value;
            }

            foreach (var kvp in dict2) {
                if (overwriteDuplicates) {
                    result[kvp.Key] = kvp.Value;
                }
                else {
                    result.TryAdd(kvp.Key, kvp.Value);
                }
            }

            return result;
        }
    }
}
