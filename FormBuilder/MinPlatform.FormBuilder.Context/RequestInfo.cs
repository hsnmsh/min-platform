namespace MinPlatform.FormBuilder.Context
{
    using System;
    using System.Collections.Generic;

    public sealed class RequestInfo
    {
        public IDictionary<string, object> Properties
        {
            get;
            set;
        }

        public IEnumerable<FileRequestInfo> Files
        {
            get;
            set;
        }
    }

    public sealed class FileRequestInfo
    {
        public string Id
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public string Extension
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public byte[] FileContent
        {
            get;
            set;
        }
    }
}
