using MinPlatform.Data.Service.DataAttributes;

namespace MinPlatform.Data.Service.Extensions
{
    public static class ConditionalOperatorExtensions
    {
        public static string ToOperator(this ConditionalOperator conditionalOperator)
        {
            var fieldInfo = conditionalOperator.GetType().GetField(conditionalOperator.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(OperatorAttribute), false) as OperatorAttribute[];

            if (attributes.Length > 0)
            {
                return attributes[0].Operator;
            }

            return null;
        }
    }
}
