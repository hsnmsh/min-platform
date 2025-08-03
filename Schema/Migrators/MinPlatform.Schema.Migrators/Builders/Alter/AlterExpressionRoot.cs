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

namespace MinPlatform.Schema.Migrators.Builders.Alter
{
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Alter;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Alter.Column;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Alter.Table;
    using MinPlatform.Schema.Migrators.Abstractions.Expressions;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Builders.Alter.Column;
    using MinPlatform.Schema.Migrators.Builders.Alter.Table;

    /// <summary>
    /// The root expression for alterations
    /// </summary>
    public class AlterExpressionRoot : IAlterExpressionRoot
    {
        private readonly IMigrationContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlterExpressionRoot"/> class.
        /// </summary>
        /// <param name="context">The migration context</param>
        public AlterExpressionRoot(IMigrationContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public IAlterTableAddColumnOrAlterColumnOrSchemaOrDescriptionSyntax Table(string tableName)
        {
            var expression = new AlterTableExpression { TableName = tableName };
            context.Expressions.Add(expression);
            return new AlterTableExpressionBuilder(expression, context);
        }

        /// <inheritdoc />
        public IAlterColumnOnTableSyntax Column(string columnName)
        {
            var expression = new AlterColumnExpression { Column = { Name = columnName } };
            context.Expressions.Add(expression);
            return new AlterColumnExpressionBuilder(expression, context);
        }
    }
}
