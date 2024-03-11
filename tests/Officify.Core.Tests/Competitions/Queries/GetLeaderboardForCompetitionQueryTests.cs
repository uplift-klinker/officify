using Microsoft.Extensions.DependencyInjection;
using Officify.Core.Common;
using Officify.Core.Common.Exceptions;
using Officify.Core.Common.Repositories;
using Officify.Core.Competitions.Entities;
using Officify.Core.Competitions.Queries;
using Officify.Core.Competitions.Repositories;
using Officify.Core.Competitors.Entities;
using Officify.Core.Competitors.Repositories;
using Officify.Core.Tests.Support;

namespace Officify.Core.Tests.Competitions.Queries;

public class GetLeaderboardForCompetitionQueryTests
{
    private readonly IRepository<CompetitionEntity, QueryParameters> _competitionRepository;
    private readonly ICompetitorRepository _competitorRepository;
    private readonly ICompetitionResultRepository _resultsRepository;
    private readonly IMessageBus _messageBus;

    public GetLeaderboardForCompetitionQueryTests()
    {
        var provider = OfficifyCoreServiceProviderFactory.Create();
        _competitionRepository = provider.GetRequiredService<
            IRepository<CompetitionEntity, QueryParameters>
        >();
        _competitorRepository = provider.GetRequiredService<ICompetitorRepository>();
        _resultsRepository = provider.GetRequiredService<ICompetitionResultRepository>();
        _messageBus = provider.GetRequiredService<IMessageBus>();
    }

    [Fact]
    public async Task WhenGettingLeaderboardForMissingCompetitionThenThrowsError()
    {
        var query = new GetLeaderboardForCompetitionQuery(Guid.NewGuid(), 10, 0);
        var act = () => _messageBus.ExecuteAsync(query);

        await act.Should().ThrowAsync<EntityNotFoundException<CompetitionEntity>>();
    }

    [Fact]
    public async Task WhenGettingLeaderboardForCompetitionThenPopulatesLeaderboardFromCompetition()
    {
        var competition = await _competitionRepository.SaveAsync(
            new CompetitionEntity { Name = "Baba ghanoush" }
        );
        var result = await _messageBus.ExecuteAsync(
            new GetLeaderboardForCompetitionQuery(competition.Id)
        );

        result.CompetitionId.Should().Be(competition.Id);
        result.CompetitionName.Should().Be("Baba ghanoush");
    }

    [Fact]
    public async Task WhenGettingLeaderboardForCompetitionWithHighestScoreRankTypeThenReturnsResultsSortedHighestToLowest()
    {
        var competition = await _competitionRepository.SaveAsync(
            new CompetitionEntity { RankType = CompetitionRankType.HighestScore }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { Result = 200, CompetitionId = competition.Id }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { Result = 1000, CompetitionId = competition.Id }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { Result = 500, CompetitionId = competition.Id }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { Result = 900, CompetitionId = competition.Id }
        );

        var result = await _messageBus.ExecuteAsync(
            new GetLeaderboardForCompetitionQuery(competition.Id)
        );

        var firstItem = result.Items[0];
        firstItem.Rank.Should().Be(1);
        firstItem.Result.Should().Be(1000);
    }

    [Fact]
    public async Task WhenGettingLeaderboardForCompetitionWithLowestScoreRankTypeThenReturnsResultsSortedLowestToHighest()
    {
        var competition = await _competitionRepository.SaveAsync(
            new CompetitionEntity { RankType = CompetitionRankType.LowestScore }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { Result = 200, CompetitionId = competition.Id }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { Result = 1000, CompetitionId = competition.Id }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { Result = 500, CompetitionId = competition.Id }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { Result = 900, CompetitionId = competition.Id }
        );

        var result = await _messageBus.ExecuteAsync(
            new GetLeaderboardForCompetitionQuery(competition.Id)
        );

        var firstItem = result.Items[0];
        firstItem.Rank.Should().Be(1);
        firstItem.Result.Should().Be(200);
    }

    [Fact]
    public async Task WhenGettingLeaderboardForCompetitionThenPopulatesCompetitorCodename()
    {
        var competition = await _competitionRepository.SaveAsync(new CompetitionEntity());
        var competitor = await _competitorRepository.SaveAsync(
            new CompetitorEntity { Codename = "Maverick" }
        );
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity
            {
                CompetitionId = competition.Id,
                CompetitorId = competitor.Id
            }
        );

        var result = await _messageBus.ExecuteAsync(
            new GetLeaderboardForCompetitionQuery(competition.Id)
        );

        var item = result.Items[0];
        item.CompetitorCodename.Should().Be("Maverick");
    }

    [Fact]
    public async Task WhenResultCompetitorIsMissingThenPopulatesCompetitorCodenameWithMissing()
    {
        var competition = await _competitionRepository.SaveAsync(new CompetitionEntity());
        await _resultsRepository.SaveAsync(
            new CompetitionResultEntity { CompetitionId = competition.Id }
        );

        var result = await _messageBus.ExecuteAsync(
            new GetLeaderboardForCompetitionQuery(competition.Id)
        );

        var item = result.Items[0];
        item.CompetitorCodename.Should().Be("(missing)");
    }

    [Fact]
    public async Task WhenGettingSecondPageOfLeaderboardThenReturnsSecondPageOfLeaders()
    {
        var competition = await _competitionRepository.SaveAsync(
            new CompetitionEntity { RankType = CompetitionRankType.HighestScore }
        );
        await AddManyResults(competition.Id, 500);

        var result = await _messageBus.ExecuteAsync(
            new GetLeaderboardForCompetitionQuery(
                CompetitionId: competition.Id,
                PageNumber: 2,
                PageSize: 10
            )
        );

        result.Items[0].Result.Should().Be(489);
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(500);
    }

    private async Task AddManyResults(Guid competitionId, int count)
    {
        var tasks = Enumerable
            .Range(0, count)
            .Select(i =>
                _resultsRepository.SaveAsync(
                    new CompetitionResultEntity { CompetitionId = competitionId, Result = i }
                )
            );
        await Task.WhenAll(tasks);
    }
}
