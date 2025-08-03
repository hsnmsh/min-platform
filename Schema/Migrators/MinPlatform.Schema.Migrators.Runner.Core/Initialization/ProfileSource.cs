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
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;
    using MinPlatform.Schema.Migrators.Abstractions;

    /// <summary>
    /// The default implementation of <see cref="IProfileSource"/>
    /// </summary>
    public class ProfileSource : IProfileSource
    {
        [NotNull]
        private readonly IFilteringMigrationSource source;

        [NotNull]
        private readonly IMigrationRunnerConventions conventions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileSource"/> class.
        /// </summary>
        /// <param name="source">The assembly source</param>
        /// <param name="conventions">The migration runner conventions</param>
        public ProfileSource(
            [NotNull] IFilteringMigrationSource source,
            [NotNull] IMigrationRunnerConventions conventions)
        {
            this.source = source;
            this.conventions = conventions;
        }

        /// <inheritdoc />
        public IEnumerable<IMigration> GetProfiles(string profile) =>
            source.GetMigrations(t => IsSelectedProfile(t, profile));

        private bool IsSelectedProfile(Type type, string profile)
        {
            if (!conventions.TypeIsProfile(type))
                return false;
            var profileAttribute = type.GetCustomAttribute<ProfileAttribute>();
            return !string.IsNullOrEmpty(profile) && string.Equals(profileAttribute.ProfileName, profile);
        }
    }
}
