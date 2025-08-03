namespace MinPlatform.FormBuilder.Engine
{
    using MinPlatform.Data.Abstractions.Models;
    using MinPlatform.FormBuilder.Elements.Sections;
    using System.Collections.Generic;

    public class FormEntity : AbstractEntity<int>
    {
        public override int Id
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

        public IDictionary<string, object> InputProperties 
        { 
            get; 
            set; 
        }

        public IEnumerable<string> AllowedRoles 
        { 
            get;
            set;
        }

        public IEnumerable<FormGroupPropertySection> FormGroups
        {
            get;
            set;
        }
    }
}
