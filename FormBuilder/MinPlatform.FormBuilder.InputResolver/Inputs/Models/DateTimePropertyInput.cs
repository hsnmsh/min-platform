namespace MinPlatform.FormBuilder.Elements.Inputs.Models
{
    using System;

    internal sealed class DateTimePropertyInput : BasePropertyInput
    {
        public override InputType Type { get => InputType.DateTime; }

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

        public string TimeZone
        {
            get;
            set;
        }
    }
}
