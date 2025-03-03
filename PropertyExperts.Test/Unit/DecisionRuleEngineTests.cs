using PropertyExperts.API.Models.Responses;
using PropertyExperts.API.Services.BusinessLogic;

namespace PropertyExperts.Test.Unit
{
    [TestFixture]
    public class DecisionRuleEngineTests
    {
        private DecisionRuleEngine _ruleEngine;

        [SetUp]
        public void Setup()
        {
            _ruleEngine = new DecisionRuleEngine();
        }

        [Test]
        public void EvaluateInvoice_ShouldApplyRules_Correctly()
        {
            decimal invoiceAmount = 100;
             
            var classification = new InvoiceClassificationResponse
            {
                RiskLevel = "Low",
                Classification = "FireDamagedWallRepair"
            };

            var result = _ruleEngine.EvaluateInvoice(invoiceAmount, classification);

            Assert.Contains("Approve", result);
        }
    }
}





