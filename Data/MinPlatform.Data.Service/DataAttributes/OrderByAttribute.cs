namespace MinPlatform.Data.Service.DataAttributes
{
    using System;

    internal sealed class OrderByAttribute: Attribute
    {
        public string OrderOperator
        {
            get;
        }

        public OrderByAttribute(string @operator)
        {
            OrderOperator = @operator;
        }
    }
}
