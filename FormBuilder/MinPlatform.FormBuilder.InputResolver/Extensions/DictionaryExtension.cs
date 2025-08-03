namespace MinPlatform.FormBuilder.Elements.Extensions
{
    using System.Collections.Generic;

    public static class DictionaryExtension
    {
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> mainDict, IDictionary<TKey, TValue> mergedDict)
        {
            foreach (var dict in mergedDict)
            {
                mainDict[dict.Key] = dict.Value;
            }

            return mainDict;
        }
    }
}
