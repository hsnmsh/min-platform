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

namespace MinPlatform.Schema.Migrators.Abstractions.Expressions
{
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;

    /// <summary>
    /// Expression to execute SQL scripts
    /// </summary>
    public class ExecuteSqlScriptExpression : ExecuteSqlScriptExpressionBase, IFileSystemExpression
    {
        private string rootPath;
        private string sqlScript;
        private string unchangedSqlScript;

        /// <summary>
        /// Gets or sets the SQL script to be executed
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.SqlScriptCannotBeNullOrEmpty))]
        public string SqlScript
        {
            get => sqlScript;
            set
            {
                unchangedSqlScript =  value;
                UpdateSqlScript();
            }
        }

        /// <summary>
        /// Gets or sets the root path where the SQL script file should be loaded from
        /// </summary>
        public string RootPath
        {
            get => rootPath;
            set
            {
                rootPath = value;
                UpdateSqlScript();
            }
        }

        /// <inheritdoc />
        public override void ExecuteWith(IMigrationProcessor processor)
        {
            string sqlText;
            using (var reader = File.OpenText(SqlScript))
            {
                sqlText = reader.ReadToEnd();
            }

            Execute(processor, sqlText);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return base.ToString() + SqlScript;
        }

        private void UpdateSqlScript()
        {
            if (!string.IsNullOrEmpty(rootPath) && !string.IsNullOrEmpty(unchangedSqlScript))
            {
                sqlScript = Path.Combine(rootPath, unchangedSqlScript);
            }
            else
            {
                sqlScript = unchangedSqlScript;
            }
        }
    }
}
