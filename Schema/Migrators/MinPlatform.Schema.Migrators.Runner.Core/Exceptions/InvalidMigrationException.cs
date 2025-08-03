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


namespace MinPlatform.Schema.Migrators.Runner.Exceptions
{
    using MinPlatform.Schema.Migrators.Abstractions;

    public class InvalidMigrationException : RunnerException
    {
        private readonly IMigration migration;
        private readonly string errors;

        public InvalidMigrationException(IMigration migration, string errors)
        {
            this.migration = migration;
            this.errors = errors;
        }

        public override string Message
        {
            get
            {
                return string.Format("The migration {0} contained the following Validation Error(s): {1}", migration.GetType().Name, errors);
            }
        }
    }
}
