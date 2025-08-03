namespace MinPlatform.ConfigStore.Service
{
    using MinPlatform.Caching.Service;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class ConfigStoreManager : IDisposable
    {
        private readonly IConfigStoreService configStoreService;
        private readonly CachingManager cachingManager;

        public ConfigStoreManager(IConfigStoreService configStoreService, CachingManager cachingManager)
        {
            this.configStoreService = configStoreService;
            this.cachingManager = cachingManager;
        }

        public async Task<IDictionary<string, object>> GetConfigAsync(int Id)
        {
            if (Id <= 0)
            {
                throw new ArgumentNullException(nameof(Id));
            }

            return await configStoreService.GetConfigByIdAsync(Id);
        }

        public async Task GetConfigAsync(int Id, Action<IDictionary<string, object>> configAction)
        {
            IDictionary<string, object> config = await this.GetConfigAsync(Id);

            configAction(config);
        }

        public async Task<IDictionary<string, object>> GetConfigAsync(string configName)
        {
            if (string.IsNullOrEmpty(configName))
            {
                throw new ArgumentNullException(nameof(configName));
            }

            var config = cachingManager.Get<IDictionary<string, object>>("config_" + configName);

            if (config is null)
            {
                config = await configStoreService.GetConfigByNameAsync(configName);

                if (config != null)
                {
                    cachingManager.Set("config_" + configName, config, null);

                }

            }

            return config;
        }

        public async Task GetConfigAsync(string configName, Action<IDictionary<string, object>> configAction)
        {
            if (string.IsNullOrEmpty(configName))
            {
                throw new ArgumentNullException(nameof(configName));
            }

            var config = cachingManager.Get<IDictionary<string, object>>("config_" + configName);

            if (config is null)
            {
                config = await configStoreService.GetConfigByNameAsync(configName);

                cachingManager.Set("config_" + configName, config, null);

            }

            configAction(config);

        }

        public void Dispose()
        {
            if (this is IDisposable disposable)
            {
                GC.SuppressFinalize(this);
            }
        }


    }
}
