namespace MinPlatform.EngineRule.Service
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public interface IEngineRuleService
    {
        Task<bool> EvaluateAsync(string expressionStatment);

        Task<bool> EvaluateAsync(string expressionStatment, IDictionary<string, object> parameters);

        Task<bool> EvaluateValueInListAsync<valueId>(valueId value, IEnumerable<valueId> setOfValues);

    }
}
