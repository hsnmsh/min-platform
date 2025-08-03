namespace MinPlatform.Data.Service
{
    using MinPlatform.Data.Service.DataAttributes;

    public enum LogicalOperator
    {
        [LogicalOperator("AND")]
        And,
        [LogicalOperator("OR")]
        Or,
        [LogicalOperator("NOT")]
        Not
    }
}
