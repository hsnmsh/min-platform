namespace MinPlatform.FormBuilder.Elements.Inputs.InputTypes
{
    using System;

    public sealed class DateTimeInputType : BaseInputType
    {
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
