namespace MinPlatform.FormBuilder.Elements.Inputs.InputTypes
{
    public abstract class BaseInputType
    {
        public string InputType
        {
            get;
            internal set;
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

        public string Variant
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public bool Disabled
        {
            get;
            set;
        }

        public bool Required
        {
            get;
            set;
        }

        public string PlaceHolder
        {
            get;
            set;
        }

        public bool Readonly
        {
            get;
            set;
        }

        public bool Visibility
        {
            get;
            set;
        }
    }
}
