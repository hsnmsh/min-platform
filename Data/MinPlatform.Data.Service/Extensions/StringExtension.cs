namespace MinPlatform.Data.Service.Extensions
{
    public static class StringExtension
    {
        public static string ToInputParameter(this string value)
        {
            return value.Replace(".", "_").ToLower();
        }

        public static string ToInputParameter(this string value, int conditionNumber)
        {
            return value.Replace(".", "_").ToLower() + "_condition_" + conditionNumber;
        }

        public static string ToInputParameter(this string value, int conditionNumber, int paramCounter)
        {
            return value.Replace(".", "_").ToLower() + "_condition_" + conditionNumber + "_" + paramCounter;
        }
    }
}
