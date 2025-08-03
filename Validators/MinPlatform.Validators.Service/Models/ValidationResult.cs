using System.Collections.Generic;

namespace MinPlatform.Validators.Service.Models
{
    public sealed class ValidationResult
    {
        public bool IsValid
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }

        public IEnumerable<string> ErrorDescriptions
        {
            get;
            set;
        }
    }
}
