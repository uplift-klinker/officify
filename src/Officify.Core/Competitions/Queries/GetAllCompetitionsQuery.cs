using Officify.Core.Common.Queries;
using Officify.Core.Common.Repositories;
using Officify.Core.Competitions.Entities;
using Officify.Models;
using Officify.Models.Competitions;

namespace Officify.Core.Competitions.Queries;

public record GetAllCompetitionsQuery : IQuery<ListResult<CompetitionModel>>;

internal class GetAllCompetitionsQueryHandler(
    IRepository<CompetitionEntity, QueryParameters> repository
) : IQueryHandler<GetAllCompetitionsQuery, ListResult<CompetitionModel>>
{
    public async Task<ListResult<CompetitionModel>> Handle(
        GetAllCompetitionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.As(e => e.ToModel());
    }
}
