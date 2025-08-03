namespace MinPlatform.Schema.Migrators.Runner.Logging
{
    public interface IPasswordMaskUtility
    {
        string ApplyMask(string connectionString);
    }
}
