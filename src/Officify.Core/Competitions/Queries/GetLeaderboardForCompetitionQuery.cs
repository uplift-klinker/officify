using Officify.Core.Common;
using Officify.Core.Common.Exceptions;
using Officify.Core.Common.Queries;
using Officify.Core.Common.Repositories;
using Officify.Core.Competitions.Entities;
using Officify.Core.Competitions.Repositories;
using Officify.Core.Competitors.Repositories;
using Officify.Models.Competitions;

namespace Officify.Core.Competitions.Queries;

public record GetLeaderboardForCompetitionQuery(
    Guid CompetitionId,
    int PageSize = PagingDefaults.PageSize,
    int PageNumber = PagingDefaults.PageNumber
) : IQuery<LeaderboardModel>;

public class GetLeaderboardForCompetitionQueryHandler(
    IRepository<CompetitionEntity, QueryParameters> competitionRepository,
    ICompetitorRepository competitorRepository,
    ICompetitionResultRepository resultsRepository
) : IQueryHandler<GetLeaderboardForCompetitionQuery, LeaderboardModel>
{
    private const string DefaultCodename = "(missing)";

    public async Task<LeaderboardModel> Handle(
        GetLeaderboardForCompetitionQuery request,
        CancellationToken cancellationToken
    )
    {
        var competition = await competitionRepository.GetByIdAsync(
            request.CompetitionId,
            cancellationToken
        );
        if (competition == null)
            throw new EntityNotFoundException<CompetitionEntity>(request.CompetitionId);

        var parameters = new CompetitionResultQueryParameters(
            CompetitionId: competition.Id,
            RankType: competition.RankType,
            PageSize: request.PageSize,
            PageNumber: request.PageNumber
        );
        var results = await resultsRepository
            .Query(parameters, cancellationToken)
            .ConfigureAwait(false);
        var tasks = results.Items.Select(
            (item, index) => CreateLeaderboardItemFromResult(item, index, cancellationToken)
        );
        var items = await Task.WhenAll(tasks).ConfigureAwait(false);
        return new LeaderboardModel(
            competition.Id,
            competition.Name,
            items.ToArray(),
            request.PageSize,
            request.PageNumber,
            results.TotalCount
        );
    }

    private async Task<LeaderboardItemModel> CreateLeaderboardItemFromResult(
        CompetitionResultEntity result,
        int index,
        CancellationToken cancellationToken
    )
    {
        var competitor = await competitorRepository
            .GetByIdAsync(result.CompetitorId, cancellationToken)
            .ConfigureAwait(false);
        return new LeaderboardItemModel(
            index + 1,
            result.CompetitorId,
            result.Id,
            competitor?.Codename ?? DefaultCodename,
            result.ResultType.ToModel(),
            result.Result
        );
    }
}
