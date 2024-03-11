using Microsoft.Extensions.DependencyInjection;

namespace Officify.Api.Client;

public static class OfficifyApiClientServiceCollectionExtensions
{
    public static IServiceCollection AddOfficifyApiClient(
        this IServiceCollection services,
        Action<OfficifyApiClientOptions> configure
    )
    {
        services.AddHttpClient(OfficifyApiClient.HttpClientName);
        services.AddOptions<OfficifyApiClientOptions>().Configure(configure);
        services.AddScoped<OfficifyApiClient>();
        return services;
    }
}
