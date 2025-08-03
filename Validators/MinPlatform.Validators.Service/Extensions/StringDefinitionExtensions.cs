namespace MinPlatform.Validators.Service.Extensions
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Validators;

    public static class StringDefinitionExtensions
    {
        public static StringDefinition Empty(this StringDefinition stringDefinition)
        {
            stringDefinition.SetValidator(new EmptyValidator());

            return stringDefinition;
        }

        public static StringDefinition NotEmpty(this StringDefinition stringDefinition)
        {
            stringDefinition.SetValidator(new NotEmptyValidator());

            return stringDefinition;
        }

        public static StringDefinition MinLength(this StringDefinition stringDefinition, int value)
        {
            stringDefinition.SetValidator(new MinLengthValidator(value));

            return stringDefinition;
        }

        public static StringDefinition MaxLength(this StringDefinition stringDefinition, int value)
        {
            stringDefinition.SetValidator(new MaxLengthValidator(value));

            return stringDefinition;
        }

        public static StringDefinition Length(this StringDefinition stringDefinition, int value)
        {
            stringDefinition.SetValidator(new LengthValidator(value));

            return stringDefinition;
        }

        public static StringDefinition RegEx(this StringDefinition stringDefinition)
        {
            stringDefinition.SetValidator(new RegExValidator());

            return stringDefinition;
        }

        public static StringDefinition StartWith(this StringDefinition stringDefinition, string value, bool ignoreCase = true)
        {
            stringDefinition.SetValidator(new StartWithValidator(value, ignoreCase));

            return stringDefinition;
        }

        public static StringDefinition EndWith(this StringDefinition stringDefinition, string value, bool ignoreCase = true)
        {
            stringDefinition.SetValidator(new EndWithValidator(value, ignoreCase));

            return stringDefinition;
        }

        public static StringDefinition Contains(this StringDefinition stringDefinition, string value, bool ignoreCase = true)
        {
            stringDefinition.SetValidator(new ContainsValidator(value, ignoreCase));

            return stringDefinition;
        }

        public static StringDefinition ContainsWithIndex(this StringDefinition stringDefinition, string stringPattern, int patternIndex, bool ignoreCase = true)
        {
            stringDefinition.SetValidator(new ContainsWithIndexValidator(stringPattern, patternIndex, ignoreCase));

            return stringDefinition;
        }
    }
}
