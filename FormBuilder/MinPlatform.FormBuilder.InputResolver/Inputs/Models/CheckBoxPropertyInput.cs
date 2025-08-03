namespace MinPlatform.FormBuilder.Elements.Inputs.Models
{
    using System.Collections.Generic;

    internal sealed class CheckBoxPropertyInput : BasePropertyInput
    {
        public override InputType Type { get => InputType.CheckBox; }

        public IDictionary<string, string> Options
        {
            get;
            set;
        }

        public IDictionary<string, string> DisabledOptions
        {
            get;
            set;
        }
    }
}
