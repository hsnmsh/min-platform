namespace MinPlatform.Logging.Abstractions.Models
{
    public sealed class FileProperties : Properties
    {
        public string Path 
        { 
            get;
            set;
        }

        public int MaxSize 
        { 
            get;
            set; 
        }
    }
}
