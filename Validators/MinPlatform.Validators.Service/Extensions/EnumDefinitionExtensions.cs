namespace MinPlatform.Validators.Service.Extensions
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Validators;

    public static class EnumDefinitionExtensions
    {
        public static EnumDefinition IsDefinedEnum(this EnumDefinition decimalDefinition)
        {
            decimalDefinition.SetValidator(new DefinedEnumValidator());

            return decimalDefinition;
        }
    }
}
