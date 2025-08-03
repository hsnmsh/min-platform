namespace MinPlatform.FormBuilder.Elements.Inputs.InputTypes
{
    using System.Collections.Generic;

    public sealed class CheckBoxInputType : BaseInputType
    {
        public IDictionary<string, string> Options
        {
            get;
            set;
        }

        public IDictionary<string, bool> DisabledOptions
        {
            get;
            set;
        }
    }
}
