namespace MinPlatform.FormBuilder.Engine
{
    using MinPlatform.Data.Service;
    using MinPlatform.EngineRule.Service;
    using MinPlatform.FormBuilder.Context;
    using MinPlatform.FormBuilder.DataProviders;
    using MinPlatform.FormBuilder.Engine.DataService;
    using MinPlatform.Tenant.Service.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class FormManager
    {
        private readonly IEnumerable<SiteConfig> siteConfigs;

        private readonly IFormInfoDataService formInfoDataService;
        private readonly IDataProviderFactory dataProviderFactory;
        private readonly IFormResolver formResolver;
        private readonly EngineRuleManager engineRuleManager;
        private readonly DataManager dataManager;


        public FormManager(IFormInfoDataService formInfoDataService,
            IFormResolver formResolver,
            EngineRuleManager engineRuleManager,
            DataManager dataManager,
            IEnumerable<SiteConfig> siteConfigs,
            IDataProviderFactory dataProviderFactory)
        {
            this.dataProviderFactory = dataProviderFactory;
            this.formInfoDataService = formInfoDataService;
            this.formResolver = formResolver;
            this.engineRuleManager = engineRuleManager;
            this.dataManager = dataManager;
            this.siteConfigs = siteConfigs;
        }

        public async Task<FormInfoAndProperties> GetFromInfoAsync(string formName, IUserProfile userProfile)
        {
            var formEntities = await formInfoDataService.GetFormsAsync(formName);

            if (formEntities is null || !formEntities.Any())
            {
                throw new FormEngineException($"form name {formName} is not found");
            }

            var formContext = new FormContext()
            {
                Id = Guid.NewGuid().ToString(),
                Name = formName,
                Properties = (formEntities.First().InputProperties != null && formEntities.First().InputProperties.Any()) ?
                    formEntities.First().InputProperties :
                    new Dictionary<string, object>(),
                User = userProfile,
                Request = new RequestInfo(),
                SiteConfig = siteConfigs
            };

            var translatedForms = await Task.WhenAll(formEntities.Select(async formEntity =>
            await formResolver.ResolveFromAsync(formEntity, formContext, dataProviderFactory, engineRuleManager, dataManager)));

            return new FormInfoAndProperties
            {
                FormProperties = formContext,
                Forms = translatedForms.ToDictionary(x => x.LanguageCode, x => x)
            };


        }


    }
}
