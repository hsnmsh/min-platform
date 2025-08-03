namespace MinPlatform.FormBuilder.Elements.Inputs.Models
{
    using System;

    internal sealed class DatePropertyInput : BasePropertyInput
    {
        public override InputType Type { get => InputType.Date; }

        public DateTime? MinValue
        {
            get;
            set;
        }

        public DateTime? MaxValue
        {
            get;
            set;
        }

        public string Format
        {
            get;
            set;
        }
    }
}
