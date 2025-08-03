namespace MinPlatform.Caching.InMemory
{
    using Microsoft.Extensions.Caching.Memory;
    using MinPlatform.Caching.Service;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public sealed class InMemoryCachingService : ICachingService
    {
        private readonly IMemoryCache memoryCache;

        public InMemoryCachingService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }
        public void Clear()
        {
            var field = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);
            PropertyInfo cacheEntriesCollectionDefinition = field?.FieldType.GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            dynamic collection = cacheEntriesCollectionDefinition.GetValue(field.GetValue(memoryCache)); 

            if (collection != null)
            {
                foreach (var item in collection)
                {
                    var methodInfo = item.GetType().GetProperty("Key");
                    var key = methodInfo.GetValue(item);

                    memoryCache.Remove(key);
                }
            }
                
        }

        public bool ContainsKey(string key)
        {
            return memoryCache.TryGetValue(key, out _);
        }

        public T Get<T>(string key)
        {
            if (memoryCache.TryGetValue(key, out T cachedValue))
            {
                return cachedValue;
            }

            return default;
        }

        public void Remove(string key)
        {
            memoryCache.Remove(key);
        }

        public void Remove(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                memoryCache.Remove(key);
            }
        }

        public void Set<T>(string key, T value, int absoluteExpiration)
        {
            memoryCache.Set(key, value, DateTimeOffset.UtcNow.AddMinutes(absoluteExpiration));
        }

        public void Set(IDictionary<string, object> keyValues, int absoluteExpiration)
        {
            foreach (var keyValue in keyValues)
            {
                memoryCache.Set(keyValue.Key, keyValue.Value, DateTimeOffset.UtcNow.AddMinutes(absoluteExpiration));
            }
        }

        public void Update<T>(string key, T value)
        {
            if (memoryCache.TryGetValue(key, out object existingValue))
            {
                memoryCache.Set(key, value);
            }
        }

        public void Update(IDictionary<string, object> keyValues)
        {
            foreach (var keyValue in keyValues)
            {
                Update(keyValue.Key, keyValue.Value);
            }
        }
    }
}
