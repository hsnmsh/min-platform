namespace MinPlatform.FormBuilder.Elements.Inputs.Models
{
    internal sealed class SliderPropertyInput : BasePropertyInput
    {
        public override InputType Type { get => InputType.Slider; }

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
