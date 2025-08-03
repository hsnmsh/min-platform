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

using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.Constraint;
using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.Index;
using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
using MinPlatform.Schema.Migrators.Abstractions.Infrastructure.Extensions;

namespace MinPlatform.Schema.Migrators.DataBases.SqlServer.SqlServer
{
    public static partial class SqlServerExtensions
    {
        public static ICreateIndexOptionsSyntax Filter(this ICreateIndexOptionsSyntax expression, string filter)
        {
            var additionalFeatures = expression as ISupportAdditionalFeatures;
            additionalFeatures.SetAdditionalFeature(IndexFilter, filter);
            return expression;
        }

        public static ICreateConstraintOptionsSyntax Filter(
            this ICreateConstraintOptionsSyntax expression,
            string filter)
        {
            var additionalFeatures = expression as ISupportAdditionalFeatures;
            additionalFeatures.SetAdditionalFeature(UniqueConstraintFilter, filter);
            return expression;
        }
    }
}
