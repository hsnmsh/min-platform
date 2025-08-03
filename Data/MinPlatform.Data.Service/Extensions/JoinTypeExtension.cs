namespace MinPlatform.Data.Service.Extensions
{
    using MinPlatform.Data.Service.DataAttributes;

    public static class JoinTypeExtension
    {
        public static string ToJoinType(this JoinType joinOperator)
        {
            var fieldInfo = joinOperator.GetType().GetField(joinOperator.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(JoinTypeAttribute), false) as JoinTypeAttribute[];

            if (attributes.Length > 0)
            {
                return attributes[0].JoinType;
            }

            return null;
        }
    }
}
