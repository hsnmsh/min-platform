namespace MinPlatform.Notifications.Abstractions.Email
{
    public sealed class FileAttachment
    {
        public string FileName 
        { 
            get; 
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public string FileExtension
        {
            get;
            set;
        }
    }
}
