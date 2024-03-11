using Microsoft.Extensions.DependencyInjection;
using Officify.Core.Common.Repositories;
using Officify.Core.Competitions.Repositories;
using Officify.Core.Competitors.Repositories;

namespace Officify.Core.Tests.Support;

public static class OfficifyCoreServiceProviderFactory
{
    public static IServiceProvider Create()
    {
        return new ServiceCollection()
            .AddOfficifyCore()
            .AddOfficifyCoreFakes()
            .BuildServiceProvider();
    }

    private static IServiceCollection AddOfficifyCoreFakes(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IRepository<,>), typeof(FakeRepository<,>));
        services.AddSingleton<ICompetitionResultRepository, FakeCompetitionResultRepository>();
        services.AddSingleton<ICompetitorRepository, FakeCompetitorRepository>();
        return services;
    }
}
