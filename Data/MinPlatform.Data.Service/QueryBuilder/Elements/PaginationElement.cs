namespace MinPlatform.Data.Service.QueryBuilder.Elements
{
    public sealed class PaginationElement : IQueryElement
    {
        public int PageSize 
        { 
            get;
            set; 
        }

        public int PageIndex
        {
            get;
            set;
        }

        
        public static PaginationElement ToPaginationElement(int  pageSize, int pageIndex)
        {
            return new PaginationElement
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
            };
        }

        public void Accept(SqlQueryBuilder sqlQueryBuilder)
        {
            sqlQueryBuilder.Visit(this);
        }
    }
}
