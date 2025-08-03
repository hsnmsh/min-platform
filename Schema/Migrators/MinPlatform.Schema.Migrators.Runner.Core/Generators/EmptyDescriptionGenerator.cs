namespace MinPlatform.Schema.Migrators.Runner.Generators
{
    using MinPlatform.Schema.Migrators.Abstractions.Expressions;
    using System.Collections.Generic;
    using System.Linq;

    public class EmptyDescriptionGenerator : IDescriptionGenerator
    {
        public IEnumerable<string> GenerateDescriptionStatements(CreateTableExpression expression)
        {
            return Enumerable.Empty<string>();
        }

        public string GenerateDescriptionStatement(AlterTableExpression expression)
        {
            return string.Empty;
        }

        public string GenerateDescriptionStatement(CreateColumnExpression expression)
        {
            return string.Empty;
        }

        public string GenerateDescriptionStatement(AlterColumnExpression expression)
        {
            return string.Empty;
        }
    }
}
