using System;

namespace MinPlatform.Data.Service.DataAttributes
{
    internal sealed class OperatorAttribute : Attribute
    {
        public string Operator
        {
            get;
        }

        public OperatorAttribute(string @operator)
        {
            Operator = @operator;
        }
    }
}
