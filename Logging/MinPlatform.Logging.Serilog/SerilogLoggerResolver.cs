using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using Serilog;

namespace MinPlatform.Logging.Serilog
{
    using MinPlatform.Logging.Abstractions;
    using MinPlatform.Logging.Abstractions.Models;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class SerilogLoggerResolver : ILoggerResolver<ILogger>
    {
        public ILogger ResolveLogger(LoggingConfig config)
        {
            ILogger logger = null;

            if (config is null ||
                config.LoggingProperties is null ||
                config.LoggingProperties is ConsoleProperties)
            {
                logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .CreateLogger();
            }
            else if (config.LoggingProperties is FileProperties file)
            {
                logger = new LoggerConfiguration()
                        .WriteTo.File(string.IsNullOrEmpty(file.Path) ? "logs/app-logs.txt" : file.Path, rollingInterval: RollingInterval.Day)
                        .CreateLogger();
            }
            else if (config.LoggingProperties is DataBaseProperties dataBaseProperties)
            {
                var columnOptions = new ColumnOptions();

                if (dataBaseProperties is null)
                {
                    dataBaseProperties.TableInfo = new TableInfo();
                }

                if (dataBaseProperties?.TableInfo?.Columns != null && dataBaseProperties.TableInfo.Columns.Any())
                {
                    var additionalColumns = new List<SqlColumn>();

                    foreach (var columnInfo in dataBaseProperties.TableInfo.Columns)
                    {
                        additionalColumns.Add(new SqlColumn
                        {
                            ColumnName = columnInfo.Name,
                            PropertyName = columnInfo.Name,
                            DataType = columnInfo.DataType,
                            DataLength = columnInfo.Size == 0 ? 20 : columnInfo.Size
                        });
                    }

                    columnOptions.AdditionalColumns = additionalColumns;
                }

                logger = new LoggerConfiguration()
                        .WriteTo
                        .MSSqlServer(
                            connectionString: dataBaseProperties.ConnectionString,
                            columnOptions: columnOptions,
                            sinkOptions: new MSSqlServerSinkOptions
                            {
                                TableName = dataBaseProperties.TableInfo.Name,
                                AutoCreateSqlTable = dataBaseProperties.AutomaticTableCreation,
                                SchemaName = "MinPlatform",

                            })
                        .CreateLogger();
            }
            else if (config.LoggingProperties is SeqProperties seq)
            {
                var levelSwitch = new LoggingLevelSwitch();

                logger = new LoggerConfiguration()
                         .WriteTo.Seq(seq.Url, apiKey: !string.IsNullOrEmpty(seq.ApiKey) ? seq.ApiKey : null, controlLevelSwitch: levelSwitch)
                         .CreateLogger();
            }

            return logger;
        }
    }
}
