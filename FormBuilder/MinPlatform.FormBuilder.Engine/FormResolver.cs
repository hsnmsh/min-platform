namespace MinPlatform.FormBuilder.Engine
{
    using MinPlatform.Data.Service;
    using MinPlatform.EngineRule.Service;
    using MinPlatform.FormBuilder.Context;
    using MinPlatform.FormBuilder.DataProviders;
    using MinPlatform.FormBuilder.Elements.Sections;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    internal sealed class FormResolver : IFormResolver
    {
        public async Task<FormInfo> ResolveFromAsync(FormEntity form, IFormContext<string> formContext, IDataProviderFactory dataProviderFactory,
            EngineRuleManager engineRuleManager,
            DataManager dataManager)
        {
            IEnumerable<string> roles = formContext.User.UserInfo.Claims
                         .Where(c => c.Type == ClaimTypes.Role)
                         .Select(c => c.Value);

            string roleClaim = formContext.User.UserInfo.Claims
                         .Where(c => c.Type == "role").First().Value;

            if (!form.AllowedRoles.Any(role => roles.Contains(role)) && !form.AllowedRoles.Contains(roleClaim))
            {
                throw new FormEngineException("The User that access the request is not authorized");
            }

            var sectionResolver = new FormGroupSectionResolver();

            var formInfo = new FormInfo
            {
                Id = form.Id.ToString(),
                Description = form.Description,
                LanguageCode = form.LanguageCode,
                LanguageText = form.LanguageText,
                Name = form.Name,
                ServiceCode = form.ServiceCode,
                TargetClient = form.TargetClient,
                Title = form.Title,
            };

            var groupSections = new List<FormGroupSection>();

            foreach (FormGroupPropertySection section in form.FormGroups)
            {
                FormGroupSection formGroupSection = await sectionResolver.ResolveSectionAsync(section, formContext, dataProviderFactory, engineRuleManager, dataManager);

                groupSections.Add(formGroupSection);
            }

            formInfo.FormGroups = groupSections;

            return formInfo;

        }
    }
}
