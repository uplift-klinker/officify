using Officify.Core.Common.Queries;
using Officify.Core.Competitors.Repositories;
using Officify.Models.Competitors;

namespace Officify.Core.Competitors.Queries;

public record GetCompetitorByIdQuery(Guid CompetitorId) : IQuery<CompetitorModel?>;

internal class GetCompetitorByIdQueryHandler(ICompetitorRepository repository)
    : IQueryHandler<GetCompetitorByIdQuery, CompetitorModel?>
{
    public async Task<CompetitorModel?> Handle(
        GetCompetitorByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var entity = await repository.GetByIdAsync(request.CompetitorId, cancellationToken);
        return entity?.ToModel();
    }
}
