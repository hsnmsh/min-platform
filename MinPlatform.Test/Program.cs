using Autofac;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinPlatform.Caching.InMemory.Extensions;
using MinPlatform.Caching.Service.Extensions;
using MinPlatform.Caching.Service.Models;
using MinPlatform.ConfigStore.Service.Extensions;
using MinPlatform.Data.Service.Extensions;
using MinPlatform.Data.Sql.Extensions;
using MinPlatform.DI.Service.Extensions;
using MinPlatform.Schema.Builder;
using MinPlatform.Schema.Migrators.Runner.Core.Extensions;
using MinPlatform.Schema.Migrators.DataBases.SqlServer.Extensions;
using MinPlatform.Schema.Abstractions.Models;

namespace MinPlatform.Test
{
    internal class Program
    {
        private static System.Timers.Timer timer1;
        private static string TestSchemaLibraryConnectionString = "Server=localhost;Database=TestSchemaLibrary;Trusted_Connection=true;MultipleActiveResultSets=true;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=True";

        public static void Main(string[] args)
        {

            var containerBuilder = new ContainerBuilder();
            containerBuilder
                .AddSchemaMigratorCore()
                .AddConventionSet()
                .ConfigureRunner
                (x => x.WithGlobalConnectionString(TestSchemaLibraryConnectionString)
                .AddSqlServer()
                        .AddSqlServer2000()
                        .AddSqlServer2005()
                        .AddSqlServer2008()
                        .AddSqlServer2012()
                        .AddSqlServer2014()
                        .AddSqlServer2016()
                        .AddSqlServerOption()
                );

            var container = containerBuilder.Build();

            var manager = container.Resolve<SchemaBuilderManager>();

            manager.MigrateUp();

            Console.ReadLine();
        }

        static IHost CreateHostBuilder(string[] args)
        {
            //return Host.CreateDefaultBuilder(args).ConfigureServices((context, services) =>
            //{
            //    var t = Assembly.GetExecutingAssembly();

            //    services.RegisterCustomDecorator<TestDecorator, LoggingDecorator>();
            //    //services.AddTransient<ITestDecorator, TestDecorator>();


            //    //services.AddServicesByNamingConvention("Service", t);

            //});

            return Host.CreateDefaultBuilder(args).ConfigureLogging((a, q) =>
            {
                q.ClearProviders();
                q.SetMinimumLevel(LogLevel.Information);


            }).UseStartup((ctx, services) =>
            {
                services.RegisterSqlServerBuilders(() => { return TestSchemaLibraryConnectionString; });
                services.RegisterDataContext();

                services.AddInMemoryCache();
                services.AddCachingContext(new CachingOption() { ExpireTime = 1 });

                services.AddConfigStore();


            }).Build();




        }


    }

    public sealed class Migration1 : BaseSchemaMigrator
    {
        public override string Name => "migration1";

        public override void Up(BaseSchemaBuilder schemaBuilder)
        {
            schemaBuilder.AddTable(new Schema.Abstractions.Models.TableConfig
            {
                Name = "TestTable",
                Columns = new List<Column>
                {
                    new Column
                    {
                        Name="column1",
                        DataType=System.Data.DbType.String
                    }
                }
            });
        }

    }

    public sealed class Migration2 : BaseSchemaMigrator
    {
        public override string Name => "migration2";
    }

    public sealed class Migration3 : BaseSchemaMigrator
    {
        public override string Name => "migration3";
    }
}