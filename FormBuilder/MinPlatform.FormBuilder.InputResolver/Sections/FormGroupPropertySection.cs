namespace MinPlatform.FormBuilder.Elements.Sections
{
    using MinPlatform.FormBuilder.Elements.Inputs.Models;
    using System.Collections.Generic;

    public class FormGroupPropertySection
    {
        public string Name
        {
            get;
            set;
        }

        public string Id
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

        public string Disabled
        {
            get;
            set;
        }

        public string Visibility
        {
            get;
            set;
        }

        public int ElementsPerRow
        {
            get;
            set;
        }

        public string DataProviderName
        {
            get;
            set;
        }

        public IEnumerable<ActionButton> Buttons
        {
            get;
            set;
        }

        public IEnumerable<BasePropertyInput> GroupInputs
        {
            get;
            set;
        }
    }

    public sealed class ActionButton
    {
        public string Name
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }

        public string ActionName
        {
            get;
            set;
        }

        public string Disabled
        {
            get;
            set;
        }

        public string Visibility
        {
            get;
            set;
        }

        public IDictionary<string, object> Styles 
        { 
            get;
            set;
        }

        public string Text 
        { 
            get;
            set;
        }

        public string IconUrl 
        { 
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }
    }
}
