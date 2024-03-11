using Officify.Core.Competitions.Entities;
using Officify.Core.Competitions.Repositories;
using Officify.Models;

namespace Officify.Core.Tests.Support;

public class FakeCompetitionResultRepository
    : FakeRepository<CompetitionResultEntity, CompetitionResultQueryParameters>,
        ICompetitionResultRepository
{
    public override async Task<PagedListResult<CompetitionResultEntity>> Query(
        CompetitionResultQueryParameters parameters,
        CancellationToken cancellationToken = default
    )
    {
        var allResult = await GetAllAsync(cancellationToken);
        var matchingItems = allResult.Items.AsEnumerable();
        if (parameters.CompetitionId.HasValue)
            matchingItems = matchingItems.Where(e =>
                e.CompetitionId == parameters.CompetitionId.Value
            );

        if (parameters.RankType.HasValue)
        {
            matchingItems =
                parameters.RankType.Value == CompetitionRankType.HighestScore
                    ? matchingItems.OrderByDescending(i => i.Result)
                    : matchingItems.OrderBy(i => i.Result);
        }

        return await PageResults(matchingItems, parameters);
    }
}
