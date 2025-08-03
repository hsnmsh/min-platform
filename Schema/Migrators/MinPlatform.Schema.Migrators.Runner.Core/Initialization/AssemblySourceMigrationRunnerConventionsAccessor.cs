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
    using MinPlatform.Schema.Migrators.Runner.Infrastructure;
    using JetBrains.Annotations;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Searches for a <see cref="IMigrationRunnerConventions"/> implementation in the given assemblies
    /// </summary>
    public class AssemblySourceMigrationRunnerConventionsAccessor : IMigrationRunnerConventionsAccessor
    {
        private readonly Lazy<IMigrationRunnerConventions> lazyConventions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblySourceMigrationRunnerConventionsAccessor"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to instantiate the found <see cref="IMigrationRunnerConventions"/> implementation</param>
        /// <param name="assemblySource">The assemblies used to search for the <see cref="IMigrationRunnerConventions"/> implementation</param>
        public AssemblySourceMigrationRunnerConventionsAccessor(
            [CanBeNull] IAssemblySource assemblySource = null)
        {
            lazyConventions = new Lazy<IMigrationRunnerConventions>(
                () =>
                {
                    if (assemblySource == null)
                        return DefaultMigrationRunnerConventions.Instance;

                    var matchedType = assemblySource.Assemblies.SelectMany(a => a.GetExportedTypes())
                        .FirstOrDefault(t => typeof(IMigrationRunnerConventions).IsAssignableFrom(t));

                    if (matchedType != null)
                    {
                        return (IMigrationRunnerConventions)Activator.CreateInstance(matchedType);
                    }

                    return DefaultMigrationRunnerConventions.Instance;
                });
        }

        /// <inheritdoc />
        public IMigrationRunnerConventions MigrationRunnerConventions => lazyConventions.Value;
    }
}
