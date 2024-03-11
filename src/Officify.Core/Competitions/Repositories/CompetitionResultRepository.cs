using Officify.Core.Common.Repositories;
using Officify.Core.Competitions.Entities;

namespace Officify.Core.Competitions.Repositories;

public record CompetitionResultQueryParameters(
    Guid? CompetitionId = null,
    CompetitionRankType? RankType = null,
    int PageSize = 10,
    int PageNumber = 0
) : QueryParameters(PageSize, PageNumber);

public interface ICompetitionResultRepository
    : IRepository<CompetitionResultEntity, CompetitionResultQueryParameters> { }
