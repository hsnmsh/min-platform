namespace MinPlatform.Data.Sql
{
    using MinPlatform.Data.Service.Extensions;
    using MinPlatform.Data.Service.Models;
    using MinPlatform.Data.Service.QueryBuilder;
    using MinPlatform.Data.Service.QueryBuilder.Elements;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class SqlServerQueryBuilder : SqlQueryBuilder
    {
        public override void Visit(SelectElement selectElement)
        {
            selectBuilder.Append("SELECT ");

            if (selectElement.IsDistinct)
            {
                selectBuilder.Append("DISTINCT ");
            }

            if (selectElement.SingleRow)
            {
                selectBuilder.Append("TOP 1 ");
            }

            selectBuilder
                .Append(selectElement.Columns == null || selectElement.Columns.Count() == 0 ? "mainTable.*" :
                "mainTable.Id, " + string.Join(',', selectElement.Columns.Select(column => !column.Contains('.') ? "mainTable." + column : column + " AS " + column.Replace(".", "_"))))
                .AppendLine(" FROM " + selectElement.Entity + " AS mainTable ");
        }

        public override void Visit(WhereElement whereElement)
        {
            if (whereElement.Conditions != null && whereElement.Conditions.Any())
            {
                whereBuilder.Append("WHERE ");

                int conditionNumber = 1;

                foreach (ConditionGroup groupCondition in whereElement.Conditions)
                {
                    int paramCounter = 1;

                    foreach (Condition condition in groupCondition.Condition)
                    {
                        whereBuilder.Append(GenerateQueryLogic(condition, ref conditionNumber, ref paramCounter));

                        if (!groupCondition.Condition.IsLastItem(condition))
                        {
                            whereBuilder
                                .Append(groupCondition.Operator != null ? groupCondition.Operator?.ToLogicalOperator() + " " : " ");
                        }

                        paramCounter++;

                    }

                    conditionNumber++;

                    if (!whereElement.Conditions.IsLastItem(groupCondition))
                    {
                        whereBuilder.AppendLine(" " + whereElement.LogicalOperator.ToLogicalOperator() + " ");
                    }

                }
            }

        }

        public override void Visit(JoinElement joinElement)
        {

            if (joinElement.JoinEntity != null && joinElement.JoinEntity.Any())
            {
                foreach (Join join in joinElement.JoinEntity)
                {
                    joinBuilder.AppendLine(join.JoinType.ToJoinType())
                        .Append(" JOIN ")
                        .Append(!string.IsNullOrEmpty(join.TargetEntity.Alias) ? join.TargetEntity.Name + " AS " + join.TargetEntity.Alias : join.TargetEntity.Name)
                        .Append(" ON ")
                        .Append((join.SourceEntity is null || string.IsNullOrEmpty(join.SourceEntity.Name)) ? "mainTable" : !string.IsNullOrEmpty(join.SourceEntity.Alias) ? join.SourceEntity.Alias : join.SourceEntity.Name)
                        .Append(".")
                        .Append(join.SourceEntity.ColumnName)
                        .Append("=")
                        .Append(!string.IsNullOrEmpty(join.TargetEntity.Alias) ? join.TargetEntity.Alias : join.TargetEntity.Name)
                        .Append(".")
                        .Append(join.TargetEntity.ColumnName)
                        .Append(' ', 1);

                }
            }
        }

        public override void Visit(OrderByElement orderByElement)
        {
            if (orderByElement.Columns != null)
            {
                orderByBuilder
                    .AppendLine("ORDER BY " + string.Join(',', orderByElement.Columns))
                    .Append(' ', 1)
                    .Append(orderByElement.Order.ToOrderOperator());

            }
        }

        public override void Visit(GroupByElement groupByElement)
        {
            if (groupByElement.GroupBy != null)
            {
                orderByBuilder
                    .Append(" GROUP BY ")
                    .Append(string.Join(',', groupByElement.GroupBy));

            }
        }

        public override void Visit(PaginationElement paginationElement)
        {
            paginationBuilder.Append(" OFFSET (@offsetValue * @pageSize) ROWS FETCH NEXT @pageSize ROWS ONLY");
        }

        public override void Visit(CountElement paginationElement)
        {
            countBuilder
                .Append(' ', 1)
                .Append(" (SELECT COUNT(*) AS totalCount FROM ")
                .Append(paginationElement.TableName)
                .Append(" AS mainTable ")
                .AppendLine(joinBuilder.ToString())
                .AppendLine(whereBuilder.ToString())
                .Append(") ");

        }

        private string GenerateQueryLogic(Condition condition, ref int conditionNumber, ref int paramCounter)
        {
            var logicBuilder = new StringBuilder();
            logicBuilder.Append((!condition.Property.Contains('.') ? "mainTable." + condition.Property : condition.Property) + " " + condition.ConditionalOperator.ToOperator() + " ");

            switch (condition.ConditionalOperator)
            {
                case Service.ConditionalOperator.Like:
                    logicBuilder.Append("'%" + condition.Value + "%' ");
                    break;

                case Service.ConditionalOperator.Between:
                case Service.ConditionalOperator.NotBetween:

                    if (condition.Value is string[] stringValue)
                    {
                        logicBuilder.Append(" '" + stringValue[0] + "' AND '" + stringValue[1] + "' ");
                    }
                    else
                    {
                        var selectedValues = (object[])condition.Value;

                        logicBuilder.Append(selectedValues[0] + " AND " + selectedValues[1]);
                    }

                    break;

                case Service.ConditionalOperator.IN:
                case Service.ConditionalOperator.NotIN:

                    if (condition.Value is IEnumerable<string> stringValues)
                    {
                        logicBuilder.Append(" (" + string.Join(",", stringValues.Select(v => "'" + v + "'")) + ") ");

                    }
                    else
                    {
                        var selectedInValues = (IEnumerable<object>)condition.Value;

                        logicBuilder.Append(" (" + string.Join(",", selectedInValues) + ") ");
                    }

                    break;

                case Service.ConditionalOperator.IsNull:
                case Service.ConditionalOperator.IsNotNull:

                    return logicBuilder.ToString();

                default:
                    logicBuilder.Append("@" + condition.Property.ToInputParameter(conditionNumber, paramCounter) + " ");
                    break;

            }

            return logicBuilder.ToString();
        }
    }
}
