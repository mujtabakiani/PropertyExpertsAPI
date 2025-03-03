using Moq;
using PropertyExperts.API.Models.Responses;
using PropertyExperts.API.Services.External;

namespace PropertyExperts.Test.Unit
{
    [TestFixture]
    public class InvoiceClassificationServiceTests
    {
        private Mock<IInvoiceClassificationService> _classificationServiceMock;

        [SetUp]
        public void Setup()
        {
            _classificationServiceMock = new Mock<IInvoiceClassificationService>();
            _classificationServiceMock
                .Setup(x => x.GetClassificationAsync(It.IsAny<byte[]>()))
                .ReturnsAsync(new InvoiceClassificationResponse
                {
                    Classification = "WaterLeakDetection",
                    RiskLevel = "Low"
                });
        }

        [Test]
        public async Task GetClassificationAsync_ShouldReturnMockedData()
        {
            var result = await _classificationServiceMock.Object.GetClassificationAsync(new byte[0]);

            Assert.AreEqual("WaterLeakDetection", result.Classification);
            Assert.AreEqual("Low", result.RiskLevel);
        }
    }
}
