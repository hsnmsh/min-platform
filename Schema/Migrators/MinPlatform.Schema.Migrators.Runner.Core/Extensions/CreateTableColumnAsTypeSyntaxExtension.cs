namespace MinPlatform.Schema.Migrators.Runner.Core.Extensions
{
    using MinPlatform.Schema.Abstractions.Models;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Alter.Table;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.Table;
    using System;
    using System.Data;

    public static class CreateTableColumnAsTypeSyntaxExtension
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax BuildDataBaseType(this ICreateTableColumnAsTypeSyntax syntax, Column column)
        {
            ICreateTableColumnOptionOrWithColumnSyntax columnBuilder = null;

            switch (column.DataType)
            {
                case DbType.String:
                    columnBuilder = column.MaxLength.HasValue ? syntax.AsString(column.MaxLength.Value) : syntax.AsString();
                    break;
                case DbType.Int32:
                    columnBuilder = syntax.AsInt32();
                    break;
                case DbType.Int64:
                    columnBuilder = syntax.AsInt64();
                    break;
                case DbType.DateTime:
                    columnBuilder = syntax.AsDateTime();
                    break;
                case DbType.Boolean:
                    columnBuilder = syntax.AsBoolean();
                    break;
                case DbType.Decimal:
                    columnBuilder = syntax.AsDecimal();
                    break;
                case DbType.Double:
                    columnBuilder = syntax.AsDouble();
                    break;
                case DbType.Guid:
                    columnBuilder = syntax.AsGuid();
                    break;
                case DbType.Date:
                    columnBuilder = syntax.AsDate();
                    break;
                case DbType.Byte:
                    columnBuilder = syntax.AsByte();
                    break;
                case DbType.Binary:
                    columnBuilder = syntax.AsBinary();
                    break;
                case DbType.Time:
                    columnBuilder = syntax.AsTime();
                    break;
                case DbType.Currency:
                    columnBuilder = syntax.AsCurrency();
                    break;
                case DbType.Xml:
                    columnBuilder = syntax.AsXml();
                    break;
                case DbType.AnsiString:
                    columnBuilder = syntax.AsAnsiString();
                    break;
                case DbType.Int16:
                    columnBuilder = syntax.AsInt16();
                    break;
                case DbType.UInt64:
                    columnBuilder = syntax.AsInt64();
                    break;
                case DbType.DateTime2:
                    columnBuilder = syntax.AsDateTime2();
                    break;
                case DbType.DateTimeOffset:
                    columnBuilder = syntax.AsDateTimeOffset();
                    break;
                case DbType.SByte:
                    columnBuilder = syntax.AsByte();
                    break;
                default:
                    throw new InvalidOperationException($" {column.DataType} is Unsupported DbType");
            }

            if (column.IsPrimaryKey)
            {
                columnBuilder.PrimaryKey();
            }

            if (column.IsUnique != null && column.IsUnique.Value)
            {
                columnBuilder.Unique();
            }

            if (column.Promoted != null && column.Promoted.Value)
            {
                columnBuilder.Indexed($"IX_{column.Name}");
            }

            if (column.AllowNull != null && !column.AllowNull.Value)
            {
                columnBuilder.NotNullable();
            }
            else
            {
                columnBuilder.Nullable();
            }

            if (column.IsAutoIncrement != null && column.IsAutoIncrement.Value)
            {
                columnBuilder.Identity();
            }

            if (column.DefaultValue != null)
            {
                columnBuilder.WithDefaultValue(column.DefaultValue);
            }

            return columnBuilder;
        }

        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax BuildDataBaseType(this IAlterTableColumnAsTypeSyntax syntax, Column column)
        {
            IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax columnBuilder = null;

            switch (column.DataType)
            {
                case DbType.String:
                    columnBuilder = column.MaxLength.HasValue ? syntax.AsString(column.MaxLength.Value) : syntax.AsString();
                    break;
                case DbType.Int32:
                    columnBuilder = syntax.AsInt32();
                    break;
                case DbType.Int64:
                    columnBuilder = syntax.AsInt64();
                    break;
                case DbType.DateTime:
                    columnBuilder = syntax.AsDateTime();
                    break;
                case DbType.Boolean:
                    columnBuilder = syntax.AsBoolean();
                    break;
                case DbType.Decimal:
                    columnBuilder = syntax.AsDecimal();
                    break;
                case DbType.Double:
                    columnBuilder = syntax.AsDouble();
                    break;
                case DbType.Guid:
                    columnBuilder = syntax.AsGuid();
                    break;
                case DbType.Date:
                    columnBuilder = syntax.AsDate();
                    break;
                case DbType.Byte:
                    columnBuilder = syntax.AsByte();
                    break;
                case DbType.Binary:
                    columnBuilder = syntax.AsBinary();
                    break;
                case DbType.Time:
                    columnBuilder = syntax.AsTime();
                    break;
                case DbType.Currency:
                    columnBuilder = syntax.AsCurrency();
                    break;
                case DbType.Xml:
                    columnBuilder = syntax.AsXml();
                    break;
                case DbType.AnsiString:
                    columnBuilder = syntax.AsAnsiString();
                    break;
                case DbType.Int16:
                    columnBuilder = syntax.AsInt16();
                    break;
                case DbType.UInt64:
                    columnBuilder = syntax.AsInt64();
                    break;
                case DbType.DateTime2:
                    columnBuilder = syntax.AsDateTime2();
                    break;
                case DbType.DateTimeOffset:
                    columnBuilder = syntax.AsDateTimeOffset();
                    break;
                case DbType.SByte:
                    columnBuilder = syntax.AsByte();
                    break;
                default:
                    throw new InvalidOperationException($" {column.DataType} is Unsupported DbType");
            }

            if (column.IsPrimaryKey)
            {
                columnBuilder.PrimaryKey();
            }

            if (column.IsUnique != null && column.IsUnique.Value)
            {
                columnBuilder.Unique();
            }

            if (column.Promoted != null && column.Promoted.Value)
            {
                columnBuilder.Indexed($"IX_{column.Name}");
            }

            if (column.AllowNull != null && !column.AllowNull.Value)
            {
                columnBuilder.NotNullable();
            }
            else
            {
                columnBuilder.Nullable();
            }

            if (column.IsAutoIncrement != null && column.IsAutoIncrement.Value)
            {
                columnBuilder.Identity();
            }

            if (column.DefaultValue != null)
            {
                columnBuilder.WithDefaultValue(column.DefaultValue);
            }

            return columnBuilder;
        }
    }
}
