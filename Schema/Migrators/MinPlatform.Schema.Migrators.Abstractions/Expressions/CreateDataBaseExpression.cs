namespace MinPlatform.Schema.Migrators.Abstractions.Expressions
{
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using System.Collections.Generic;

    public class CreateDataBaseExpression : MigrationExpressionBase, ISupportAdditionalFeatures
    {
        public IDictionary<string, object> AdditionalFeatures { get; } = new Dictionary<string, object>();

        public override void ExecuteWith(IMigrationProcessor processor)
        {
            processor.Process(this);
        }
    }
}
