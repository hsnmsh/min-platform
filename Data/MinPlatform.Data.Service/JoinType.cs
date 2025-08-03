namespace MinPlatform.Data.Service
{
    using MinPlatform.Data.Service.DataAttributes;

    public enum JoinType
    {
        [JoinType("INNER")]
        Inner,
        [JoinType("LEFT")]
        Left,
        [JoinType("RIGHT")]
        Right,
        [JoinType("FULL")]
        Full
    }
}
