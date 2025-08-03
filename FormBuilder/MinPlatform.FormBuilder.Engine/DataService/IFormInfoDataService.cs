namespace MinPlatform.FormBuilder.Engine.DataService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFormInfoDataService
    {
        Task<IEnumerable<FormEntity>> GetFormsAsync(string name);
        Task<FormEntity> GetFormAsync(string name, string languageCode);

    }
}
