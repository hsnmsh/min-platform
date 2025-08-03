namespace MinPlatform.FormBuilder.Elements.Inputs.Models
{
    using System.Collections.Generic;

    internal sealed class EmailPropertyInput : BasePropertyInput
    {
        public override InputType Type { get => InputType.Email; }

        public IEnumerable<string> AllowedDomains
        {
            get;
            set;
        }
    }
}
