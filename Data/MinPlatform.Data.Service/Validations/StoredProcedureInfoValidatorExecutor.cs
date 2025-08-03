namespace MinPlatform.Data.Service.Validations
{
    using MinPlatform.Data.Service.Models;
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Extensions;
    using MinPlatform.Validators.Service.ValidationExecutor;

    internal sealed class StoredProcedureInfoValidatorExecutor : AbstractValidatorExecutor<StoredProcedureInfo>
    {
        public StoredProcedureInfoValidatorExecutor(StoredProcedureInfo model) : base(model)
        {
            CreateDefinition(new StringDefinition()
            {
                Key = "SPName",
                ThrowInValidException = true,

            }).NotNull<StringDefinition, string>().NotEmpty();
        }
    }
}
