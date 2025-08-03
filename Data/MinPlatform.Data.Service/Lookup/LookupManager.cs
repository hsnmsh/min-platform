namespace MinPlatform.Data.Service.Lookup
{
    using MinPlatform.Caching.Service;
    using MinPlatform.Data.Abstractions.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class LookupManager
    {
        private readonly ILookupDataService lookupDataService;
        private readonly ILookupInfoDataService lookupInfoDataService;
        private readonly CachingManager cachingManager;

        public LookupManager(ILookupDataService lookupDataService, ILookupInfoDataService lookupInfoDataService, CachingManager cachingManager)
        {
            this.lookupDataService = lookupDataService;
            this.lookupInfoDataService = lookupInfoDataService;
            this.cachingManager = cachingManager;
        }

        public async Task<IDictionary<string, IEnumerable<LookupEntity>>> GetLookupsAsync(string tableName, string columnValue = null)
        {
            if (tableName is null)
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            LookupInfoEntity lookupInfo = cachingManager.Get<LookupInfoEntity>(tableName + "_lookupInfo");

            if (lookupInfo is null)
            {
                lookupInfo = await lookupInfoDataService.GetLookupInfoAsync(tableName);

                cachingManager.Set(tableName + "_lookupInfo", lookupInfo, null);

            }

            IDictionary<string, IEnumerable<LookupEntity>> lookups = cachingManager.Get<IDictionary<string, IEnumerable<LookupEntity>>>(tableName + "_lookup");

            if (lookups is null)
            {
                lookups = await lookupDataService.GetLookupsAsync(tableName,
                    lookupInfo.HasDependentColumn,
                    lookupInfo.ColumnName, columnValue);

                if (!lookupInfo.HasDependentColumn)
                {
                    cachingManager.Set(tableName + "_lookup", lookups, null);
                }

            }

            return lookups;
        }

        public async Task<IEnumerable<LookupEntity>> GetLookupsByLanguageAsync(string tableName, string languageCode, string columnValue = null)
        {
            if (tableName is null)
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            LookupInfoEntity lookupInfo = cachingManager.Get<LookupInfoEntity>(tableName + "_lookupInfo");

            if (lookupInfo is null)
            {
                lookupInfo = await lookupInfoDataService.GetLookupInfoAsync(tableName);

                cachingManager.Set(tableName + "_lookupInfo", lookupInfo, null);

            }

            IEnumerable<LookupEntity> lookups = cachingManager.Get<IEnumerable<LookupEntity>>(tableName + "_lookup_" + languageCode);

            if (lookups is null)
            {
                lookups = await lookupDataService.GetLookupsByLanguageCodeAsync(tableName,
                 lookupInfo.HasDependentColumn,
                 lookupInfo.ColumnName,
                 languageCode,
                 columnValue);

                if (!lookupInfo.HasDependentColumn)
                {
                    cachingManager.Set(tableName + "_lookup_" + languageCode, lookups, null);
                }

            }

            return lookups;
        }
    }
}
