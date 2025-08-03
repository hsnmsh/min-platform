#region License
// Copyright (c) 2018, Fluent Migrator Project
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

namespace MinPlatform.Schema.Migrators.Runner.Initialization
{
    using System;
    using System.Linq;
    using System.Reflection;

    using JetBrains.Annotations;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;

    /// <summary>
    /// A compatibility service to get the assembly collection from the found migrations
    /// </summary>
    [Obsolete("Exists only to simplify the migration to the new FluentMigration version")]
    public class AssemblyCollectionService : IAssemblyCollection
    {
        private readonly Lazy<Assembly[]> lazyAssemblies;

        private readonly Lazy<Type[]> exportedTypes;

        private readonly Lazy<ManifestResourceNameWithAssembly[]> _resourceEntries;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyCollectionService"/> class.
        /// </summary>
        /// <param name="source">The source assemblies used to search for types with given traits</param>
        public AssemblyCollectionService([NotNull] IAssemblySource source)
        {
            lazyAssemblies = new Lazy<Assembly[]>(() => source.Assemblies.ToArray());
            exportedTypes = new Lazy<Type[]>(() => lazyAssemblies.Value.SelectMany(a => a.GetExportedTypes()).ToArray());
            _resourceEntries = new Lazy<ManifestResourceNameWithAssembly[]>(
                () => lazyAssemblies.Value.SelectMany(a => a.GetManifestResourceNames(), (asm, name) => new ManifestResourceNameWithAssembly(name, asm)).ToArray());
        }

        /// <inheritdoc />
        public Assembly[] Assemblies => lazyAssemblies.Value;

        /// <inheritdoc />
        public Type[] GetExportedTypes() => exportedTypes.Value;

        /// <inheritdoc />
        public ManifestResourceNameWithAssembly[] GetManifestResourceNames() => _resourceEntries.Value;
    }
}
