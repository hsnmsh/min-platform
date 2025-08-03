namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;

    internal interface IBaseValidator
    {
    }

    internal interface IValidator: IBaseValidator
    {
        ValidationCheckResult IsValid(BaseDefinition typeDefinition);
    }

    internal interface IValidator<TProperty>: IBaseValidator
    {
        ValidationCheckResult IsValid(BaseDefinition typeDefinition, TProperty value);
    }
}
