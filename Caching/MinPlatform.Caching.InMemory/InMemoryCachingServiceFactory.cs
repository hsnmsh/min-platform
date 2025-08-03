namespace MinPlatform.Caching.InMemory
{
    using Microsoft.Extensions.Caching.Memory;
    using MinPlatform.Caching.Service;

    public sealed class InMemoryCachingServiceFactory : ICachingServiceFactory
    {
        public ICachingService Create()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            return new InMemoryCachingService(memoryCache);
        }
    }
}
