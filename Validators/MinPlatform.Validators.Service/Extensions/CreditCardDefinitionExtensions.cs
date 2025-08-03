namespace MinPlatform.Validators.Service.Extensions
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Validators;

    public static class CreditCardDefinitionExtensions
    {
        public static CreditCardDefinition ValidateCreditCard(this CreditCardDefinition creditCardDefinition)
        {
            creditCardDefinition.SetValidator(new CreditCardValidator());

            return creditCardDefinition;
        }
    }
}
