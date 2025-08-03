#region License
//
// Copyright (c) 2018, Fluent Migrator Project
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

namespace MinPlatform.Schema.Migrators.Runner
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using MinPlatform.Schema.Migrators.Runner.Conventions;
    using MinPlatform.Schema.Migrators.Runner.Exceptions;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;
    using MinPlatform.Schema.Migrators.Abstractions.Validation;
    using MinPlatform.Schema.Migrators.Abstractions;
    using MinPlatform.Schema.Migrators.Abstractions.Expressions;
    using MinPlatform.Logging.Service;

    public class MigrationValidator
    {
        [CanBeNull]
        private readonly LoggerManager logger;

        [CanBeNull]
        private readonly IConventionSet conventions;

        [NotNull]
        private readonly IMigrationExpressionValidator validator;

        public MigrationValidator(
            LoggerManager logger,
            [NotNull] IConventionSet conventions,
            [CanBeNull] IMigrationExpressionValidator validator = null)
        {
            this.logger = logger;
            this.conventions = conventions;
            this.validator = validator ?? new DefaultMigrationExpressionValidator();
        }

        /// <summary>
        /// Validates each migration expression that has implemented the ICanBeValidated interface.
        /// It throws an InvalidMigrationException exception if validation fails.
        /// </summary>
        /// <param name="migration">The current migration being run</param>
        /// <param name="expressions">All the expressions contained in the up or down action</param>
        public void ApplyConventionsToAndValidateExpressions(IMigration migration, IEnumerable<IMigrationExpression> expressions)
        {
            var errorMessageBuilder = new StringBuilder();

            foreach (var expression in expressions.Apply(conventions))
            {
                var errors = new Collection<string>();
                foreach (var result in validator.Validate(expression))
                {
                    errors.Add(result.ErrorMessage);
                }

                if (errors.Count > 0)
                {
                    AppendError(errorMessageBuilder, expression.GetType().Name, string.Join(" ", errors.ToArray()));
                }
            }

            if (errorMessageBuilder.Length > 0)
            {
                var errorMessage = errorMessageBuilder.ToString();
                logger?.LogMessageTemplate("The migration {0} contained the following Validation Error(s): {1}", LogLevel.Error, migration.GetType().Name, errorMessage);
                throw new InvalidMigrationException(migration, errorMessage);
            }
        }

        private void AppendError(StringBuilder builder, string expressionType, string errors)
        {
            builder.AppendFormat("{0}: {1}{2}", expressionType, errors, Environment.NewLine);
        }
    }
}
