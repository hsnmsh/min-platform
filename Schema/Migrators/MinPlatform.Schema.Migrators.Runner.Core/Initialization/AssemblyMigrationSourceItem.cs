#region License
//
// Copyright (c) 2018, Fluent Migrator Project
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

namespace MinPlatform.Schema.Migrators.Runner.Initialization
{
    using MinPlatform.Schema.Migrators.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Implementation of <see cref="IMigrationSourceItem"/> that accepts a collection of assemblies
    /// </summary>
    public class AssemblyMigrationSourceItem : IMigrationSourceItem
    {
        private readonly IReadOnlyCollection<Assembly> assemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyMigrationSourceItem"/> class.
        /// </summary>
        /// <param name="assemblies">The assemblies to get the candidate types from</param>
        public AssemblyMigrationSourceItem(IReadOnlyCollection<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        /// <inheritdoc />
        public IEnumerable<Type> MigrationTypeCandidates => assemblies
            .SelectMany(a => a.GetExportedTypes())
            .Where(t => typeof(IMigration).IsAssignableFrom(t) && !t.IsAbstract);
    }
}
