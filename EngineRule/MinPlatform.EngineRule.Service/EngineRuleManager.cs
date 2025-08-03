namespace MinPlatform.EngineRule.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public sealed class EngineRuleManager
    {
        private readonly IEngineRuleService engineRuleService;

        public EngineRuleManager(IEngineRuleService engineRuleService)
        {
            this.engineRuleService = engineRuleService;
        }

        public async Task<bool> EvaluateAsync(string expressionStatment)
        {
            if (string.IsNullOrEmpty(expressionStatment))
            {
                throw new ArgumentNullException(nameof(expressionStatment));
            }

            return await engineRuleService.EvaluateAsync(expressionStatment);
        }

        public async Task<bool> EvaluateAsync(string expressionStatment, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(expressionStatment))
            {
                throw new ArgumentNullException(nameof(expressionStatment));
            }

            if (parameters is null || !parameters.Any())
            {
                throw new ArgumentNullException("parameter expression must not be null or empty");
            }

            return await engineRuleService.EvaluateAsync(expressionStatment, parameters);

        }

        public async Task<bool> EvaluateValueInListAsync<valueId>(valueId value, IEnumerable<valueId> setOfValues)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (setOfValues is null || !setOfValues.Any())
            {
                throw new ArgumentNullException("set Of Values expression must not be null or empty");
            }

            return await engineRuleService.EvaluateValueInListAsync(value, setOfValues);
        }
    }
}
