namespace MinPlatform.Schema.Migrators.Abstractions.Expressions
{
    using System.ComponentModel.DataAnnotations;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;

    /// <summary>
    /// Expression to delete constraints
    /// </summary>
    public class DeleteDefaultConstraintExpression : MigrationExpressionBase, ISchemaExpression
    {
        /// <inheritdoc />
        public virtual string SchemaName { get; set; }

        /// <summary>
        /// Gets or sets the table name
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.TableNameCannotBeNullOrEmpty))]
        public virtual string TableName { get; set; }

        /// <summary>
        /// Gets or sets the column name
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.ColumnNameCannotBeNullOrEmpty))]
        public virtual string ColumnName { get; set; }

        /// <inheritdoc />
        public override void ExecuteWith(IMigrationProcessor processor)
        {
            processor.Process(this);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return base.ToString() +
                   string.Format("{0}.{1} {2}",
                                 SchemaName,
                                 TableName,
                                 ColumnName);
        }
    }
}
