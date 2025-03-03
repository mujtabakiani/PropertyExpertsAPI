using RestSharp;

namespace PropertyExperts.API.Services.Client
{
	public class RestClientFactory: IRestClientFactory
	{
		public RestClient CreateClient(string baseUrl)
		{
			return new RestClient(baseUrl);
		}
	}
}
