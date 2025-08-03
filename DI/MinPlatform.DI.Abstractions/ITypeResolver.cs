namespace MinPlatform.DI.Abstractions
{
    public interface ITypeResolver<out OutType>
    {
        OutType Resolve();
    }
}
