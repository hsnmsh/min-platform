namespace MinPlatform.Validators.Service.Extensions
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Validators;

    public static class BaseDefinitionExtensions
    {
        public static TDefinition Null<TDefinition, TProperty>(this TDefinition baseDefinition) where TDefinition : BaseDefinition
        {
            baseDefinition.SetValidator(new NullValidator<TProperty>());

            return baseDefinition;
        }

        public static TDefinition NotNull<TDefinition, TProperty>(this TDefinition baseDefinition) where TDefinition : BaseDefinition
        {
            baseDefinition.SetValidator(new NotNullValidator<TProperty>());

            return baseDefinition;
        }

        public static TDefinition Equal<TDefinition, TProperty>(this TDefinition baseDefinition, TProperty compareValue) where TDefinition : BaseDefinition
        {
            baseDefinition.SetValidator(new EqualValidator<TProperty>(compareValue));

            return baseDefinition;
        }

        public static TDefinition NotEqual<TDefinition, TProperty>(this TDefinition baseDefinition, TProperty compareValue) where TDefinition : BaseDefinition
        {
            baseDefinition.SetValidator(new NotEqualValidator<TProperty>(compareValue));

            return baseDefinition;
        }
    }
}
