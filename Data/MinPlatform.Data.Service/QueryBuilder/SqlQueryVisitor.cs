namespace MinPlatform.Data.Service.QueryBuilder
{
    using MinPlatform.Data.Service.Extensions;
    using MinPlatform.Data.Service.Models;
    using MinPlatform.Data.Service.QueryBuilder.Elements;
    using MinPlatform.Data.Service.QueryBuilder.Factory;
    using System.Collections.Generic;

    internal class SqlQueryVisitor
    {
        protected readonly SqlQueryBuilder sqlQueryBuilder;

        public SqlQueryVisitor(ISqlQueryBuilderFactory sqlQueryBuilderFactory)
        {
            sqlQueryBuilder = sqlQueryBuilderFactory.Create();
        }

        public virtual (string, IDictionary<string, object>) GenerateQuery(QueryData queryData, PageInfo pageInfo = null)
        {
            if (queryData.Columns is null)
            {
                queryData.Columns = new List<string>();
            }

            var selectElement = SelectElement.ToSelectElement(queryData.Columns, queryData.Entity, queryData.IsDistinct, queryData.SingleRow);

            selectElement.Accept(sqlQueryBuilder);

            if (queryData.Conditions is null)
            {
                queryData.Conditions = new List<ConditionGroup>();
            }

            var whereElement = WhereElement.ToWhereElement(queryData.Conditions, queryData.LogicalOperator);
            whereElement.Accept(sqlQueryBuilder);

            var joinElement = JoinElement.ToJoinElement(queryData.JoinEntity);
            joinElement.Accept(sqlQueryBuilder);

            if (queryData.OrderBy is null)
            {
                queryData.OrderBy = new OrderBy();
            }

            if ((queryData.OrderBy.Columns is null || queryData.OrderBy.Columns.Count == 0) && pageInfo != null)
            {
                queryData.OrderBy.Columns = new List<string>()
                {
                    "Id"
                };

            }

            var orderByElement = OrderByElement.ToOrderByElement(queryData.OrderBy.Columns, queryData.OrderBy.Order);
            orderByElement.Accept(sqlQueryBuilder);

            var parameters = new Dictionary<string, object>();

            if (pageInfo != null)
            {
                parameters.Add("offsetValue", pageInfo.PageNumber);
                parameters.Add("pageSize", pageInfo.PageSize);

                var paginationElement = PaginationElement.ToPaginationElement(pageInfo.PageSize, pageInfo.PageNumber);
                paginationElement.Accept(sqlQueryBuilder);

                if (pageInfo.ReturnTotalCount)
                {
                    var countElement = CountElement.ToCountElement(queryData.Entity);
                    countElement.Accept(sqlQueryBuilder);
                }
            }

            int conditionNumber = 1;

            foreach (ConditionGroup conditionGroup in queryData.Conditions)
            {
                int paramCounter = 1;

                foreach (Condition condition in conditionGroup.Condition)
                {
                    parameters.Add(condition.Property.ToInputParameter(conditionNumber, paramCounter), condition.Value);
                    paramCounter++;
                }

                conditionNumber++;
            }

            string sqlQuery = sqlQueryBuilder.ToString();
            sqlQueryBuilder.InitializeBuilders();

            return (sqlQuery, parameters);

        }
    }
}
