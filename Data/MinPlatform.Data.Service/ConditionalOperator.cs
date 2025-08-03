namespace MinPlatform.Data.Service
{
    using MinPlatform.Data.Service.DataAttributes;

    public enum ConditionalOperator
    {
        [Operator("=")]
        Equal,
        [Operator("LIKE")]
        Like,
        [Operator("<>")]
        NotEqual,
        [Operator("<")]
        LessThan,
        [Operator("<=")]
        LessThanOrEqual,
        [Operator(">")]
        GreaterThan,
        [Operator(">=")]
        GreaterThanOrEqual,
        [Operator("BETWEEN")]
        Between,
        [Operator("NOT BETWEEN")]
        NotBetween,
        [Operator("IN")]
        IN,
        [Operator("NOT IN")]
        NotIN,
        [Operator("IS NULL")]
        IsNull,
        [Operator("IS NOT NULL")]
        IsNotNull,
    }
}
