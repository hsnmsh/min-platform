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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinPlatform.Schema.Migrators.Abstractions;
using MinPlatform.Schema.Migrators.DataBases.SqlServer.BatchParser;
using MinPlatform.Schema.Migrators.DataBases.SqlServer.Generators.SqlServer;
using MinPlatform.Schema.Migrators.DataBases.SqlServer.Processors.SqlServer;
using MinPlatform.Schema.Migrators.Runner;

namespace MinPlatform.Schema.Migrators.DataBases.SqlServer.Extensions
{
   
    public static class SqlServerRunnerServiceCollectionExtensions
    {
      
        public static IMigrationRunnerBuilder<IServiceCollection> AddSqlServer(this IMigrationRunnerBuilder<IServiceCollection> builder)
        {
            builder.Services.TryAddTransient<SqlServerBatchParser>();
            builder.Services.TryAddScoped<SqlServer2008Quoter>();
            builder.Services.TryAddScoped<ISqlServerTypeMap>(sp => new SqlServer2008TypeMap());
            builder.Services
                .AddScoped<SqlServer2016Processor>()
                .AddScoped<IMigrationProcessor>(sp => sp.GetRequiredService<SqlServer2016Processor>())
                .AddScoped<SqlServer2016Generator>()
                .AddScoped<IMigrationGenerator>(sp => sp.GetRequiredService<SqlServer2016Generator>());

            return builder;
        }

        public static IMigrationRunnerBuilder<IServiceCollection> AddSqlServer2000(this IMigrationRunnerBuilder<IServiceCollection> builder)
        {
            builder.Services.TryAddTransient<SqlServerBatchParser>();
            builder.Services.TryAddScoped<SqlServer2000Quoter>();
            builder.Services
                .AddScoped<SqlServer2000Processor>()
                .AddScoped<IMigrationProcessor>(sp => sp.GetRequiredService<SqlServer2000Processor>())
                .AddScoped<ISqlServerTypeMap>(sp => new SqlServer2000TypeMap())
                .AddScoped<SqlServer2000Generator>()
                .AddScoped<IMigrationGenerator>(sp => sp.GetRequiredService<SqlServer2000Generator>());

            return builder;
        }

        public static IMigrationRunnerBuilder<IServiceCollection> AddSqlServer2005(this IMigrationRunnerBuilder<IServiceCollection> builder)
        {
            builder.Services.TryAddTransient<SqlServerBatchParser>();
            builder.Services.TryAddScoped<SqlServer2005Quoter>();
            builder.Services
                .AddScoped<SqlServer2005Processor>()
                .AddScoped<IMigrationProcessor>(sp => sp.GetRequiredService<SqlServer2005Processor>())
                .AddScoped<ISqlServerTypeMap>(sp => new SqlServer2005TypeMap())
                .AddScoped<SqlServer2005Generator>()
                .AddScoped<IMigrationGenerator>(sp => sp.GetRequiredService<SqlServer2005Generator>());

            return builder;
        }

        public static IMigrationRunnerBuilder<IServiceCollection> AddSqlServer2008(this IMigrationRunnerBuilder<IServiceCollection> builder)
        {
            builder.Services.TryAddTransient<SqlServerBatchParser>();
            builder.Services.TryAddScoped<SqlServer2008Quoter>();
            builder.Services.TryAddScoped<ISqlServerTypeMap>(sp => sp.GetRequiredService<SqlServer2008TypeMap>());
            builder.Services
                .AddScoped<SqlServer2008Processor>()
                .AddScoped<IMigrationProcessor>(sp => sp.GetRequiredService<SqlServer2008Processor>())
                .AddScoped<SqlServer2008Generator>()
                .AddScoped<IMigrationGenerator>(sp => sp.GetRequiredService<SqlServer2008Generator>());

            return builder;
        }

        public static IMigrationRunnerBuilder<IServiceCollection> AddSqlServer2012(this IMigrationRunnerBuilder<IServiceCollection> builder)
        {
            builder.Services.TryAddTransient<SqlServerBatchParser>();
            builder.Services.TryAddScoped<SqlServer2008Quoter>();
            builder.Services
                .AddScoped<SqlServer2012Processor>()
                .AddScoped<IMigrationProcessor>(sp => sp.GetRequiredService<SqlServer2012Processor>())
                .AddScoped<ISqlServerTypeMap>(sp => new SqlServer2008TypeMap())
                .AddScoped<SqlServer2012Generator>()
                .AddScoped<IMigrationGenerator>(sp => sp.GetRequiredService<SqlServer2012Generator>());

            return builder;
        }

        public static IMigrationRunnerBuilder<IServiceCollection> AddSqlServer2014(this IMigrationRunnerBuilder<IServiceCollection> builder)
        {
            builder.Services.TryAddTransient<SqlServerBatchParser>();
            builder.Services.TryAddScoped<SqlServer2008Quoter>();
            builder.Services
                .AddScoped<SqlServer2014Processor>()
                .AddScoped<IMigrationProcessor>(sp => sp.GetRequiredService<SqlServer2014Processor>())
                .AddScoped<ISqlServerTypeMap>(sp => new SqlServer2008TypeMap())
                .AddScoped<SqlServer2014Generator>()
                .AddScoped<IMigrationGenerator>(sp => sp.GetRequiredService<SqlServer2014Generator>());

            return builder;
        }

        public static IMigrationRunnerBuilder<IServiceCollection> AddSqlServer2016(this IMigrationRunnerBuilder<IServiceCollection> builder)
        {
            builder.Services.TryAddTransient<SqlServerBatchParser>();
            builder.Services.TryAddScoped<SqlServer2008Quoter>();
            builder.Services
                .AddScoped<SqlServer2016Processor>()
                .AddScoped<IMigrationProcessor>(sp => sp.GetRequiredService<SqlServer2016Processor>())
                .AddScoped<ISqlServerTypeMap>(sp => new SqlServer2008TypeMap())
                .AddScoped<SqlServer2016Generator>()
                .AddScoped<IMigrationGenerator>(sp => sp.GetRequiredService<SqlServer2016Generator>());

            return builder;
        }
    }
}
