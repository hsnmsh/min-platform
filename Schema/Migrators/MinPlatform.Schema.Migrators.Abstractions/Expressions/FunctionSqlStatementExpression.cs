namespace MinPlatform.Schema.Migrators.Abstractions.Expressions
{
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using System.ComponentModel.DataAnnotations;

    public class FunctionSqlStatementExpression : ExecuteSqlScriptExpressionBase
    {
        /// <summary>
        /// Gets or sets the SQL statement to be executed
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.SqlStatementCannotBeNullOrEmpty))]
        public virtual string SqlStatement { get; set; }

        /// <summary>
        /// Gets or sets the description for this SQL statement
        /// </summary>
        public virtual string Description { get; set; }

        /// <inheritdoc />
        public override void ExecuteWith(IMigrationProcessor processor)
        {
            var finalSqlScript = SqlScriptTokenReplacer.ReplaceSqlScriptTokens(SqlStatement, Parameters);
            processor.Execute(finalSqlScript);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return base.ToString() + (Description ?? SqlStatement);
        }
    }
}
