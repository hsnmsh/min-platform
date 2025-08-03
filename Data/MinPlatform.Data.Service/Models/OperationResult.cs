namespace MinPlatform.Data.Service.Models
{
    using System.Collections.Generic;

    public sealed class OperationResult<T>
    {
        public T Id
        {
            get;
            set;
        }

        public int RowsAffected
        {
            get;
            set;
        }

        public bool Success
        {
            get;
            set;
        }

        public List<string> Errors
        {
            get;
            set;
        }

        public IDictionary<string, object> Properties
        {
            get;
            set;
        }
    }
}
