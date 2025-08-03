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

namespace MinPlatform.Schema.Migrators.DataBases.SqlServer.Processors.SqlServer
{
    using JetBrains.Annotations;
    using MinPlatform.Logging.Service;
    using MinPlatform.Schema.Migrators.DataBases.SqlServer.Generators.SqlServer;
    using MinPlatform.Schema.Migrators.Runner.Initialization;
    using MinPlatform.Schema.Migrators.Runner.Processors;


    public class SqlServer2005Processor : SqlServerProcessor
    {
        /// <inheritdoc />
        public SqlServer2005Processor(
            LoggerManager logger,
            [NotNull] SqlServer2005Quoter quoter,
            [NotNull] SqlServer2005Generator generator,
            ProcessorOptions options,
            [NotNull] IConnectionStringAccessor connectionStringAccessor)
            : base(new[] { ProcessorId.SqlServer2005, ProcessorId.SqlServer }, generator, quoter, logger, options, connectionStringAccessor)
        {
        }
    }
}
