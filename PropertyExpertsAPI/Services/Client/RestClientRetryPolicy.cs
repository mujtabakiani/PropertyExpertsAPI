using Polly.Retry;
using Polly;
using RestSharp;
using System.Net;

namespace PropertyExperts.API.Services.Client
{
	public class RestClientRetryPolicy: IRestClientRetryPolicy
	{
		private readonly AsyncRetryPolicy<RestResponse> _retryPolicy;
		private readonly ILogger<RestClientRetryPolicy> _logger;

		public RestClientRetryPolicy(ILogger<RestClientRetryPolicy> logger,
		IConfiguration configuration)
		{
			_logger = logger;
			var retryCount = int.TryParse(configuration["RestSharpRetryCount"], out var result) ? result : 1;

			_retryPolicy = Policy
				.HandleResult<RestResponse>(r => r.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(r.Content))
				.WaitAndRetryAsync(retryCount, retryAttempt =>
					TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Exponential backoff
					(response, timeSpan, retryCount, context) =>
					{
						_logger.LogWarning("Retry {RetryCount} for API call. Waiting {TimeSpan} before retry.", retryCount, timeSpan);
					});
		}

		public async Task<RestResponse> ExecuteWithRetryAsync(RestRequest request, RestClient client)
		{
			return await _retryPolicy.ExecuteAsync(async () =>
			{
				var response = await client.ExecuteAsync(request);

				if (!response.IsSuccessful)
				{
					_logger.LogWarning("Request failed with status {StatusCode}: {Content}", response.StatusCode, response.Content);
				}

				return response;
			});
		}
	}
}
