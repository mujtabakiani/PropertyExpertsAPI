using FluentValidation;
using PropertyExperts.API.Models.Requests;
using PropertyExperts.API.Services.BusinessLogic;
using PropertyExperts.API.Services.External;
using PropertyExperts.API.Services.Client;
using PropertyExperts.API.Validators;

namespace PropertyExperts.API.Extensions
{
	public static class ServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddSingleton<IRestClientFactory, RestClientFactory>();
			services.AddSingleton<IRestClientRetryPolicy, RestClientRetryPolicy>();
			services.AddScoped<IInvoiceService, InvoiceService>();
			services.AddScoped<IInvoiceClassificationService, MockInvoiceClassificationService>();
			services.AddScoped<IDecisionRuleEngine, DecisionRuleEngine>();
			services.AddScoped<IValidator<InvoiceRequest>, InvoiceValidator>();
			
			return services;
		}
	}
}
