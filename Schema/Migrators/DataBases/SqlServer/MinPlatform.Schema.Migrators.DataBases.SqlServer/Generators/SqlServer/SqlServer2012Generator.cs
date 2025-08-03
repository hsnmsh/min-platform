#region License
//
// Copyright (c) 2007-2024, Fluent Migrator Project
// Copyright (c) 2012, Daniel Lee
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

using System.Text;
using JetBrains.Annotations;
using MinPlatform.Schema.Migrators.Abstractions.Expressions;
using MinPlatform.Schema.Migrators.Runner.Generators;
using MinPlatform.Schema.Migrators.Runner.Initialization;

namespace MinPlatform.Schema.Migrators.DataBases.SqlServer.Generators.SqlServer
{
    public class SqlServer2012Generator : SqlServer2008Generator
    {
        public SqlServer2012Generator(IConnectionStringAccessor connectionStringAccessor)
            : this(new SqlServer2008Quoter(), connectionStringAccessor)
        {
        }

        public SqlServer2012Generator(
            [NotNull] SqlServer2008Quoter quoter,
            IConnectionStringAccessor connectionStringAccessor)
            : base(quoter, new GeneratorOptions(), connectionStringAccessor)
        {
        }

        public SqlServer2012Generator(
            [NotNull] SqlServer2008Quoter quoter,
            GeneratorOptions generatorOptions,
            IConnectionStringAccessor connectionStringAccessor)
            : base(quoter, generatorOptions, connectionStringAccessor)
        {
        }

        protected SqlServer2012Generator(
            [NotNull] IColumn column,
            [NotNull] IQuoter quoter,
            [NotNull] IDescriptionGenerator descriptionGenerator,
            [NotNull] GeneratorOptions generatorOptions,
            IConnectionStringAccessor connectionStringAccessor)
            : base(column, quoter, descriptionGenerator, generatorOptions, connectionStringAccessor)
        {
        }

        public override string Generate(CreateSequenceExpression expression)
        {
            var result = new StringBuilder("CREATE SEQUENCE ");
            var seq = expression.Sequence;
            result.Append(Quoter.QuoteSequenceName(seq.Name, seq.SchemaName));

            if (seq.Increment.HasValue)
            {
                result.AppendFormat(" INCREMENT BY {0}", seq.Increment);
            }

            if (seq.MinValue.HasValue)
            {
                result.AppendFormat(" MINVALUE {0}", seq.MinValue);
            }

            if (seq.MaxValue.HasValue)
            {
                result.AppendFormat(" MAXVALUE {0}", seq.MaxValue);
            }

            if (seq.StartWith.HasValue)
            {
                result.AppendFormat(" START WITH {0}", seq.StartWith);
            }

            const long MINIMUM_CACHE_VALUE = 2;
            if (seq.Cache.HasValue)
            {
                if (seq.Cache.Value < MINIMUM_CACHE_VALUE)
                {
                    return CompatibilityMode.HandleCompatibility("Cache size must be greater than 1; if you intended to disable caching, set Cache to null.");
                }
                result.AppendFormat(" CACHE {0}", seq.Cache);
            }
            else
            {
                result.Append(" NO CACHE");
            }

            if (seq.Cycle)
            {
                result.Append(" CYCLE");
            }

            return result.ToString();
        }

        public override string Generate(DeleteSequenceExpression expression)
        {
            return $"DROP SEQUENCE {Quoter.QuoteSequenceName(expression.SequenceName, expression.SchemaName)}";
        }
    }
}
