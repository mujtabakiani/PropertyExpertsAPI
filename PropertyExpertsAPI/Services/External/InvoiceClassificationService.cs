using System.Net;
using Newtonsoft.Json;
using PropertyExperts.API.Models.Responses;
using PropertyExperts.API.Services.External;
using PropertyExperts.API.Services.Client;
using RestSharp;

public class MockInvoiceClassificationService : IInvoiceClassificationService
{
	private readonly string _mockApiUrl;
    private readonly ILogger<MockInvoiceClassificationService> _logger;
	private readonly IRestClientFactory _restClientFactory;
	private readonly IRestClientRetryPolicy _retryPolicy;

	public MockInvoiceClassificationService(IRestClientFactory restClientFactory, ILogger<MockInvoiceClassificationService> logger, 
		IRestClientRetryPolicy retryPolicy,IConfiguration configuration)
    {
        _logger = logger;
		_restClientFactory = restClientFactory;
		_retryPolicy = retryPolicy;
		_mockApiUrl = configuration["MockApi:Url"] ?? "";

	}

    public async Task<InvoiceClassificationResponse> GetClassificationAsync(byte[] document)
    {
		var client = _restClientFactory.CreateClient(_mockApiUrl);
		var request = CreateRequest(document);
		var response = await _retryPolicy.ExecuteWithRetryAsync(request, client);

		return HandleResponse(response);
	}

	private RestRequest CreateRequest(byte[] document)
	{
		var request = new RestRequest("", Method.Post);
		request.AddHeader("Content-Type", "application/json");
		request.AddJsonBody(new { FileContent = Convert.ToBase64String(document) });

		return request;
	}

	private InvoiceClassificationResponse HandleResponse(RestResponse response)
	{
		if (response.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
		{
			var classificationResponse = JsonConvert.DeserializeObject<InvoiceClassificationResponse>(response.Content);
			if (classificationResponse != null) classificationResponse.IsValid = true;
			else classificationResponse = new InvoiceClassificationResponse() { Classification = "Unknown", RiskLevel = "Unknown" };
				_logger.LogInformation("Mock API Response: {Response}", response.Content);
			return classificationResponse;
		}

		_logger.LogWarning("Mock API returned an error: {StatusCode} - {Content}", response.StatusCode, response.Content);
		return new InvoiceClassificationResponse { Classification = "Unknown", RiskLevel = "Unknown" };
	}
}
