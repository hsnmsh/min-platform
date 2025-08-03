namespace MinPlatform.FormBuilder.Elements.Inputs.Models
{
    public sealed class NumberPropertyInput : BasePropertyInput
    {
        public override InputType Type { get => InputType.Number; }

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
