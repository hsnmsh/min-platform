namespace MinPlatform.Data.Service.Extensions
{
    using MinPlatform.Data.Service.DataAttributes;

    public static class OrderByExtension
    {
        public static string ToOrderOperator(this OrderOperator orderOperator)
        {
            var fieldInfo = orderOperator.GetType().GetField(orderOperator.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(OrderByAttribute), false) as OrderByAttribute[];

            if (attributes.Length > 0)
            {
                return attributes[0].OrderOperator;
            }

            return null;
        }
    }
}
