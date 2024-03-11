using Officify.Core.Common.Queries;
using Officify.Core.Competitors.Repositories;
using Officify.Models.Competitors;

namespace Officify.Core.Competitors.Queries;

public record GetCompetitorByUserIdQuery(string UserId) : IQuery<CompetitorModel?>;

internal class GetCompetitorByUserIdQueryHandler(ICompetitorRepository repository)
    : IQueryHandler<GetCompetitorByUserIdQuery, CompetitorModel?>
{
    public async Task<CompetitorModel?> Handle(
        GetCompetitorByUserIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var parameters = new CompetitorQueryParameters(request.UserId);
        var result = await repository.Query(parameters, cancellationToken);
        var entity = result.Items.FirstOrDefault();
        return entity?.ToModel();
    }
}
