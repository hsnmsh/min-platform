
namespace MinPlatform.Schema.Migrators.Abstractions.Expressions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Abstractions.Model;

    /// <summary>
    /// Expression to delete a constraint
    /// </summary>
    public class DeleteConstraintExpression : MigrationExpressionBase, ISupportAdditionalFeatures, IConstraintExpression, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteConstraintExpression"/> class.
        /// </summary>
        public DeleteConstraintExpression(ConstraintType type)
        {
            Constraint = new ConstraintDefinition(type);
        }

        /// <summary>
        /// Gets or sets the constraint definition
        /// </summary>
        public ConstraintDefinition Constraint { get; set; }

        /// <inheritdoc />
        public IDictionary<string, object> AdditionalFeatures => Constraint.AdditionalFeatures;

        /// <inheritdoc />
        public override void ExecuteWith(IMigrationProcessor processor)
        {
            processor.Process(this);
        }

        /// <inheritdoc />
        public override string ToString()
        {

            return base.ToString() + Constraint.ConstraintName;
        }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Constraint.TableName))
            {
                yield return new ValidationResult(ErrorMessages.TableNameCannotBeNullOrEmpty);
            }
        }
    }
}
