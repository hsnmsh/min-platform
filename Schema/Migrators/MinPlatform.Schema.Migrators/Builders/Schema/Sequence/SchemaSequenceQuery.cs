#region License
// Copyright (c) 2024, Fluent Migrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace MinPlatform.Schema.Migrators.Builders.Schema.Sequence
{
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Schema.Sequence;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;

    /// <summary>
    /// The implementation of the <see cref="ISchemaSequenceSyntax"/> interface.
    /// </summary>
    public class SchemaSequenceQuery : ISchemaSequenceSyntax
    {
        private readonly IMigrationContext context;
        private readonly string schemaName;
        private readonly string sequenceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaSequenceQuery"/> class.
        /// </summary>
        /// <param name="context">The migration context</param>
        /// <param name="schemaName">The schema name</param>
        /// <param name="sequenceName">The sequence name</param>
        public SchemaSequenceQuery(IMigrationContext context, string schemaName, string sequenceName)
        {
            this.context = context;
            this.schemaName = schemaName;
            this.sequenceName = sequenceName;
        }

        /// <inheritdoc />
        public bool Exists()
        {
            return context.QuerySchema.SequenceExists(schemaName, sequenceName);
        }
    }
}
