namespace MinPlatform.FormBuilder.Elements.Inputs.Models
{
    using System.Collections.Generic;

    internal sealed class DropDownListPropertyInput : BasePropertyInput
    {
        public override InputType Type { get => InputType.DropDownList; }

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

        public bool ShowCheckBox
        {
            get;
            set;
        }
    }
}
