#region License
//
// Copyright (c) 2007-2024, Fluent Migrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace MinPlatform.Schema.Migrators.Builders.Insert
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Insert;
    using MinPlatform.Schema.Migrators.Abstractions.Expressions;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Abstractions.Model;

    /// <summary>
    /// An expression builder for a <see cref="InsertDataExpression"/>
    /// </summary>
    public class InsertDataExpressionBuilder : IInsertDataOrInSchemaSyntax, ISupportAdditionalFeatures
    {
        private readonly InsertDataExpression expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertDataExpressionBuilder"/> class.
        /// </summary>
        /// <param name="expression">The underlying expression</param>
        public InsertDataExpressionBuilder(InsertDataExpression expression)
        {
            this.expression = expression;
        }

        /// <inheritdoc />
        public IDictionary<string, object> AdditionalFeatures => expression.AdditionalFeatures;

        /// <inheritdoc />
        public IInsertDataSyntax Row(object dataAsAnonymousType)
        {
            IDictionary<string, object> data = ExtractData(dataAsAnonymousType);

            return Row(data);
        }

        /// <inheritdoc />
        public IInsertDataSyntax Row(IDictionary<string, object> data)
        {
            var dataDefinition = new InsertionDataDefinition();

            dataDefinition.AddRange(data);

            expression.Rows.Add(dataDefinition);

            return this;
        }

        /// <inheritdoc />
        public IInsertDataSyntax InSchema(string schemaName)
        {
            expression.SchemaName = schemaName;
            return this;
        }

        private static IDictionary<string, object> ExtractData(object dataAsAnonymousType)
        {
            var data = new Dictionary<string, object>();

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(dataAsAnonymousType);

            foreach (PropertyDescriptor property in properties)
            {
                data.Add(property.Name, property.GetValue(dataAsAnonymousType));
            }

            return data;
        }
    }
}
