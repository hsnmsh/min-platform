namespace MinPlatform.FormBuilder.Elements.Inputs.InputTypes
{
    using System.Collections.Generic;

    public sealed class TelephoneInputType : BaseInputType
    {
        public IDictionary<string, string> Countries
        {
            get;
            set;
        }

        public IDictionary<string, string> PreferredCountries
        {
            get;
            set;
        }

        public bool HideDropdown
        {
            get;
            set;
        }

        public string Prefix
        {
            get;
            set;
        }

        public string DefaultMask
        {
            get;
            set;
        }

        public bool DisableCountryGuess
        {
            get;
            set;
        }

        public bool DisableFormatting
        {
            get;
            set;
        }

        public bool Flags
        {
            get;
            set;
        }
    }
}
