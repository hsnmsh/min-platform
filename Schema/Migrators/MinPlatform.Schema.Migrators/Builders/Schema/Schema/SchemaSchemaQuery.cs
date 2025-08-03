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

namespace MinPlatform.Schema.Migrators.Builders.Schema.Schema
{
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Schema.Schema;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Schema.Sequence;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Schema.Table;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Builders.Schema.Sequence;
    using MinPlatform.Schema.Migrators.Builders.Schema.Table;

    /// <summary>
    /// The implementation of the <see cref="ISchemaSchemaSyntax"/> interface.
    /// </summary>
    public class SchemaSchemaQuery : ISchemaSchemaSyntax
    {
        private readonly IMigrationContext context;
        private readonly string schemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaSchemaQuery"/> class.
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <param name="schemaName">The schema name</param>
        public SchemaSchemaQuery(IMigrationContext context, string schemaName)
        {
            this.context = context;
            this.schemaName = schemaName;
        }

        /// <inheritdoc />
        public bool Exists()
        {
            return context.QuerySchema.SchemaExists(schemaName);
        }

        /// <inheritdoc />
        public ISchemaTableSyntax Table(string tableName)
        {
            return new SchemaTableQuery(context, schemaName, tableName);
        }

        /// <inheritdoc />
        public ISchemaSequenceSyntax Sequence(string sequenceName)
        {
            return new SchemaSequenceQuery(context, schemaName, sequenceName);
        }
    }
}
