using MudBlazor.Services;
using Officify.Api.Client;

namespace Officify.Web.Host;

public static class OfficifyWebHostServiceCollectionExtensions
{
    public static IServiceCollection AddOfficifyWebHost(
        this IServiceCollection services,
        Action<OfficifyApiClientOptions> configureApi,
        Action<MudServicesConfiguration>? configureMud = null
    )
    {
        services.AddOfficifyApiClient(configureApi);
        if (configureMud != null)
            services.AddMudServices(configureMud);
        else
            services.AddMudServices();

        return services;
    }
}
