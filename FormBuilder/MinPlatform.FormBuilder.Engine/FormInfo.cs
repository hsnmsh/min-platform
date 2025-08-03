namespace MinPlatform.FormBuilder.Engine
{
    using MinPlatform.FormBuilder.Elements.Sections;
    using System.Collections.Generic;

    public class FormInfo
    {
        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string ServiceCode
        {
            get;
            set;
        }

        public string LanguageCode
        {
            get;
            set;
        }

        public string LanguageText
        {
            get;
            set;
        }

        public string TargetClient
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public IEnumerable<FormGroupSection> FormGroups
        {
            get;
            set;
        }
    }
}
