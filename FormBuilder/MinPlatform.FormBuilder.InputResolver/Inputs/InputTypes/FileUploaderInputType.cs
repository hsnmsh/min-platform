namespace MinPlatform.FormBuilder.Elements.Inputs.InputTypes
{
    using System.Collections.Generic;

    public sealed class FileUploaderInputType : BaseInputType
    {
        public bool MultipleFiles
        {
            get;
            set;
        }

        public IEnumerable<string> Accept
        {
            get;
            set;
        }

        public bool ShowClearButton
        {
            get;
            set;
        }

        public bool UploadOnPickFile
        {
            get;
            set;
        }

        public string UploadUrl
        {
            get;
            set;
        }

        public string DownloadUrl
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
