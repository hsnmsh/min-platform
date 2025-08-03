namespace MinPlatform.Data.Service.QueryBuilder.Validations
{
    using MinPlatform.Data.Service.Models;
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Extensions;
    using MinPlatform.Validators.Service.ValidationExecutor;

    internal sealed class QueryDataValidatorExecutor : AbstractValidatorExecutor<QueryData>
    {
        public QueryDataValidatorExecutor(QueryData model) : base(model)
        {
            CreateDefinition(new StringDefinition()
            {
                Key = "entity",
                ThrowInValidException = true,

            }).NotNull<StringDefinition, string>().NotEmpty();
        }
    }
}
