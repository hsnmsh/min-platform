namespace MinPlatform.Data.Service.Models
{
    using System.Collections.Generic;

    public sealed class OrderBy
    {
        public IList<string> Columns 
        {
            get;
            set;
        }

        public OrderOperator Order 
        { 
            get;
            set;
        }
    }
}
