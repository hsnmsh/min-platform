namespace MinPlatform.Data.Service.QueryBuilder.Elements
{
    using MinPlatform.Data.Service.Models;
    using System.Collections.Generic;

    public sealed class JoinElement : IQueryElement
    {
        public IList<Join> JoinEntity
        {
            get;
            set;
        }

        public static JoinElement ToJoinElement(IList<Join> JoinEntity)
        {
            return new JoinElement
            {
                JoinEntity = JoinEntity,
            };
        }

        public void Accept(SqlQueryBuilder sqlQueryBuilder)
        {
            sqlQueryBuilder.Visit(this);
        }
    }
}
