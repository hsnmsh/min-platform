namespace MinPlatform.FormBuilder.Elements.Inputs.Models
{
    using System.Collections.Generic;

    internal sealed class FileUploaderPropertyInput : BasePropertyInput
    {
        public override InputType Type { get => InputType.File; }

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
