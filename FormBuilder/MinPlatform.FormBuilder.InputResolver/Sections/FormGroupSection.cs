namespace MinPlatform.FormBuilder.Elements.Sections
{
    using MinPlatform.FormBuilder.Elements.Inputs.InputTypes;
    using System.Collections.Generic;

    public class FormGroupSection
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

        public bool Visibility
        {
            get;
            set;
        }

        public int ElementsPerRow
        {
            get;
            set;
        }

        public IEnumerable<ActionButtonGroup> Buttons
        {
            get;
            set;
        }

        public IEnumerable<BaseInputType> GroupInputs
        {
            get;
            set;
        }
    }

    public sealed class ActionButtonGroup
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

        public bool Disabled
        {
            get;
            set;
        }

        public bool Visibility
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
