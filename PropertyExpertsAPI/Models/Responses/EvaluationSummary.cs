namespace PropertyExperts.API.Models.Responses
{
    public class EvaluationSummary
    {
        public required string EvaluationId { get; set; }
        public required int InvoiceId { get; set; }
        public List<string>? RulesApplied { get; set; }
        public string? Classification { get; set; }
        public required string EvaluationFile { get; set; }
    }
}
