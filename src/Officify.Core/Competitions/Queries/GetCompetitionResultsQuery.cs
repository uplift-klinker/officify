using Officify.Core.Common;
using Officify.Core.Common.Queries;
using Officify.Core.Competitions.Repositories;
using Officify.Models;
using Officify.Models.Competitions;

namespace Officify.Core.Competitions.Queries;

public record GetCompetitionResultsQuery(
    Guid CompetitionId,
    int PageSize = PagingDefaults.PageSize,
    int PageNumber = PagingDefaults.PageNumber
) : IQuery<PagedListResult<CompetitionResultModel>>;

public class GetCompetitionResultsQueryHandler(ICompetitionResultRepository repository)
    : IQueryHandler<GetCompetitionResultsQuery, PagedListResult<CompetitionResultModel>>
{
    public async Task<PagedListResult<CompetitionResultModel>> Handle(
        GetCompetitionResultsQuery request,
        CancellationToken cancellationToken
    )
    {
        var parameters = new CompetitionResultQueryParameters(
            CompetitionId: request.CompetitionId,
            PageSize: request.PageSize,
            PageNumber: request.PageNumber
        );
        var result = await repository.Query(parameters, cancellationToken).ConfigureAwait(false);

        return result.As(r => r.ToModel());
    }
}
