namespace MinPlatform.Tenant.Service.TenantConfigStore
{
    using MinPlatform.Data.Service;
    using MinPlatform.Data.Service.Models;
    using MinPlatform.Tenant.Service.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    
    public sealed class TenantConfigInfoDataService : ITenantConfigInfoDataService
    {
        private readonly DataManager dataManager;

        public TenantConfigInfoDataService(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public async Task<IList<TenantConfig>> GetTenantConfigByTenantIdAsync(string tenantId)
        {
            IEnumerable<IDictionary<string, object>> tenantConnectionInfoList = await dataManager.SearchEntitiesAsync(new QueryData
            {
                Entity = Constants.TenantConfig,
                IsDistinct = true,
                Conditions = new List<ConditionGroup>
                {
                    new ConditionGroup
                    {
                        Condition=new List<Condition>
                        {
                            new Condition
                            {
                                Property=Constants.TenantId,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=tenantId
                            }
                        },
                    }
                },

            });

            if (tenantConnectionInfoList is null || !tenantConnectionInfoList.Any())
            {
                throw new InavlidTenantConfigException("Tenant connection Info is not found");
            }

            return tenantConnectionInfoList.Select(siteConfigEntity => ToConnectionInfo(siteConfigEntity)).ToList();

        }

        public async Task<TenantConfig> GetTenantConfigByTenantIdAndNameAsync(string TenantId, string connectionName)
        {
            IEnumerable<IDictionary<string, object>> tenantConnectionInfoList = await dataManager.SearchEntitiesAsync(new QueryData
            {
                SingleRow = true,
                Entity = Constants.TenantConfig,
                IsDistinct = true,
                Conditions = new List<ConditionGroup>
                {
                    new ConditionGroup
                    {
                        Condition=new List<Condition>
                        {
                            new Condition
                            {
                                Property=Constants.TenantId,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=TenantId
                            },
                            new Condition
                            {
                                Property=Constants.Name,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=connectionName
                            }
                        },

                        Operator=LogicalOperator.And
                    }
                },

            });

            if (tenantConnectionInfoList is null || !tenantConnectionInfoList.Any())
            {
                throw new InavlidTenantConfigException("Tenant connection Info is not found");
            }

            IDictionary<string, object> siteConfig = tenantConnectionInfoList.First();

            return ToConnectionInfo(siteConfig);
        }

        private static TenantConfig ToConnectionInfo(IDictionary<string, object> connectionInfoEntity)
        {
            return new TenantConfig
            {
                TenantId = connectionInfoEntity["TenantId"]?.ToString(),

                Properties = connectionInfoEntity["Properties"]?.ToString() != null ?
                    JsonConvert.DeserializeObject<IDictionary<string, object>>(connectionInfoEntity["Properties"]?.ToString()) :
                    null,

                Name = connectionInfoEntity["Name"]?.ToString(),

            };
        }


    }
}
