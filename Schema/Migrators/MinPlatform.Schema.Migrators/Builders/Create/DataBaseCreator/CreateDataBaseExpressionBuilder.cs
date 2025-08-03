namespace MinPlatform.Schema.Migrators.Builders.Create.DataBaseCreator
{
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.DataBaseCreator;
    using MinPlatform.Schema.Migrators.Abstractions.Expressions;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using System.Collections.Generic;

    public class CreateDataBaseExpressionBuilder : ExpressionBuilderBase<CreateDataBaseExpression>, ICreateDataBaseSyntax, ISupportAdditionalFeatures
    {
        public CreateDataBaseExpressionBuilder(CreateDataBaseExpression expression) : base(expression) { }

        public IDictionary<string, object> AdditionalFeatures { get; } = new Dictionary<string, object>();
    }
}
