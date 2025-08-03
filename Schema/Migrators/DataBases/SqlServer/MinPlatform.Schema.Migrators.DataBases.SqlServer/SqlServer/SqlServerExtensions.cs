#region License
// Copyright (c) 2007-2024, Fluent Migrator Project
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

using MinPlatform.Schema.Migrators.Abstractions.Builders;
using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.Constraint;
using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.Table;
using MinPlatform.Schema.Migrators.Abstractions.Builders.Insert;
using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
using System;



namespace MinPlatform.Schema.Migrators.DataBases.SqlServer.SqlServer
{
    public static partial class SqlServerExtensions
    {
        public static readonly string IdentityInsert = "SqlServerIdentityInsert";
        public static readonly string IdentitySeed = "SqlServerIdentitySeed";
        public static readonly string IdentityIncrement = "SqlServerIdentityIncrement";
        public static readonly string ConstraintType = "SqlServerConstraintType";
        public static readonly string IncludesList = "SqlServerIncludes";
        public static readonly string OnlineIndex = "SqlServerOnlineIndex";
        public static readonly string IndexFilter = "SqlServerIndexFilter";
        public static readonly string RowGuidColumn = "SqlServerRowGuidColumn";
        public static readonly string IndexColumnNullsDistinct = "SqlServerIndexColumnNullsDistinct";
        public static readonly string SchemaAuthorization = "SqlServerSchemaAuthorization";
        public static readonly string SparseColumn = "SqlServerSparseColumn";
        public static readonly string UniqueConstraintFilter = "SqlServerUniqueConstraintFilter";
        public static readonly string UniqueConstraintIncludesList = "SqlServerUniqueConstraintIncludes";

        /// <summary>
        /// Inserts data using Sql Server's IDENTITY INSERT feature.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IInsertDataSyntax WithIdentityInsert(this IInsertDataSyntax expression)
        {
            var castExpression = expression as ISupportAdditionalFeatures ??
                throw new InvalidOperationException(UnsupportedMethodMessage(nameof(WithIdentityInsert), nameof(ISupportAdditionalFeatures)));
            castExpression.AdditionalFeatures[IdentityInsert] = true;
            return expression;
        }

        private static void SetConstraintType(ICreateConstraintOptionsSyntax expression, SqlServerConstraintType type)
        {
            if (!(expression is ISupportAdditionalFeatures additionalFeatures))
                throw new InvalidOperationException(UnsupportedMethodMessage(type, nameof(ISupportAdditionalFeatures)));

            additionalFeatures.AdditionalFeatures[ConstraintType] = type;
        }

        public static ICreateConstraintOptionsSyntax Clustered(this ICreateConstraintOptionsSyntax expression)
        {
            SetConstraintType(expression, SqlServerConstraintType.Clustered);
            return expression;
        }

        public static ICreateConstraintOptionsSyntax NonClustered(this ICreateConstraintOptionsSyntax expression)
        {
            SetConstraintType(expression, SqlServerConstraintType.NonClustered);
            return expression;
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax RowGuid(this ICreateTableColumnOptionOrWithColumnSyntax expression)
        {
            var columnExpression = expression as IColumnExpressionBuilder ??
                throw new InvalidOperationException(UnsupportedMethodMessage(nameof(RowGuid), nameof(IColumnExpressionBuilder)));
            columnExpression.Column.AdditionalFeatures[RowGuidColumn] = true;
            return expression;
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax Sparse(this ICreateTableColumnOptionOrWithColumnSyntax expression)
        {
            var columnExpression = expression as IColumnExpressionBuilder ??
                throw new InvalidOperationException(UnsupportedMethodMessage(nameof(Sparse), nameof(IColumnExpressionBuilder)));
            columnExpression.Column.AdditionalFeatures[SparseColumn] = true;
            return expression;
        }

        private static string UnsupportedMethodMessage(object methodName, string interfaceName)
        {
            var msg = string.Format(ErrorMessages.MethodXMustBeCalledOnObjectImplementingY, methodName, interfaceName);
            return msg;
        }
    }
}
