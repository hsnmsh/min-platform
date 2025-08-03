namespace MinPlatform.Data.Service.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    internal static class DictionaryExtension
    {
        public static IDictionary<string, object> Merge(this IDictionary<string, object> dictionary, IDictionary<string, object> other)
        {
            return dictionary.Union(other)
                          .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
