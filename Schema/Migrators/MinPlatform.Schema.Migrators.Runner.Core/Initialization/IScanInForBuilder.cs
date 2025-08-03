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
    using MinPlatform.Schema.Migrators.Runner.Conventions;

    /// <summary>
    /// Defines how the scanned assemblies are used
    /// </summary>
    public interface IScanInForBuilder<ServiceBuilderType> : IScanIn where ServiceBuilderType : class
    {
        /// <summary>
        /// Use the scanned assemblies for migrations
        /// </summary>
        /// <returns>The next step</returns>
        IScanInBuilder<ServiceBuilderType> Migrations();

        /// <summary>
        /// Use the scanned assemblies for version table metadata
        /// </summary>
        /// <returns>The next step</returns>
        IScanInBuilder<ServiceBuilderType> VersionTableMetaData();

        /// <summary>
        /// Use the scanned assemblies to search for types implementing <see cref="IConventionSet"/>
        /// </summary>
        /// <returns></returns>
        IScanInBuilder<ServiceBuilderType> ConventionSet();

        /// <summary>
        /// Use the scanned assemblies for embedded resources
        /// </summary>
        /// <returns>The next step</returns>
        IScanInBuilder<ServiceBuilderType> EmbeddedResources();

        /// <summary>
        /// Use the scanned assemblies for everything (migrations, etc...)
        /// </summary>
        /// <returns>The next step</returns>
        IMigrationRunnerBuilder<ServiceBuilderType> All();
    }
}
