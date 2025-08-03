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

namespace MinPlatform.Schema.Migrators.DataBases.SqlServer.SqlServer
{
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.Constraint;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.Index;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure.Extensions;
    using MinPlatform.Schema.Migrators.DataBases.SqlServer.Builders.Create.Index;
    using MinPlatform.Schema.Migrators.DataBases.SqlServer.Model;
    using System;
    using System.Collections.Generic;

    public static partial class SqlServerExtensions
    {
        public static ICreateIndexOptionsSyntax Include(this ICreateIndexOptionsSyntax expression, string columnName)
        {
            var additionalFeatures = expression as ISupportAdditionalFeatures;
            additionalFeatures.CreateIndexInclude(columnName);
            return expression;
        }

        public static ICreateIndexNonKeyColumnSyntax Include(this ICreateIndexOnColumnSyntax expression, string columnName)
        {
            var additionalFeatures = expression as ISupportAdditionalFeatures;
            additionalFeatures.CreateIndexInclude(columnName);
            return new CreateIndexExpressionNonKeyBuilder(expression, additionalFeatures);
        }

        internal static void CreateIndexInclude(this ISupportAdditionalFeatures additionalFeatures, string columnName)
        {
            if (additionalFeatures == null)
                throw new InvalidOperationException(UnsupportedMethodMessage(nameof(CreateIndexInclude), nameof(ISupportAdditionalFeatures)));
            var includes = additionalFeatures.GetAdditionalFeature<IList<IndexIncludeDefinition>>(IncludesList, () => new List<IndexIncludeDefinition>());
            includes.Add(new IndexIncludeDefinition { Name = columnName });
        }

        public static ICreateConstraintOptionsSyntax Include(this ICreateConstraintOptionsSyntax expression, string columnName)
        {
            var additionalFeatures = expression as ISupportAdditionalFeatures;
            additionalFeatures.CreateUniqueConstraintInclude(columnName);
            return expression;
        }

        internal static void CreateUniqueConstraintInclude(this ISupportAdditionalFeatures additionalFeatures, string columnName)
        {
            if (additionalFeatures == null)
                throw new InvalidOperationException(UnsupportedMethodMessage(nameof(CreateUniqueConstraintInclude), nameof(ISupportAdditionalFeatures)));
            var includes = additionalFeatures.GetAdditionalFeature<IList<IndexIncludeDefinition>>(IncludesList, () => new List<IndexIncludeDefinition>());
            includes.Add(new IndexIncludeDefinition { Name = columnName });
        }
    }
}
