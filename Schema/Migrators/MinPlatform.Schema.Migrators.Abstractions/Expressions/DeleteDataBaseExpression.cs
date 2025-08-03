namespace MinPlatform.Schema.Migrators.Abstractions.Expressions
{
    public class DeleteDataBaseExpression : MigrationExpressionBase
    {
        public override void ExecuteWith(IMigrationProcessor processor)
        {
            processor.Process(this);
        }
    }
}
