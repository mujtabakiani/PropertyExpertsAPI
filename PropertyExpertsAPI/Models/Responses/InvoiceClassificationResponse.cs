namespace PropertyExperts.API.Models.Responses
{
    public class InvoiceClassificationResponse
    {
		public bool IsValid { get; set; }
        public required string Classification { get; set; }
        public required string RiskLevel { get; set; }
    }
}
