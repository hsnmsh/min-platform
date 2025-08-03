namespace MinPlatform.Data.Service.Lookup
{
    using MinPlatform.Data.Abstractions.Models;

    public sealed class LookupEntity : AbstractEntity<string>
    {
        public override string Id 
        { 
            get ;
            set; 
        }

        public string Code 
        { 
            get;
            set;
        }

        public string Title 
        { 
            get;
            set;
        }

        public string LanguageCode 
        { 
            get;
            set;
        }
    }
}
