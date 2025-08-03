namespace MinPlatform.FormBuilder.Elements.Inputs.Models
{
    using System.Collections.Generic;

    internal sealed class RadioPropertyInput : BasePropertyInput
    {
        public override InputType Type { get => InputType.Radio; }

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
