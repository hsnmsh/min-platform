namespace MinPlatform.EngineRule.EvaluationEngine
{
    using MinPlatform.EngineRule.Service;
    using NCalc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal sealed class EngineRuleService : IEngineRuleService
    {
        public async Task<bool> EvaluateAsync(string expressionStatment)
        {
            var expression = new Expression(expressionStatment);

            return await Task.Run(() => { return (bool)expression.Evaluate(); });
        }

        public async Task<bool> EvaluateAsync(string expressionStatment, IDictionary<string, object> parameters)
        {
            var expression = new Expression(expressionStatment);
            expression.Parameters = (Dictionary<string, object>)parameters;

            return await Task.Run(() => { return (bool)expression.Evaluate(); });
        }

        public async Task<bool> EvaluateValueInListAsync<valueId>(valueId value, IEnumerable<valueId> setOfValues)
        {
            var expressionBuilder = new StringBuilder().Append("in([value], ");
            var listValues = new Dictionary<string, object>();
            listValues["value"] = value;

            valueId[] arrayOfVaues = setOfValues.ToArray();

            for (int i = 0; i < arrayOfVaues.Length; i++)
            {
                string variableName = string.Concat("v", i);
                expressionBuilder.Append("[" + variableName + "],");
                listValues[variableName] = arrayOfVaues[i];
            }

            expressionBuilder.Remove(expressionBuilder.Length - 1, 1).Append(")");

            var expression = new Expression(expressionBuilder.ToString());
            expression.Parameters = listValues;

            return await Task.Run(() =>
            {
                return (bool)expression.Evaluate();
            });
        }
    }
}
