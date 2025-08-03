namespace MinPlatform.Data.Service.Models
{
    public sealed class PageInfo
    {
        public int PageSize 
        { 
            get;
            set;
        }

        public int PageNumber 
        { 
            get;
            set; 
        }

        public bool ReturnTotalCount
        {
            get;
            set;
        }
    }
}
