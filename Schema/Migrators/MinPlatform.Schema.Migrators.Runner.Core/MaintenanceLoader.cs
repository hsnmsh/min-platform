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

namespace MinPlatform.Schema.Migrators.Runner
{
    using JetBrains.Annotations;
    using MinPlatform.Schema.Migrators.Abstractions;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Infrastructure;
    using MinPlatform.Schema.Migrators.Infrastructure.Extensions;
    using MinPlatform.Schema.Migrators.Runner.Initialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MaintenanceLoader : IMaintenanceLoader
    {
        private readonly IDictionary<MigrationStage, IList<IMigration>> maintenance;

        [Obsolete]
        public MaintenanceLoader(IAssemblyCollection assemblyCollection, IEnumerable<string> tags, IMigrationRunnerConventions conventions)
        {
            var tagsList = tags?.ToArray() ?? new string[0];

            maintenance = (
                from a in assemblyCollection.Assemblies
                    from type in a.GetExportedTypes()
                    let stage = conventions.GetMaintenanceStage(type)
                    where stage != null
                where conventions.HasRequestedTags(type, tagsList, false)
                let migration = (IMigration)Activator.CreateInstance(type)
                group migration by stage.GetValueOrDefault()
            ).ToDictionary(
                g => g.Key,
                g => (IList<IMigration>)g.OrderBy(m => m.GetType().Name).ToArray()
            );
        }

        public MaintenanceLoader(
            [NotNull] IAssemblySource assemblySource,
            RunnerOptions options,
            [NotNull] IMigrationRunnerConventions conventions,
            IMigration mainMigration)
        {
            var tagsList = options.Tags ?? new string[0];

            var types = assemblySource.Assemblies.SelectMany(a => a.ExportedTypes).ToList();

            maintenance = (
                from type in types
                let stage = conventions.GetMaintenanceStage(type)
                where stage != null
                where conventions.HasRequestedTags(type, tagsList, options.IncludeUntaggedMaintenances)
                let migration = mainMigration
                group migration by stage.GetValueOrDefault()
            ).ToDictionary(
                g => g.Key,
                g => (IList<IMigration>)g.OrderBy(m => m.GetType().Name).ToArray()
            );
        }

        public IList<IMigrationInfo> LoadMaintenance(MigrationStage stage)
        {
            IList<IMigrationInfo> migrationInfos = new List<IMigrationInfo>();
            if (!maintenance.TryGetValue(stage, out var migrations))
                return migrationInfos;

            foreach (var migration in migrations)
            {
                var transactionBehavior = migration.GetType().GetOneAttribute<MaintenanceAttribute>().TransactionBehavior;
                migrationInfos.Add(new NonAttributedMigrationToMigrationInfoAdapter(migration, transactionBehavior));
            }

            return migrationInfos;
        }
    }
}
