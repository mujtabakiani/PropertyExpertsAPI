namespace PropertyExperts.API.Models.Entities
{
    public class DecisionRule
    {
        public int RuleId { get; set; }
        public required string Condition { get; set; }
        public required string Action { get; set; }
    }
}
