namespace MinPlatform.Schema.Abstractions.Models
{
    public sealed class DataBaseConfig
    {
        public string DataBaseName 
        { 
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int Version
        {
            get;
            set;
        }
    }
}
