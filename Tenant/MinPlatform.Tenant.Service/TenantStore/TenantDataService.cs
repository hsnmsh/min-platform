namespace MinPlatform.Tenant.Service.TenantStore
{
    using MinPlatform.Data.Service;
    using MinPlatform.Data.Service.Models;
    using MinPlatform.Tenant.Service.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class TenantDataService : ITenantDataService
    {
        private readonly DataManager dataManager;

        public TenantDataService(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public async Task<SiteConfig> GetSiteConfigByDomainAndLanguageCodeAsync(string domain, string languageCode)
        {
            IEnumerable<IDictionary<string, object>> siteConfigInfoList = await dataManager.SearchEntitiesAsync(new QueryData
            {
                SingleRow = true,
                Entity = Constants.SiteConfig,
                IsDistinct = true,
                Conditions = new List<ConditionGroup>
                {
                    new ConditionGroup
                    {
                        Condition=new List<Condition>
                        {
                            new Condition
                            {
                                Property=Constants.OfficalEntityDomain,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=domain
                            },
                            new Condition
                            {
                                Property=Constants.LanguageCode,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=languageCode
                            }
                        },
                        Operator=LogicalOperator.And
                    }
                },

            });

            if (siteConfigInfoList is null || !siteConfigInfoList.Any())
            {
                throw new InavlidTenantException("Tenant is not found");
            }

            IDictionary<string, object> siteConfig = siteConfigInfoList.First();

            return ToSiteConfig(siteConfig);

        }

        public async Task<IList<SiteConfig>> GetSiteConfigByDomainAsync(string domain)
        {
            IEnumerable<IDictionary<string, object>> siteConfigInfoList = await dataManager.SearchEntitiesAsync(new QueryData
            {
                Entity = Constants.SiteConfig,
                IsDistinct = true,
                Conditions = new List<ConditionGroup>
                {
                    new ConditionGroup
                    {
                        Condition=new List<Condition>
                        {
                            new Condition
                            {
                                Property=Constants.OfficalEntityDomain,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=domain
                            }
                        },
                    }
                },

            });

            if (siteConfigInfoList is null || !siteConfigInfoList.Any())
            {
                throw new InavlidTenantException("Tenant is not found");
            }

            return siteConfigInfoList.Select(siteConfigEntity => ToSiteConfig(siteConfigEntity)).ToList();
        }

        public async Task<SiteConfig> GetSiteConfigByIdAndLanguageCodeAsync(string Id, string languageCode)
        {
            IEnumerable<IDictionary<string, object>> siteConfigInfoList = await dataManager.SearchEntitiesAsync(new QueryData
            {
                SingleRow = true,
                Entity = Constants.SiteConfig,
                IsDistinct = true,
                Conditions = new List<ConditionGroup>
                {
                    new ConditionGroup
                    {
                        Condition=new List<Condition>
                        {
                            new Condition
                            {
                                Property=Constants.Id,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=Id
                            },
                            new Condition
                            {
                                Property=Constants.LanguageCode,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=languageCode
                            }
                        },
                        Operator=LogicalOperator.And
                    }
                },

            });

            if (siteConfigInfoList is null || !siteConfigInfoList.Any())
            {
                throw new InavlidTenantException("Tenant is not found");
            }

            IDictionary<string, object> siteConfig = siteConfigInfoList.First();

            return ToSiteConfig(siteConfig);
        }

        public async Task<IList<SiteConfig>> GetSiteConfigByIdAsync(string Id)
        {
            IEnumerable<IDictionary<string, object>> siteConfigInfoList = await dataManager.SearchEntitiesAsync(new QueryData
            {
                Entity = Constants.SiteConfig,
                IsDistinct = true,
                Conditions = new List<ConditionGroup>
                {
                    new ConditionGroup
                    {
                        Condition=new List<Condition>
                        {
                            new Condition
                            {
                                Property=Constants.Id,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=Id
                            }
                        },
                    }
                },

            });

            if (siteConfigInfoList is null || !siteConfigInfoList.Any())
            {
                throw new InavlidTenantException("Tenant is not found");
            }

            return siteConfigInfoList.Select(siteConfigEntity => ToSiteConfig(siteConfigEntity)).ToList();
        }

        private static SiteConfig ToSiteConfig(IDictionary<string, object> siteConfigEntity)
        {
            return new SiteConfig
            {
                Id = siteConfigEntity["Id"] as string,
                Configurations = siteConfigEntity["Configurations"]?.ToString() != null ?
                    JsonConvert.DeserializeObject<IDictionary<string, object>>(siteConfigEntity["Configurations"]?.ToString()) :
                    null,

                Email = siteConfigEntity["Email"]?.ToString(),
                EntityCode = siteConfigEntity["EntityCode"]?.ToString(),
                ExtensionData = siteConfigEntity["ExtensionData"]?.ToString() != null ?
                    JsonConvert.DeserializeObject<IDictionary<string, object>>(siteConfigEntity["ExtensionData"]?.ToString()) :
                    null,

                FooterDescripion = siteConfigEntity["FooterDescripion"]?.ToString(),
                LabelColor = siteConfigEntity["LabelColor"]?.ToString(),
                LanguageCode = siteConfigEntity["LanguageCode"]?.ToString(),
                LanguageText = siteConfigEntity["LanguageText"]?.ToString(),
                LogoUrl = siteConfigEntity["LogoUrl"]?.ToString(),
                OfficalEntityDomain = siteConfigEntity["OfficalEntityDomain"]?.ToString(),
                OfficalEntityName = siteConfigEntity["OfficalEntityName"]?.ToString(),
                PrimaryColor = siteConfigEntity["PrimaryColor"]?.ToString(),
                SocialMediaLinks = siteConfigEntity["SocialMediaLinks"]?.ToString(),
                Styles = siteConfigEntity["Styles"]?.ToString() != null ?
                    JsonConvert.DeserializeObject<IDictionary<string, object>>(siteConfigEntity["Styles"]?.ToString()) :
                    null,

                TelephoneNumber = siteConfigEntity["TelephoneNumber"]?.ToString(),
                TextTypingColor = siteConfigEntity["TextTypingColor"]?.ToString(),
            };
        }
    }
}
