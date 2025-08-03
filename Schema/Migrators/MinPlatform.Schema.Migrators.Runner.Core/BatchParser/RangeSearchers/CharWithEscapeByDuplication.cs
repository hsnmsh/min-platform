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

namespace MinPlatform.Schema.Migrators.Runner.BatchParser.RangeSearchers
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Utility class that handles single-character ranges (e.g. <c>'text'</c>) where the
    /// end characters might be duplicated to escape it.
    /// </summary>
    public class CharWithEscapeByDuplication : IRangeSearcher
    {
        private readonly char endChar;
        private readonly Regex startCodeRegex;
        private readonly Regex endCodeRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharWithEscapeByDuplication"/> class.
        /// </summary>
        /// <param name="startAndEndChar">The character used for start and end</param>
        /// <param name="isComment">Is this a comment</param>
        public CharWithEscapeByDuplication(char startAndEndChar, bool isComment = false)
        {
            IsComment = isComment;
            endChar = startAndEndChar;
            var codePattern = Regex.Escape(startAndEndChar.ToString());
            startCodeRegex = endCodeRegex =
                new Regex(codePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharWithEscapeByDuplication"/> class.
        /// </summary>
        /// <param name="startChar">The start character</param>
        /// <param name="endChar">The end character</param>
        /// <param name="isComment">Is this a comment</param>
        public CharWithEscapeByDuplication(char startChar, char endChar, bool isComment = false)
        {
            IsComment = isComment;
            this.endChar = endChar;
            var startCodePattern = Regex.Escape(startChar.ToString());
            var endCodePattern = Regex.Escape(endChar.ToString());
            startCodeRegex = new Regex(startCodePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
            endCodeRegex = new Regex(endCodePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        /// <inheritdoc />
        public int StartCodeLength => 1;

        /// <inheritdoc />
        public int EndCodeLength => 1;

        /// <inheritdoc />
        public bool IsComment { get; }

        /// <inheritdoc />
        public int FindStartCode(ILineReader reader)
        {
            var match = startCodeRegex.Match(reader.Line, reader.Index);
            if (!match.Success)
                return -1;
            return match.Index;
        }

        /// <inheritdoc />
        public EndCodeSearchResult FindEndCode(ILineReader reader)
        {
            var lastIndex = reader.Line.Length - 1;
            var startIndex = reader.Index;
            for (; ; )
            {
                var match = endCodeRegex.Match(reader.Line, startIndex);
                if (!match.Success)
                    return null;

                var foundIndex = match.Index;
                if (foundIndex == lastIndex)
                    return foundIndex;

                if (reader.Line[foundIndex + 1] != endChar)
                    return foundIndex;

                startIndex = foundIndex + 2;
            }
        }
    }
}
