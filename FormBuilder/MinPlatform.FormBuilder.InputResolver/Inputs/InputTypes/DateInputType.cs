namespace MinPlatform.FormBuilder.Elements.Inputs.InputTypes
{
    using System;

    public sealed class DateInputType : BaseInputType
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
    }
}
