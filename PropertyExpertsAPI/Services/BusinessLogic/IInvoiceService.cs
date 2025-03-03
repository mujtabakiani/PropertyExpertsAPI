using PropertyExperts.API.Models.Requests;
using PropertyExperts.API.Models.Responses;

namespace PropertyExperts.API.Services.BusinessLogic
{
    public interface IInvoiceService
    {
        Task<EvaluationSummary> EvaluateInvoiceAsync(InvoiceRequest request);
    }
}
