using PropertyExperts.API.Models.Responses;

namespace PropertyExperts.API.Services.BusinessLogic
{
    public interface IDecisionRuleEngine
    {
        List<string> EvaluateInvoice(decimal invoiceAmount, InvoiceClassificationResponse classification);
    }
}
