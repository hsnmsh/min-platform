using MinPlatform.Data.Service.DataAttributes;

namespace MinPlatform.Data.Service.Extensions
{
    public static class LogicalOperatorExtensions
    {
        public static string ToLogicalOperator(this LogicalOperator logicalOperator)
        {
            var fieldInfo = logicalOperator.GetType().GetField(logicalOperator.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(LogicalOperatorAttribute), false) as LogicalOperatorAttribute[];

            if (attributes.Length > 0)
            {
                return attributes[0].LogicalOperator;
            }

            return null;
        }
    }
}
