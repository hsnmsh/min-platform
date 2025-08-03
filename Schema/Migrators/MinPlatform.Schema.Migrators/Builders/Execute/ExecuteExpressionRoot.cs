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

namespace MinPlatform.Schema.Migrators.Builders.Execute
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using MinPlatform.Schema.Migrators.Abstractions.Expressions;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Execute;

    /// <summary>
    /// The implementation of the <see cref="IExecuteExpressionRoot"/> interface.
    /// </summary>
    public class ExecuteExpressionRoot : IExecuteExpressionRoot
    {
        private readonly IMigrationContext context;
        private readonly IEnumerable<IEmbeddedResourceProvider> embeddedResources;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteExpressionRoot"/> class.
        /// </summary>
        /// <param name="context">The migration context</param>
        public ExecuteExpressionRoot(IMigrationContext context, IEnumerable<IEmbeddedResourceProvider> embeddedResources)
        {
            this.context = context;
            this.embeddedResources = embeddedResources ?? throw new ArgumentNullException(nameof(embeddedResources));

        }

        /// <inheritdoc />
        public void Sql(string sqlStatement)
        {
            var expression = new ExecuteSqlStatementExpression { SqlStatement = sqlStatement };
            context.Expressions.Add(expression);
        }

        /// <inheritdoc />
        public void Sql(string sqlStatement, IDictionary<string, string> parameters)
        {
            var expression = new ExecuteSqlStatementExpression
            {
                SqlStatement = sqlStatement,
                Parameters = parameters,
            };

            context.Expressions.Add(expression);
        }

        /// <inheritdoc />
        public void Sql(string sqlStatement, string description)
        {
            var expression = new ExecuteSqlStatementExpression
            {
                SqlStatement = sqlStatement,
                Description = description,
            };

            context.Expressions.Add(expression);
        }

        /// <inheritdoc />
        public void Sql(string sqlStatement, string description, IDictionary<string, string> parameters)
        {
            var expression = new ExecuteSqlStatementExpression
            {
                SqlStatement = sqlStatement,
                Description = description,
                Parameters = parameters,
            };

            context.Expressions.Add(expression);
        }

        /// <inheritdoc />
        public void Script(string pathToSqlScript, IDictionary<string, string> parameters)
        {
            var expression = new ExecuteSqlScriptExpression
            {
                SqlScript = pathToSqlScript,
                Parameters = parameters,
            };

            context.Expressions.Add(expression);
        }

        /// <inheritdoc />
        public void Script(string pathToSqlScript)
        {
            var expression = new ExecuteSqlScriptExpression { SqlScript = pathToSqlScript };
            context.Expressions.Add(expression);
        }

        /// <inheritdoc />
        public void WithConnection(Action<IDbConnection, IDbTransaction> operation)
        {
            var expression = new PerformDBOperationExpression { Operation = operation };
            context.Expressions.Add(expression);
        }

        /// <inheritdoc />
        public void EmbeddedScript(string embeddedSqlScriptName)
        {

            var expression = new ExecuteEmbeddedSqlScriptExpression(embeddedResources) { SqlScript = embeddedSqlScriptName };
            context.Expressions.Add(expression);

        }

        /// <inheritdoc />
        public void EmbeddedScript(string embeddedSqlScriptName, IDictionary<string, string> parameters)
        {
            var expression = new ExecuteEmbeddedSqlScriptExpression(embeddedResources)
            {
                SqlScript = embeddedSqlScriptName,
                Parameters = parameters,
            };

            context.Expressions.Add(expression);
        }
    }
}
