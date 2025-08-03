namespace MinPlatform.Schema.Abstractions.Models
{
    using System.Collections.Generic;

    public sealed class Function
    {
        public string Body
        {
            get;
            set;
        }

        public IDictionary<string, string> Parameters
        {
            get;
            set;
        }
    }
}
