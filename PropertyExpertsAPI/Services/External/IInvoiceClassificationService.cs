using PropertyExperts.API.Models.Responses;

namespace PropertyExperts.API.Services.External
{
    public interface IInvoiceClassificationService
    {
        Task<InvoiceClassificationResponse> GetClassificationAsync(byte[] document);
    }
}
