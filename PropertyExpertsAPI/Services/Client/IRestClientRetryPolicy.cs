using RestSharp;

namespace PropertyExperts.API.Services.Client
{
	public interface IRestClientRetryPolicy
	{
		Task<RestResponse> ExecuteWithRetryAsync(RestRequest request, RestClient client);
	}
}
