namespace MinPlatform.Validators.Service.Definitions
{
    using MinPlatform.Validators.Service.Models;
    using MinPlatform.Validators.Service.Validators;
    using System.Collections.Generic;

    public abstract class BaseDefinition
    {
        public string Key
        {
            get;
            set;
        }

        public IDictionary<ErrorMessageType, string> CustomErrorMessage
        {
            get;
            set;
        }

        public IDictionary<string, string> Properties
        {
            get;
            set;
        }

        public bool ThrowInValidException 
        { 
            get;
            set; 
        }

        internal abstract string DefinitionName
        {
            get;
        }

        internal IList<IBaseValidator> Validators;

        public BaseDefinition()
        {
            Validators = new List<IBaseValidator>();
        }

        internal void SetValidator(IBaseValidator validator)
        {
            Validators.Add(validator);
        }

        internal string GetErrorMessage(ErrorMessageType errorMessageType)
        {
            if (CustomErrorMessage != null)
            {
                if (CustomErrorMessage.TryGetValue(errorMessageType, out string message))
                {
                    return message;
                }
            }

            string enumValueName = errorMessageType.ToString();

            return typeof(Constants).GetField(enumValueName).GetValue(null).ToString();
        }

        internal abstract ValidationCheckResult CheckValidatorsPerDefinition(object value);


    }
}
