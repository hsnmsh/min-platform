namespace MinPlatform.FormBuilder.DataProviders
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class DataProviderFactory : IDataProviderFactory
    {
        private readonly IDictionary<string, IDataProvider> dataProviders;

        public DataProviderFactory(IEnumerable<IDataProvider> dataProviderList)
        {
            dataProviders = dataProviderList.ToDictionary(dataProvider => dataProvider.Name, dataProvider => dataProvider);
        }

        public IDataProvider GetDataProviders(string name)
        {
            if (dataProviders.TryGetValue(name, out IDataProvider dataProvider))
            {
                return dataProvider;
            }

            return null;
        }
    }
}
