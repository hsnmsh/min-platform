namespace MinPlatform.Data.Service.DataAttributes
{
    using System;

    internal sealed class JoinTypeAttribute : Attribute
    {
        public string JoinType
        {
            get;
        }

        public JoinTypeAttribute(string joinType)
        {
            JoinType = joinType;
        }
    }
}
