namespace MinPlatform.Data.Service.QueryBuilder
{
    using MinPlatform.Data.Service.QueryBuilder.Elements;
    using System.Text;

    public abstract class SqlQueryBuilder
    {
        protected  StringBuilder selectBuilder;
        protected  StringBuilder whereBuilder;
        protected  StringBuilder joinBuilder;
        protected  StringBuilder orderByBuilder;
        protected  StringBuilder groupByBuilder;
        protected  StringBuilder paginationBuilder;
        protected  StringBuilder countBuilder;


        public SqlQueryBuilder()
        {
            InitializeBuilders();
        }

        public abstract void Visit(SelectElement selectElement);
        public abstract void Visit(WhereElement whereElement);
        public abstract void Visit(JoinElement joinElement);
        public abstract void Visit(OrderByElement orderByElement);
        public abstract void Visit(GroupByElement groupByElement);
        public abstract void Visit(PaginationElement paginationElement);
        public abstract void Visit(CountElement paginationElement);

        public void InitializeBuilders()
        {
            selectBuilder = new StringBuilder();
            whereBuilder = new StringBuilder();
            joinBuilder = new StringBuilder();
            orderByBuilder = new StringBuilder();
            groupByBuilder = new StringBuilder();
            paginationBuilder = new StringBuilder();
            countBuilder = new StringBuilder();
        }

        public override string ToString()
        {
            return Build();
        }

        protected virtual string Build()
        {
            return selectBuilder.ToString() +
                joinBuilder.ToString() +
                whereBuilder.ToString() +
                orderByBuilder.ToString() +
                paginationBuilder.ToString() +
                countBuilder.ToString();
        }

  
    }
}
