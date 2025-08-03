namespace MinPlatform.ConfigStore.Service
{
    using MinPlatform.Data.Service;
    using MinPlatform.Data.Service.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class ConfigStoreService : IConfigStoreService
    {

        private readonly DataManager dataManager;

        public ConfigStoreService(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public async Task<IDictionary<string, object>> GetConfigByIdAsync(int Id)
        {
            return await dataManager.GetEntityByIdAsync(Id, Constants.ConfigurationTableName);

        }

        public async Task<IDictionary<string, object>> GetConfigByNameAsync(string name)
        {
            IEnumerable<IDictionary<string, object>> configs = await dataManager.SearchEntitiesAsync(new QueryData
            {
                SingleRow = true,
                IsDistinct = true,
                Conditions = new List<ConditionGroup>
                {
                    new ConditionGroup
                    {
                         Condition=new List<Condition>
                         {
                             new Condition
                             {
                                 Property=Constants.Name,
                                 Value=name,
                                 ConditionalOperator=ConditionalOperator.Equal
                             }
                         }
                    }
                },
                Entity = Constants.ConfigurationTableName
            });

            return configs.FirstOrDefault();
        }
    }
}
