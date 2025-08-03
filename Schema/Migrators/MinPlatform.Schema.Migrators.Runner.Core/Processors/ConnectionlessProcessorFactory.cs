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

namespace MinPlatform.Schema.Migrators.Runner.Processors
{
    using System;
    using MinPlatform.Schema.Migrators.Runner.Generators;
    using MinPlatform.Schema.Migrators.Runner.Initialization;
    using MinPlatform.Schema.Migrators.Runner.Logging;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Options;
    using MinPlatform.Schema.Migrators.Abstractions;
    using MinPlatform.Logging.Service;

    /// <summary>
    /// A processor factory to create SQL statements only (without executing them)
    /// </summary>
    [Obsolete]
    public class ConnectionlessProcessorFactory : IMigrationProcessorFactory
    {
        private readonly LoggerManager loggerManager;

        private readonly SelectingProcessorAccessorOptions selectingProcessorAccessorOptions;

        [NotNull]
        private readonly IMigrationGenerator generator;

        [NotNull]
        private readonly string databaseId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionlessProcessorFactory"/> class.
        /// </summary>
        /// <param name="generatorAccessor">The accessor to get the migration generator to use</param>
        /// <param name="runnerContext">The runner context</param>
        [Obsolete]
        public ConnectionlessProcessorFactory(
            [NotNull] IGeneratorAccessor generatorAccessor,
            [NotNull] IRunnerContext runnerContext,
            LoggerManager loggerManager,
            SelectingProcessorAccessorOptions selectingProcessorAccessorOptions)
        {
            generator = generatorAccessor.Generator;
            databaseId = runnerContext.Database;
            Name = generator.GetName();
            this.loggerManager = loggerManager;
            this.selectingProcessorAccessorOptions = selectingProcessorAccessorOptions;
        }

        /// <inheritdoc />
        [Obsolete]
        public IMigrationProcessor Create(string connectionString, IAnnouncer announcer, IMigrationProcessorOptions options)
        {
            var processorOptions = options.GetProcessorOptions(connectionString);
            return new ConnectionlessProcessor(
                new PassThroughGeneratorAccessor(generator),
                loggerManager,
                processorOptions,
                selectingProcessorAccessorOptions);
        }

        /// <inheritdoc />
        public bool IsForProvider(string provider) => true;

        /// <inheritdoc />
        public string Name { get; }

        private class PassThroughGeneratorAccessor : IGeneratorAccessor
        {
            public PassThroughGeneratorAccessor(IMigrationGenerator generator)
            {
                Generator = generator;
            }

            /// <inheritdoc />
            public IMigrationGenerator Generator { get; }
        }


    }
}
