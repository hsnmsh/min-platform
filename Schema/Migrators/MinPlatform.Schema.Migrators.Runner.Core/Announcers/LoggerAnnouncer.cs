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

namespace MinPlatform.Schema.Migrators.Runner.Announcers
{
    using JetBrains.Annotations;
    using MinPlatform.Logging.Service;
    using System;

#pragma warning disable CS0612 // Type or member is obsolete
    public class LoggerAnnouncer : IAnnouncer
#pragma warning restore CS0612 // Type or member is obsolete
    {
        [NotNull]
        private readonly LoggerManager logger;

        [NotNull]
#pragma warning disable CS0612 // Type or member is obsolete
        private readonly AnnouncerOptions options;
#pragma warning restore CS0612 // Type or member is obsolete


#pragma warning disable CS0612 // Type or member is obsolete
        public LoggerAnnouncer(LoggerManager logger, AnnouncerOptions options)
#pragma warning restore CS0612 // Type or member is obsolete
        {
            this.logger = logger;
            this.options = options;
        }

        public void Heading(string message)
        {
            logger.Information(message);
        }

        /// <inheritdoc />
        public void Say(string message)
        {
            logger.Information(message);
        }

        /// <inheritdoc />
        public void Emphasize(string message)
        {
            logger.Information(message);
        }

        /// <inheritdoc />
        public void Sql(string sql)
        {
            if (options.ShowSql)
                logger.Information(sql);
        }

        /// <inheritdoc />
        public void ElapsedTime(TimeSpan timeSpan)
        {
            if (options.ShowElapsedTime)
            {
                logger.Information(timeSpan.ToString());
            }
        }

        /// <inheritdoc />
        public void Error(string message)
        {
            logger.Error(message);
        }

        /// <inheritdoc />
        public void Error(Exception exception)
        {
            logger.Error(exception.Message);
        }

        /// <inheritdoc />
        public void Write(string message, bool isNotSql = true)
        {
            if (isNotSql)
            {
                Say(message);
            }
            else if (options.ShowSql)
            {
                Sql(message);
            }
        }

        
    }
}
