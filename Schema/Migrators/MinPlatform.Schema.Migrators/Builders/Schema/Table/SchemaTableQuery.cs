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

namespace MinPlatform.Schema.Migrators.Builders.Schema.Table
{
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Schema.Column;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Schema.Constraint;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Schema.Index;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Schema.Table;
    using MinPlatform.Schema.Migrators.Builders.Schema.Column;
    using MinPlatform.Schema.Migrators.Builders.Schema.Index;
    using MinPlatform.Schema.Migrators.Builders.Schema.Constraint;

    /// <summary>
    /// The implementation of the <see cref="ISchemaTableSyntax"/> interface.
    /// </summary>
    public class SchemaTableQuery : ISchemaTableSyntax
    {
        private readonly IMigrationContext context;
        private readonly string schemaName;
        private readonly string tableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaTableQuery"/> class.
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <param name="schemaName">The schema name</param>
        /// <param name="tableName">The table name</param>
        public SchemaTableQuery(IMigrationContext context, string schemaName, string tableName)
        {
            this.context = context;
            this.schemaName = schemaName;
            this.tableName = tableName;
        }

        /// <inheritdoc />
        public bool Exists()
        {
            return context.QuerySchema.TableExists(schemaName, tableName);
        }

        /// <inheritdoc />
        public ISchemaColumnSyntax Column(string columnName)
        {
            return new SchemaColumnQuery(schemaName, tableName, columnName, context);
        }

        /// <inheritdoc />
        public ISchemaIndexSyntax Index(string indexName)
        {
            return new SchemaIndexQuery(schemaName, tableName, indexName, context);
        }

        /// <inheritdoc />
        public ISchemaConstraintSyntax Constraint(string constraintName)
        {
            return new SchemaConstraintQuery(schemaName, tableName, constraintName, context);
        }
    }
}
