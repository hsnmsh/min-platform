namespace MinPlatform.FormBuilder.Elements.Inputs.Models
{
    internal sealed class TextAreaPropertyInput : BasePropertyInput
    {
        public override InputType Type { get => InputType.TextArea; }

        public int MaxTextSize
        {
            get;
            set;
        }
    }
}
