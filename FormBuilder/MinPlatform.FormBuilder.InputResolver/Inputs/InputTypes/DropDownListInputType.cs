namespace MinPlatform.FormBuilder.Elements.Inputs.InputTypes
{
    using System.Collections.Generic;

    public sealed class DropDownListInputType : BaseInputType
    {
        public IDictionary<string, string> Options
        {
            get;
            set;
        }

        public IDictionary<string, string> SelectedOptions
        {
            get;
            set;
        }

        public IDictionary<string, string> DefaultOptions
        {
            get;
            set;
        }

        public bool IsMultiValues
        {
            get;
            set;
        }

        public bool AllowTextSearch
        {
            get;
            set;
        }

        public string FilterTextUrl
        {
            get;
            set;
        }

        public bool showCheckBox
        {
            get;
            set;
        }
    }
}
