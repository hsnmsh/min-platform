namespace MinPlatform.Data.Service.Models
{
    using System.Collections.Generic;

    public sealed class PagedItems<T>
    {
        public IEnumerable<T> Items 
        { 
            get;
            set;
        }

        public int? PageIndex
        {
            get;
            set;
        }

        public int? PageSize
        {
            get;
            set;
        }

        public int? TotalCount
        {
            get;
            set;
        }
    }
}
