#region License
//
// Copyright (c) 2007-2024, Fluent Migrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace MinPlatform.Schema.Migrators.Builders.Create
{
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.Column;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.Constraint;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.DataBaseCreator;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.ForeignKey;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.Index;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.Schema;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.Sequence;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.Table;
    using MinPlatform.Schema.Migrators.Abstractions.Expressions;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Abstractions.Model;
    using MinPlatform.Schema.Migrators.Builders.Create.Column;
    using MinPlatform.Schema.Migrators.Builders.Create.Constraint;
    using MinPlatform.Schema.Migrators.Builders.Create.DataBaseCreator;
    using MinPlatform.Schema.Migrators.Builders.Create.ForeignKey;
    using MinPlatform.Schema.Migrators.Builders.Create.Index;
    using MinPlatform.Schema.Migrators.Builders.Create.Schema;
    using MinPlatform.Schema.Migrators.Builders.Create.Sequence;
    using MinPlatform.Schema.Migrators.Builders.Create.Table;

    /// <summary>
    /// The <see cref="ICreateExpressionRoot"/> implementation
    /// </summary>
    public class CreateExpressionRoot : ICreateExpressionRoot
    {
        private readonly IMigrationContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateExpressionRoot"/> class.
        /// </summary>
        /// <param name="context">The migration context</param>
        public CreateExpressionRoot(IMigrationContext context)
        {
            this.context = context;
        }

        public ICreateDataBaseSyntax DataBase()
        {
            var expression = new CreateDataBaseExpression();
            context.Expressions.Add(expression);

            return new CreateDataBaseExpressionBuilder(expression);
        }

        /// <inheritdoc />
        public ICreateSchemaOptionsSyntax Schema(string schemaName)
        {
            var expression = new CreateSchemaExpression { SchemaName = schemaName };
            context.Expressions.Add(expression);
            return new CreateSchemaExpressionBuilder(expression);
        }

        /// <inheritdoc />
        public ICreateTableWithColumnOrSchemaOrDescriptionSyntax Table(string tableName)
        {
            var expression = new CreateTableExpression { TableName = tableName };
            context.Expressions.Add(expression);
            return new CreateTableExpressionBuilder(expression, context);
        }

        /// <inheritdoc />
        public ICreateColumnOnTableSyntax Column(string columnName)
        {
            var expression = new CreateColumnExpression { Column = { Name = columnName } };
            context.Expressions.Add(expression);
            return new CreateColumnExpressionBuilder(expression, context);
        }

        /// <inheritdoc />
        public ICreateForeignKeyFromTableSyntax ForeignKey()
        {
            var expression = new CreateForeignKeyExpression();
            context.Expressions.Add(expression);
            return new CreateForeignKeyExpressionBuilder(expression);
        }

        /// <inheritdoc />
        public ICreateForeignKeyFromTableSyntax ForeignKey(string foreignKeyName)
        {
            var expression = new CreateForeignKeyExpression { ForeignKey = { Name = foreignKeyName } };
            context.Expressions.Add(expression);
            return new CreateForeignKeyExpressionBuilder(expression);
        }

        /// <inheritdoc />
        public ICreateIndexForTableSyntax Index()
        {
            var expression = new CreateIndexExpression();
            context.Expressions.Add(expression);
            return new CreateIndexExpressionBuilder(expression);
        }

        /// <inheritdoc />
        public ICreateIndexForTableSyntax Index(string indexName)
        {
            var expression = new CreateIndexExpression { Index = { Name = indexName } };
            context.Expressions.Add(expression);
            return new CreateIndexExpressionBuilder(expression);
        }

        /// <inheritdoc />
        public ICreateSequenceInSchemaSyntax Sequence(string sequenceName)
        {
            var expression = new CreateSequenceExpression { Sequence = { Name = sequenceName } };
            context.Expressions.Add(expression);
            return new CreateSequenceExpressionBuilder(expression);
        }

        /// <inheritdoc />
        public ICreateConstraintOnTableSyntax UniqueConstraint()
        {
            var expression = new CreateConstraintExpression(ConstraintType.Unique);
            context.Expressions.Add(expression);
            return new CreateConstraintExpressionBuilder(expression);
        }

        /// <inheritdoc />
        public ICreateConstraintOnTableSyntax UniqueConstraint(string constraintName)
        {
            var expression = new CreateConstraintExpression(ConstraintType.Unique);
            expression.Constraint.ConstraintName = constraintName;
            context.Expressions.Add(expression);
            return new CreateConstraintExpressionBuilder(expression);
        }

        /// <inheritdoc />
        public ICreateConstraintOnTableSyntax PrimaryKey()
        {
            var expression = new CreateConstraintExpression(ConstraintType.PrimaryKey);
            context.Expressions.Add(expression);
            return new CreateConstraintExpressionBuilder(expression);
        }

        /// <inheritdoc />
        public ICreateConstraintOnTableSyntax PrimaryKey(string primaryKeyName)
        {
            var expression = new CreateConstraintExpression(ConstraintType.PrimaryKey);
            expression.Constraint.ConstraintName = primaryKeyName;
            context.Expressions.Add(expression);
            return new CreateConstraintExpressionBuilder(expression);
        }

        
    }
}
