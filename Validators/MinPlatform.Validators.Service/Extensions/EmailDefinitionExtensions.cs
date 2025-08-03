namespace MinPlatform.Validators.Service.Extensions
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Validators;

    public static class EmailDefinitionExtensions
    {
        public static EmailDefinition ValidateEmail(this EmailDefinition emailDefinition)
        {
            emailDefinition.SetValidator(new EmailValidator());

            return emailDefinition;
        }
    }
}
