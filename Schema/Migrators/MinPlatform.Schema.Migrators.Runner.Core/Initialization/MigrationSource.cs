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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using MinPlatform.Logging.Abstractions;
    using MinPlatform.Logging.Service;
    using MinPlatform.Schema.Migrators.Abstractions;

    /// <summary>
    /// The default implementation of a <see cref="IFilteringMigrationSource"/>.
    /// </summary>
    public class MigrationSource : IFilteringMigrationSource
    {
        [NotNull]
        private readonly IAssemblySource source;

        [NotNull]
        private readonly IMigrationRunnerConventions conventions;

        [NotNull]
        private readonly ConcurrentDictionary<Type, IMigration> instanceCache = new ConcurrentDictionary<Type, IMigration>();

        [NotNull, ItemNotNull]
        private readonly IEnumerable<IMigrationSourceItem> sourceItems;

        private readonly LoggerManager logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileSource"/> class.
        /// </summary>
        /// <param name="source">The assembly source</param>
        /// <param name="conventions">The migration runner conventios</param>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="sourceItems">The additional migration source items</param>
        /// <param name="logger">The logger for troubleshooting "No migrations found" error.</param>
        public MigrationSource(
            [NotNull] IAssemblySource source,
            [NotNull] IMigrationRunnerConventions conventions,
            [NotNull, ItemNotNull] IEnumerable<IMigrationSourceItem> sourceItems,
             LoggerManager logger)
        {
            this.source = source;
            this.conventions = conventions;
            this.sourceItems = sourceItems;
            this.logger = logger;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileSource"/> class.
        /// </summary>
        /// <param name="source">The assembly source</param>
        /// <param name="conventions">The migration runner conventions</param>
        [Obsolete]
        public MigrationSource(
            [NotNull] IAssemblySource source,
            [NotNull] IMigrationRunnerConventions conventions)
        {
            this.source = source;
            this.conventions = conventions;
            sourceItems = Enumerable.Empty<IMigrationSourceItem>();
        }

        /// <inheritdoc />
        public IEnumerable<IMigration> GetMigrations()
        {
            return GetMigrations(conventions.TypeIsMigration);
        }

        /// <inheritdoc />
        public IEnumerable<IMigration> GetMigrations(Func<Type, bool> predicate)
        {
            foreach (var type in GetMigrationTypeCandidates())
            {
                if (type.IsAbstract)
                {
                    logger.Information($"Type [{type.AssemblyQualifiedName}] is abstract. Skipping.");
                    continue;
                }

                if (!(typeof(IMigration).IsAssignableFrom(type) || typeof(MigrationBase).IsAssignableFrom(type)))
                {
                    logger.Information($"Type [{type.AssemblyQualifiedName}] is not assignable to IMigration. Skipping.");
                    continue;
                }

                if (!(predicate == null || predicate(type)))
                {
                    logger.Information($"Type [{type.AssemblyQualifiedName}] doesn't satisfy predicate. Skipping.");
                    continue;
                }

                logger.Information($"Type {type.AssemblyQualifiedName} is a migration. Adding.");

                yield return instanceCache.GetOrAdd(type, CreateInstance);
            }
        }

        private IEnumerable<Type> GetExportedTypes()
        {
            return source
                .Assemblies.SelectMany(a => a.GetExportedTypes());
        }

        private IEnumerable<Type> GetMigrationTypeCandidates()
        {
            return GetExportedTypes()
                .Union(sourceItems.SelectMany(i => i.MigrationTypeCandidates));
        }

        private IMigration CreateInstance(Type type)
        {
            return (IMigration)Activator.CreateInstance(type);
        }
    }
}
