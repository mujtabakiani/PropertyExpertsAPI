using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PropertyExperts.Tests.Integration
{
    [TestFixture]
    public class InvoiceEvaluationTests
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task EvaluateInvoice_Returns_ValidResponse()
        {
            var invoiceRequest = new
            {
                InvoiceId = 12345,
                InvoiceNumber = "S12345",
                InvoiceDate = DateTime.UtcNow.AddDays(-5),
                Comment = "Test invoice",
                Amount = 1500.50m
            };

            var fileContent = Encoding.UTF8.GetBytes("%PDF-1.4 Fake PDF content");
            var fileContentStream = new ByteArrayContent(fileContent);
            fileContentStream.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            using var formData = new MultipartFormDataContent
            {
                { new StringContent(invoiceRequest.InvoiceId.ToString()), "InvoiceId" },
                { new StringContent(invoiceRequest.InvoiceNumber), "InvoiceNumber" },
                { new StringContent(invoiceRequest.InvoiceDate.ToString("yyyy-MM-ddTHH:mm:ss")), "InvoiceDate" },
                { new StringContent(invoiceRequest.Comment), "Comment" },
                { new StringContent(invoiceRequest.Amount.ToString()), "Amount" },
                { fileContentStream, "File", "invoice.pdf" }
            };

            var response = await _client.PostAsync("/api/Invoice/evaluate", formData);
            var responseContent = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent.Should().Contain("evaluationId");
        }

        [Test]
        public async Task EvaluateInvoice_InvalidInvoiceNumber_ReturnsBadRequest()
        {
            var invoiceRequest = new
            {
                InvoiceId = 12345,
                InvoiceNumber = "A12345", // Invalid format (should start with 'S')
                InvoiceDate = DateTime.UtcNow.AddDays(-5),
                Comment = "Test invoice",
                Amount = 1500.50m
            };

            var fileContent = Encoding.UTF8.GetBytes("%PDF-1.4 Fake PDF content");
            var fileContentStream = new ByteArrayContent(fileContent);
            fileContentStream.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            using var formData = new MultipartFormDataContent
            {
                { new StringContent(invoiceRequest.InvoiceId.ToString()), "InvoiceId" },
                { new StringContent(invoiceRequest.InvoiceNumber), "InvoiceNumber" },
                { new StringContent(invoiceRequest.InvoiceDate.ToString("yyyy-MM-ddTHH:mm:ss")), "InvoiceDate" },
                { new StringContent(invoiceRequest.Comment), "Comment" },
                { new StringContent(invoiceRequest.Amount.ToString()), "Amount" },
                { fileContentStream, "File", "invoice.pdf" }
            };

            var response = await _client.PostAsync("/api/Invoice/evaluate", formData);
            var responseContent = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
            jsonResponse.GetProperty("success").GetBoolean().Should().BeFalse();

            var errors = jsonResponse.GetProperty("errors").EnumerateArray();
            errors.Should().ContainSingle(e =>
                e.GetProperty("field").GetString() == "InvoiceNumber" &&
                e.GetProperty("message").GetString() == "Invoice number must start with 'S' followed by 5 digits." &&
                e.GetProperty("attemptedValue").GetString() == "A12345"
            );
        }
    }
}
