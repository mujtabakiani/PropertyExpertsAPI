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



/*

using PropertyExperts.API.Models.Entities;
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
            // Mock rules instead of loading from FileHelper
            var rules = new List<DecisionRule>
            {
                new DecisionRule { Condition = "amount < 500 && riskLevel == \"Low\"", Action = "Approve" },
                new DecisionRule { Condition = "amount >= 500 && riskLevel == \"High\"", Action = "Reject" }
            };

            // Override the private field using reflection (since we can't inject)
            _ruleEngine = new DecisionRuleEngine();
            typeof(DecisionRuleEngine)
                .GetField("_rules", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(_ruleEngine, rules);
        }

        [Test]
        public void EvaluateInvoice_ShouldReturnApprove_WhenLowRiskAndAmountIsLessThan500()
        {
            // Arrange
            decimal invoiceAmount = 400;
            var classification = new InvoiceClassificationResponse { RiskLevel = "Low", Classification = "FireDamagedWallRepair" };

            // Act
            var result = _ruleEngine.EvaluateInvoice(invoiceAmount, classification);

            // Assert
            Assert.Contains("Approve", result);
        }

        [Test]
        public void EvaluateInvoice_ShouldReturnReject_WhenHighRiskAndAmountIs500OrMore()
        {
            // Arrange
            decimal invoiceAmount = 600;
            var classification = new InvoiceClassificationResponse { RiskLevel = "High", Classification = "FloodDamage" };

            // Act
            var result = _ruleEngine.EvaluateInvoice(invoiceAmount, classification);

            // Assert
            Assert.Contains("Reject", result);
        }
    }
}

 */
 
 
 





