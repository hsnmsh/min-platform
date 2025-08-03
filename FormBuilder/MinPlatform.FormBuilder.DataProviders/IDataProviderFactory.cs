namespace MinPlatform.FormBuilder.DataProviders
{
    public interface IDataProviderFactory
    {
        IDataProvider GetDataProviders(string name);
    }
}
