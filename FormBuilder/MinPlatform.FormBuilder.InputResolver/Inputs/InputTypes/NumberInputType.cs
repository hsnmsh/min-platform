namespace MinPlatform.FormBuilder.Elements.Inputs.InputTypes
{
    public sealed class NumberInputType : BaseInputType
    {
        public int? MinValue
        {
            get;
            set;
        }

        public int? MaxValue
        {
            get;
            set;
        }
    }
}
