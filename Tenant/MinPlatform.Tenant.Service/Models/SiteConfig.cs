namespace MinPlatform.Tenant.Service.Models
{
    using System.Collections.Generic;

    public sealed class SiteConfig
    {
        public string Id 
        { 
            get;
            set;
        }

        public string OfficalEntityName 
        { 
            get;
            set;
        }

        public string OfficalEntityDomain 
        { 
            get;
            set;
        }

        public string LanguageCode 
        { 
            get;
            set;
        }

        public string LogoUrl 
        { 
            get;
            set; 
        }

        public string PrimaryColor 
        { 
            get; 
            set; 
        }

        public string LabelColor 
        { 
            get;
            set;
        }

        public string TextTypingColor 
        { 
            get;
            set; 
        }

        public string FooterDescripion 
        { 
            get;
            set;
        }

        public string SocialMediaLinks 
        { 
            get;
            set;
        }

        public string TelephoneNumber 
        { 
            get;
            set;
        }

        public string Email 
        { 
            get;
            set;
        }

        public IDictionary<string, object> ExtensionData 
        { 
            get;
            set;
        }

        public IDictionary<string, object> Styles 
        { 
            get;
            set;
        }

        public IDictionary<string, object> Configurations 
        { 
            get;
            set; 
        }

        public string EntityCode 
        { 
            get; 
            set;
        }

        public string LanguageText 
        { 
            get;
            set;
        }
    }

}
