namespace MinPlatform.FormBuilder.Elements.Inputs.Models
{
    public abstract class BasePropertyInput
    {
        public abstract InputType Type
        {
            get;
        }

        public string Name
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }

        public string Label
        {
            get;
            set;
        }

        public string Classes
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }

        public InputStyle Variant
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public string Disabled
        {
            get;
            set;
        }

        public string Required
        {
            get;
            set;
        }

        public string PlaceHolder
        {
            get;
            set;
        }

        public string Readonly
        {
            get;
            set;
        }

        public string Visibility
        {
            get;
            set;
        }

        public string DataProviderName
        {
            get;
            set;
        }
    }

    public enum InputType
    {
        Text,
        Number,
        DateTime,
        Date,
        Email,
        Telephone,
        TextArea,
        Slider,
        Toggle,
        CheckBox,
        Radio,
        DropDownList,
        File,
        ListView

    }

    public enum InputStyle
    {
        Normal,
        FadedBackgroud,
        Material,
    }
}
