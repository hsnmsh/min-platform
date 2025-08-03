namespace MinPlatform.Data.Service.Lookup
{
    using MinPlatform.Data.Service.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal sealed class LookupDataService : ILookupDataService
    {
        private readonly DataManager dataManager;

        public LookupDataService(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public async Task<IDictionary<string, IEnumerable<LookupEntity>>> GetLookupsAsync(string tableName, bool hasDependentColumn, string column, string columnValue = null)
        {
            var dataQuery = new QueryData()
            {
                Entity = tableName
            };

            if (hasDependentColumn)
            {
                dataQuery.Conditions = new List<ConditionGroup>
                {
                    new ConditionGroup
                    {
                        Condition=new List<Condition>
                        {
                            new Condition
                            {
                                Property=column,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=columnValue
                            }
                        }

                    }
                };
            }

            IEnumerable<IDictionary<string, object>> lookups = await dataManager.SearchEntitiesAsync(dataQuery);

            return lookups
                .Select(lookup => ToLookupEntity(lookup))
                .GroupBy(lookup => lookup.LanguageCode)
                .ToDictionary(l => l.Key, l => l.Select(entity => entity).AsEnumerable());
        }

        public async Task<IEnumerable<LookupEntity>> GetLookupsByLanguageCodeAsync(string tableName, 
            bool hasDependentColumn, 
            string column, 
            string languageCode, 
            string columnValue = null)
        {
            var dataQuery = new QueryData()
            {
                Entity = tableName,
                Conditions = new List<ConditionGroup>
                {
                    new ConditionGroup
                    {
                        Condition=new List<Condition>
                        {
                            new Condition
                            {
                                Property=Constants.LanguageCode,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=languageCode
                            }
                        }
                    }
                }
            };

            if (hasDependentColumn)
            {
                dataQuery.Conditions.Add(new ConditionGroup
                {
                    Condition = new List<Condition>
                    {
                        new Condition
                        {
                            Property=column,
                            ConditionalOperator=ConditionalOperator.Equal,
                            Value=columnValue
                        }
                    },
                });

                dataQuery.LogicalOperator = LogicalOperator.And;
            }

            IEnumerable<IDictionary<string, object>> lookups = await dataManager.SearchEntitiesAsync(dataQuery);

            return lookups.Select(lookup => ToLookupEntity(lookup));
             
        }

        private static LookupEntity ToLookupEntity(IDictionary<string, object> lookupValue)
        {
            return new LookupEntity
            {
                Id = lookupValue[Constants.Id].ToString(),
                Code = lookupValue[Constants.Code].ToString(),
                LanguageCode = lookupValue[Constants.LanguageCode].ToString(),
                Title = lookupValue[Constants.Title].ToString(),
            };
        }
    }
}
