namespace MinPlatform.FormBuilder.Elements.Inputs.InputResolvers
{
    using MinPlatform.Data.Service;
    using MinPlatform.EngineRule.Service;
    using MinPlatform.FormBuilder.Context;
    using MinPlatform.FormBuilder.DataProviders;
    using MinPlatform.FormBuilder.Elements.Inputs.InputTypes;
    using MinPlatform.FormBuilder.Elements.Inputs.Models;
    using System.Threading.Tasks;

    internal sealed class FileUploaderPropertyResolver : BaseInputPropertyResolver<FileUploaderPropertyInput, FileUploaderInputType>
    {
        public FileUploaderPropertyResolver(FileUploaderPropertyInput propertyInputType,
            IFormContext<string> formContext,
            IDataProviderFactory dataProviderFactory,
            EngineRuleManager engineRuleManager,
            DataManager dataManager) : base(propertyInputType, formContext, dataProviderFactory, engineRuleManager, dataManager)
        {
        }

        public async override Task<FileUploaderInputType> ResolveInputAsync()
        {
            await SetCommonPropertiesAsync();

            return new FileUploaderInputType
            {
                InputType = InputType.File.ToString().ToLower(),
                Classes = PropertyInputType.Classes,
                Disabled = Disabled,
                ErrorMessage = PropertyInputType.ErrorMessage,
                Id = PropertyInputType.Id,
                Label = PropertyInputType.Label,
                Name = PropertyInputType.Name,
                PlaceHolder = PropertyInputType.PlaceHolder,
                Readonly = Readonly,
                Required = Required,
                Value = Value ?? PropertyInputType.Value,
                Variant = PropertyInputType.Variant.ToString(),
                Visibility = Visibility,
                Accept = PropertyInputType.Accept,
                MultipleFiles = PropertyInputType.MultipleFiles,
                ShowClearButton = PropertyInputType.ShowClearButton,
                UploadOnPickFile = !string.IsNullOrEmpty(PropertyInputType.UploadUrl) ? true : PropertyInputType.UploadOnPickFile,
                UploadUrl = PropertyInputType.UploadUrl,
                MaxSize = PropertyInputType.MaxSize,
                DownloadUrl = PropertyInputType.DownloadUrl,
            };
        }
    }
}
