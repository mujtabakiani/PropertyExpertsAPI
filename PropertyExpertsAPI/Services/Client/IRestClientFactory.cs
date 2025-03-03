using RestSharp;

namespace PropertyExperts.API.Services.Client
{
	public interface IRestClientFactory
	{
		RestClient CreateClient(string baseUrl);
	}
}
