namespace MinPlatform.FormBuilder.Engine
{
    using MinPlatform.FormBuilder.Context;
    using System.Collections.Generic;

    public sealed class FormInfoAndProperties
    {
        public IDictionary<string, FormInfo> Forms 
        { 
            get; 
            set; 
        }

        public IFormContext<string> FormProperties 
        { 
            get;
            set;
        }
    }
}
