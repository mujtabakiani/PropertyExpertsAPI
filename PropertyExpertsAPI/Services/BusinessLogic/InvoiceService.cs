using PropertyExperts.API.Models.Requests;
using PropertyExperts.API.Models.Responses;
using PropertyExperts.API.Utils;
using PropertyExperts.API.Services.External;

namespace PropertyExperts.API.Services.BusinessLogic
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceClassificationService _classificationService;
        private readonly IDecisionRuleEngine _ruleEngine;

        public InvoiceService(IInvoiceClassificationService classificationService, IDecisionRuleEngine ruleEngine)
        {
            _classificationService = classificationService;
            _ruleEngine = ruleEngine;
        }

        public async Task<EvaluationSummary> EvaluateInvoiceAsync(InvoiceRequest request)
        {
            byte[] fileData = await FileHelper.ReadFileAsync(request.File);

            var classification = await _classificationService.GetClassificationAsync(fileData);
            var rulesApplied = EvaluateRules(request.Amount, classification);

            var evaluationSummary = EvaluationHelper.GenerateSummary(request, classification,  rulesApplied);

            return evaluationSummary;
        }

		private List<string>? EvaluateRules(decimal amount, InvoiceClassificationResponse classification)
		{
			return classification?.IsValid == true ? _ruleEngine.EvaluateInvoice(amount, classification) : null;
		}
	}
}
