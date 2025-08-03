namespace MinPlatform.Data.Service.Lookup
{
    using MinPlatform.Data.Abstractions.Models;
    using MinPlatform.Data.Service.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class LookupInfoDataService : ILookupInfoDataService
    {
        private readonly DataManager dataManager;

        public LookupInfoDataService(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public async Task<LookupInfoEntity> GetLookupInfoAsync(string tableName)
        {
            var lookupInfo = await dataManager.SearchEntitiesAsync(new Models.QueryData
            {
                Entity = Constants.LookupInfo,
                Conditions = new List<ConditionGroup>
                {
                    new ConditionGroup
                    {
                        Condition=new List<Condition>
                        {
                            new Condition
                            {
                                Property=Constants.ValidationName,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=tableName
                            }
                        }
                    }
                }
            });

            if (lookupInfo is null || lookupInfo.Count() == 0)
            {
                throw new LookupException("Invalid lookup info for " + tableName);
            }

            IDictionary<string, object> lookup = lookupInfo.First();

            return new LookupInfoEntity
            {
                Id = Convert.ToInt32(lookup[Constants.Id]),
                ColumnName = lookup[Constants.ColumnName]?.ToString(),
                HasDependentColumn = Convert.ToBoolean(lookup[Constants.HasDependentColumn]),
                TableName = lookup[Constants.TableName].ToString(),
                ValidationName = lookup[Constants.ValidationName].ToString(),
            };
        }
    }
}
