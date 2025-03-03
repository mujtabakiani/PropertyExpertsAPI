using System.Linq.Dynamic.Core;
using PropertyExperts.API.Models.Entities;
using PropertyExperts.API.Utils;
using PropertyExperts.API.Models.Responses;

namespace PropertyExperts.API.Services.BusinessLogic
{
    public class DecisionRuleEngine : IDecisionRuleEngine
    {
        private readonly List<DecisionRule> _rules;

        public DecisionRuleEngine()
        {
            _rules = FileHelper.LoadDecisionRules();
        }

        public List<string> EvaluateInvoice(decimal invoiceAmount, InvoiceClassificationResponse classification)
        {
            List<string> actions = new List<string>();

            foreach (var rule in _rules)
            {
                if (EvaluateRule(rule, invoiceAmount, classification))
                {
                    actions.Add(rule.Action);
                }
            }

            return actions;
        }
        private bool EvaluateRule(DecisionRule rule, decimal invoiceAmount, InvoiceClassificationResponse classification)
        {
            string expression = rule.Condition.Replace("AND", "&&").Replace("OR", "||");

            expression = ConvertSingleQuotedStrings(expression);
            var parameter = new
            {
                amount = invoiceAmount,
                riskLevel = classification.RiskLevel,
                classification = classification.Classification
            };

            var lambda = DynamicExpressionParser.ParseLambda(parameter.GetType(),typeof(bool),expression);

            return (bool)lambda.Compile().DynamicInvoke(parameter);
        }

        private static string ConvertSingleQuotedStrings(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, @"'([^']*)'", "\"$1\"");
        }
    }
}
