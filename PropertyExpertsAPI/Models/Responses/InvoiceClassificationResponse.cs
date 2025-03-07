namespace PropertyExperts.API.Models.Responses
{
    public class InvoiceClassificationResponse
    {
		public bool IsValid { get; set; }
        public string? Classification { get; set; }
        public string? RiskLevel { get; set; }
    }
}
