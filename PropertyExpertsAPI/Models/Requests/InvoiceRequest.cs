namespace PropertyExperts.API.Models.Requests
{
    public class InvoiceRequest
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Comment { get; set; }
        public decimal Amount { get; set; }
        public IFormFile File { get; set; }
    }
}
