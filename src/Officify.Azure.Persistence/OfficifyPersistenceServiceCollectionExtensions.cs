using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Officify.Azure.Persistence.Common.Options;
using Officify.Azure.Persistence.Common.Repositories;
using Officify.Azure.Persistence.Competitions;
using Officify.Azure.Persistence.Competitors;
using Officify.Core.Common.Repositories;
using Officify.Core.Competitions.Repositories;
using Officify.Core.Competitors.Repositories;

namespace Officify.Azure.Persistence;

public static class OfficifyPersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddOfficifyAzurePersistence(
        this IServiceCollection services,
        string storageAccountConnectionString,
        Action<AzurePersistenceOptions> configure
    )
    {
        services.AddAzureClients(b =>
        {
            b.AddTableServiceClient(storageAccountConnectionString);
        });
        services.AddOptions().Configure(configure);
        services.AddScoped(typeof(IRepository<,>), typeof(AzureTableStorageRepository<,>));
        services.AddScoped<
            ICompetitionResultRepository,
            AzureTableStorageCompetitionResultRepository
        >();
        services.AddScoped<ICompetitorRepository, AzureTableStorageCompetitorRepository>();
        return services;
    }
}
