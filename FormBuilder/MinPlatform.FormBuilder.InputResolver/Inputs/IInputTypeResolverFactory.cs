namespace MinPlatform.FormBuilder.Elements.Inputs
{
    using MinPlatform.FormBuilder.Elements.Inputs.InputTypes;
    using MinPlatform.FormBuilder.Elements.Inputs.Models;
    using System.Threading.Tasks;

    public interface IInputTypeResolverFactory
    {
        Task<BaseInputType> CreateInputTypeAsync(BasePropertyInput propertyInputType);
    }
}
