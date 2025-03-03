using System.Text;
using PropertyExperts.API.Models.Requests;
using PropertyExperts.API.Models.Responses;

namespace PropertyExperts.API.Utils
{
    public static class EvaluationHelper
    {
		private static int _evaluationCounter = 1;
		public static EvaluationSummary GenerateSummary(InvoiceRequest request, InvoiceClassificationResponse? classification, List<string>? rulesApplied)
		{
			var evaluationText = GenerateEvaluationSummaryText(request, classification, rulesApplied);
			var evaluationFileBase64 = FileHelper.ConvertToBase64(evaluationText);

			return new EvaluationSummary
			{
				EvaluationId = GenerateEvaluationId(),
				InvoiceId = request.InvoiceId,
				Classification = classification?.Classification,
				RulesApplied = rulesApplied,
				EvaluationFile = evaluationFileBase64
			};
		}


		public static string GenerateEvaluationSummaryText(InvoiceRequest request, InvoiceClassificationResponse? classification, List<string>? rulesApplied)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Invoice Evaluation Summary");
            sb.AppendLine($"Invoice ID: {request.InvoiceId}");
            sb.AppendLine($"Classification: {classification?.Classification}");
            sb.AppendLine($"Risk Level: {classification?.RiskLevel}");
            sb.AppendLine($"Amount: {request.Amount}");
            var appliedRules = (rulesApplied != null && rulesApplied.Any()) ? string.Join(", ", rulesApplied) : "";
			sb.AppendLine($"Rules Applied: {appliedRules}");

			sb.AppendLine($"Result Explanation:");

            if (appliedRules.Contains("Approve"))
                sb.AppendLine("The invoice has been approved automatically.");
            else if (appliedRules.Contains("Flag for Review"))
                sb.AppendLine("The invoice requires manual review due to high risk.");
            else
                sb.AppendLine("No specific rule was applied to this invoice.");

            return sb.ToString();
        }
        public static string GenerateEvaluationId()
        {
            return $"EVAL{_evaluationCounter++.ToString("D3")}";
        }
    }
}
