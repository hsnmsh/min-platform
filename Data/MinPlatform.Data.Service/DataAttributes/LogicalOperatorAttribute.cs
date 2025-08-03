namespace MinPlatform.Data.Service.DataAttributes
{
    using System;

    internal class LogicalOperatorAttribute : Attribute
    {
        public string LogicalOperator
        {
            get;
        }

        public LogicalOperatorAttribute(string @operator)
        {
            LogicalOperator = @operator;
        }
    }
}
