#if NETFRAMEWORK

namespace MinPlatform.Schema.Migrators.Runner.Initialization.NetFramework
{
    using System.Configuration;

    using JetBrains.Annotations;

    /// <summary>
    /// Understand .NET config mechanism and provides access to Configuration sections
    /// </summary>
    public interface INetConfigManager
    {
        [NotNull]
        Configuration LoadFromFile(string path);

        [NotNull]
        Configuration LoadFromMachineConfiguration();
    }
}
#endif
