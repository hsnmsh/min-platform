namespace MinPlatform.DI.Abstractions
{
    public interface IObjectResolver<ObjectBuilderType, out ObjectBuilder>
    {
        ObjectBuilder ResolveObjectBuilder(ObjectBuilderType objectBuilderType);
    }
}
