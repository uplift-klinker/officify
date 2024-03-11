using Officify.Core.Competitors.Entities;
using Officify.Core.Competitors.Repositories;
using Officify.Models;

namespace Officify.Core.Tests.Support;

public class FakeCompetitorRepository
    : FakeRepository<CompetitorEntity, CompetitorQueryParameters>,
        ICompetitorRepository
{
    public override async Task<PagedListResult<CompetitorEntity>> Query(
        CompetitorQueryParameters queryParameters,
        CancellationToken cancellationToken = default
    )
    {
        var allItems = await GetAllAsync(cancellationToken);
        var matchingItems = allItems.Items.AsEnumerable();
        if (!string.IsNullOrEmpty(queryParameters.UserId))
        {
            matchingItems = matchingItems.Where(i =>
                string.Compare(i.UserId, queryParameters.UserId, StringComparison.OrdinalIgnoreCase)
                == 0
            );
        }

        return await PageResults(matchingItems, queryParameters);
    }
}
