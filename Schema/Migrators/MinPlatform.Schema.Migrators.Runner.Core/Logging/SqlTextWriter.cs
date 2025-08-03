#region License
// Copyright (c) 2018, Fluent Migrator Project
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

namespace MinPlatform.Schema.Migrators.Runner.Logging
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// A <see cref="TextWriter"/> implementation that puts everything into multi-line comments
    /// </summary>
    internal class SqlTextWriter : TextWriter
    {
        private readonly TextWriter innerWriter;

        public SqlTextWriter(TextWriter innerWriter)
        {
            this.innerWriter = innerWriter;
        }

        /// <inheritdoc />
        public override Encoding Encoding => innerWriter.Encoding;

        /// <inheritdoc />
        public override void WriteLine(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                innerWriter.WriteLine($"/* {value} */");
            }
            else
            {
                innerWriter.WriteLine();
            }

            innerWriter.Flush();
        }

        public void WriteLineDirect(string message)
        {
            innerWriter.WriteLine(message);

            innerWriter.Flush();
        }
    }
}
