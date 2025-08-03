namespace MinPlatform.Schema.Migrators.Abstractions.Expressions
{
    using System.ComponentModel.DataAnnotations;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;

    /// <summary>
    /// Expression to delete a sequence
    /// </summary>
    public class DeleteSequenceExpression : MigrationExpressionBase, ISchemaExpression
    {
        /// <inheritdoc />
        public virtual string SchemaName { get; set; }

        /// <summary>
        /// Gets or sets the sequence name
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.SequenceNameCannotBeNullOrEmpty))]
        public virtual string SequenceName { get; set; }

        /// <inheritdoc />
        public override void ExecuteWith(IMigrationProcessor processor)
        {
            processor.Process(this);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return base.ToString() + SequenceName;
        }
    }
}
