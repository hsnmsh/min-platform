namespace MinPlatform.FormBuilder.Context
{
    using MinPlatform.Tenant.Service.Models;
    using System.Collections.Generic;

    public interface IFormContext<FormIdType>
    {
        public FormIdType Id 
        { 
            get;
            set;
        }

        public string Name 
        { 
            get;
            set;
        }

        public IUserProfile User 
        { 
            get; 
            set;
        }

        public IEnumerable<SiteConfig> SiteConfig 
        { 
            get;
            set; 
        }

        public IDictionary<string, object> Properties 
        { 
            get;
            set;
        }

        public RequestInfo Request 
        { 
            get;
            set;
        }
    }
}
