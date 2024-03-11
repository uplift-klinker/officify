using Microsoft.Extensions.DependencyInjection;
using Officify.Core.Common;
using Officify.Core.Common.Repositories;
using Officify.Core.Competitions.Entities;
using Officify.Core.Competitions.Queries;
using Officify.Core.Competitions.Repositories;
using Officify.Core.Tests.Support;

namespace Officify.Core.Tests.Competitions.Queries;

public class GetCompetitionResultsQueryTests : IAsyncLifetime
{
    private readonly CompetitionEntity _competition;
    private readonly IRepository<CompetitionEntity, QueryParameters> _competitionRepository;
    private readonly ICompetitionResultRepository _resultsRepository;
    private readonly IMessageBus _messageBus;

    public GetCompetitionResultsQueryTests()
    {
        _competition = new CompetitionEntity();

        var provider = OfficifyCoreServiceProviderFactory.Create();
        _competitionRepository = provider.GetRequiredService<
            IRepository<CompetitionEntity, QueryParameters>
        >();
        _resultsRepository = provider.GetRequiredService<ICompetitionResultRepository>();
        _messageBus = provider.GetRequiredService<IMessageBus>();
    }

    public async Task InitializeAsync()
    {
        await _competitionRepository.SaveAsync(_competition);
    }

    [Fact]
    public async Task WhenGettingResultsForCompetitionThenReturnsResultsForProvidedCompetition()
    {
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { CompetitionId = _competition.Id }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { CompetitionId = Guid.NewGuid() }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { CompetitionId = _competition.Id }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { CompetitionId = _competition.Id }
        );

        var query = new GetCompetitionResultsQuery(_competition.Id);
        var result = await _messageBus.ExecuteAsync(query);

        result.Items.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);
    }

    [Fact]
    public async Task WhenGettingResultsForCompetitionThenReturnsPagedResults()
    {
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { CompetitionId = _competition.Id }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { CompetitionId = _competition.Id }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { CompetitionId = Guid.NewGuid() }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { CompetitionId = _competition.Id }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { CompetitionId = _competition.Id }
        );

        var query = new GetCompetitionResultsQuery(_competition.Id, 2, 1);
        var result = await _messageBus.ExecuteAsync(query);

        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(4);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
