namespace MinPlatform.Caching.Service
{
    using MinPlatform.Caching.Service.Models;
    using System;
    using System.Collections.Generic;

    public sealed class CachingManager
    {
        private readonly CachingOption cachingOption;
        private readonly ICachingService cachingService;

        public CachingManager(CachingOption cachingOption, ICachingServiceFactory cachingServiceFactory)
        {
            this.cachingOption = cachingOption;
            this.cachingService = cachingServiceFactory.Create();
        }


        public void Clear()
        {
            cachingService.Clear();
        }

        public bool ContainsKey(string key)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return cachingService.ContainsKey(key);
        }

        public T Get<T>(string key)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return cachingService.Get<T>(key);
        }

        public void Remove(string key)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            cachingService.Remove(key);
        }

        public void Remove(IEnumerable<string> keys)
        {
            if (keys is null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            foreach (var key in keys)
            {
                this.Remove(key);
            }
        }

        public void Set<T>(string key, T value, int? absoluteExpiration = null)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            cachingService.Set(key, value, absoluteExpiration is null ? cachingOption.ExpireTime : absoluteExpiration.Value);
        }

        public void Set(IDictionary<string, object> keyValues, int? absoluteExpiration)
        {
            if (keyValues is null)
            {
                throw new ArgumentNullException(nameof(keyValues));
            }

            cachingService.Set(keyValues, absoluteExpiration is null ? cachingOption.ExpireTime : absoluteExpiration.Value);
        }

        public void Update<T>(string key, T value)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            cachingService.Update(key, value);
        }

        public void Update(IDictionary<string, object> keyValues)
        {
            if (keyValues is null)
            {
                throw new ArgumentNullException(nameof(keyValues));
            }

            cachingService.Update(keyValues);

        }
    }
}
