namespace MinPlatform.Validators.Service.Models
{
    using System.Collections.Generic;

    public sealed class ValidationCheckResult
    {
        public bool IsValid 
        { 
            get;
            set;
        }

        public List<string> ErrorDescription
        {
            get;
            set;
        }
    }
}
