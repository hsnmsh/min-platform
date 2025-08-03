namespace MinPlatform.FormBuilder.Elements.Inputs.InputTypes
{
    using System.Collections.Generic;

    public sealed class EmailInputType : BaseInputType
    {
        public IEnumerable<string> AllowedDomains
        {
            get;
            set;
        }
    }
}
