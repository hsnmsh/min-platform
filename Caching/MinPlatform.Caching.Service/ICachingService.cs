namespace MinPlatform.Caching.Service
{
    using System.Collections.Generic;

    public interface ICachingService
    {
        /// <summary>
        /// Get the cached value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the cached value.</typeparam>
        /// <param name="key">The unique key for the cached value.</param>
        /// <returns>The cached value or default(T) if the key is not found.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Set a value in the cache with the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value to be cached.</typeparam>
        /// <param name="key">The unique key for the cached value.</param>
        /// <param name="value">The value to be cached.</param>
        /// <param name="absoluteExpiration">The absolute expiration time for the cached value.</param>
        void Set<T>(string key, T value, int absoluteExpiration);

        /// <summary>
        /// Set values in the cache with the specified keys.
        /// </summary>
        /// <typeparam name="T">The type of the values to be cached.</typeparam>
        /// <param name="keyValues">A dictionary of key-value pairs to be cached.</param>
        /// <param name="absoluteExpiration">The absolute expiration time for the cached values.</param>
        void Set(IDictionary<string, object> keyValues, int absoluteExpiration);

        /// <summary>
        /// Update the value in the cache associated with the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value to be updated.</typeparam>
        /// <param name="key">The unique key for the cached value.</param>
        /// <param name="value">The new value to update the cached entry.</param>
        void Update<T>(string key, T value);

        /// <summary>
        /// Update values in the cache associated with the specified keys.
        /// </summary>
        /// <typeparam name="T">The type of the values to be updated.</typeparam>
        /// <param name="keyValues">A dictionary of key-value pairs to be updated in the cache.</param>
        void Update(IDictionary<string, object> keyValues);

        /// <summary>
        /// Remove the cached value associated with the specified key.
        /// </summary>
        /// <param name="key">The unique key for the cached value.</param>
        void Remove(string key);

        /// <summary>
        /// Remove values from the cache associated with the specified keys.
        /// </summary>
        /// <param name="keys">The collection of keys to be removed from the cache.</param>
        void Remove(IEnumerable<string> keys);

        /// <summary>
        /// Check if the cache contains the specified key.
        /// </summary>
        /// <param name="key">The unique key to check for existence in the cache.</param>
        /// <returns>True if the key is found in the cache; otherwise, false.</returns>
        bool ContainsKey(string key);

        /// <summary>
        /// Clear all entries from the cache.
        /// </summary>
        void Clear();
    }

}
