namespace MinPlatform.Notifications.Abstractions.Email
{
    using System.Collections.Generic;

    public sealed class EmailInfo
    {
        public string From 
        { 
            get; 
            set;
        }
        
        public IList<string> To 
        { 
            get;
            set;
        }

        public IList<string> CC
        {
            get;
            set;
        }

        public IList<string> BCC
        {
            get;
            set;
        }

        public IList<FileAttachment> Files 
        { 
            get;
            set; 
        }
    }
}
