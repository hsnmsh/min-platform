namespace MinPlatform.FormBuilder.Engine.DataService
{
    using MinPlatform.Data.Service;
    using MinPlatform.Data.Service.Models;
    using MinPlatform.FormBuilder.Elements.Inputs;
    using MinPlatform.FormBuilder.Elements.Sections;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;

    internal sealed class FormInfoDataService : IFormInfoDataService
    {
        private const string formTableName = "MinPlatform.FormInfo";
        private const string nameColumn = "Name";
        private const string LanguageCodeColumn = "LanguageCode";

        private readonly DataManager dataManager;

        public FormInfoDataService(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public async Task<FormEntity> GetFormAsync(string name, string languageCode)
        {
            IEnumerable<IDictionary<string, object>> formEntities = await dataManager.SearchEntitiesAsync(new Data.Service.Models.QueryData
            {
                Entity = formTableName,
                Conditions = new List<ConditionGroup>
                {
                    new ConditionGroup
                    {
                        Condition=new List<Condition>
                        {
                            new Condition
                            {
                                Property=nameColumn,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=name
                            },
                            new Condition
                            {
                                Property=LanguageCodeColumn,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=languageCode
                            }
                        },
                        Operator=LogicalOperator.And
                    }
                }
            });

            if (formEntities is null || !formEntities.Any())
            {
                throw new FormEngineException($"Form info with name {name} and {languageCode} are not found");
            }

            return ToFormInfo(formEntities.First());
        }

        public async Task<IEnumerable<FormEntity>> GetFormsAsync(string name)
        {
            IEnumerable<IDictionary<string, object>> formEntities = await dataManager.SearchEntitiesAsync(new Data.Service.Models.QueryData
            {
                Entity = formTableName,
                Conditions = new List<ConditionGroup>
                {
                    new ConditionGroup
                    {
                        Condition=new List<Condition>
                        {
                            new Condition
                            {
                                Property=nameColumn,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=name
                            }
                        },
                    }
                }
            });

            if (formEntities is null || !formEntities.Any())
            {
                throw new FormEngineException($"Form info with name {name}  are not found");
            }

            return formEntities.Select(form => ToFormInfo(form));
        }

        private static FormEntity ToFormInfo(IDictionary<string, object> formEntity)
        {
            var properties = JsonConvert.DeserializeObject<JObject>(formEntity["Properties"].ToString());

            var settings = new JsonSerializerSettings
            {
                Converters = { new BaseJsonConverter(), new StringEnumConverter() },
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };

            DateTime? createdOn = null;
            DateTime? modifiedOn = null;

            if (DateTime.TryParse(formEntity["CreatedOn"]?.ToString(), out DateTime v))
            {
                createdOn = v;
            }

            if (DateTime.TryParse(formEntity["ModifiedOn"]?.ToString(), out DateTime m))
            {
                modifiedOn = m;
            }

            return new FormEntity
            {
                Id = Convert.ToInt32(formEntity["Id"]),
                Name = formEntity["Name"].ToString(),
                LanguageText = formEntity["LanguageText"]?.ToString(),
                LanguageCode = formEntity["LanguageCode"].ToString(),
                AllowedRoles = properties.Value<JArray>("AllowedRoles").ToObject<IEnumerable<string>>(),
                CreatedBy = formEntity["CreatedBy"]?.ToString(),
                CreatedOn = createdOn,
                Description = formEntity["Description"]?.ToString(),
                ModifiedBy = formEntity["ModifiedBy"]?.ToString(),
                ModifiedOn = modifiedOn,
                ServiceCode = formEntity["ServiceCode"].ToString(),
                TargetClient = formEntity["TargetClient"]?.ToString(),
                Title = formEntity["Title"].ToString(),
                InputProperties = properties.Value<JObject>("InputProperties")?.ToObject<IDictionary<string, object>>(),
                FormGroups = properties.Value<JArray>("FormGroups").ToObject<IEnumerable<FormGroupPropertySection>>(JsonSerializer.CreateDefault(settings))
            };



        }
    }
}
