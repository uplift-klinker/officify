using Officify.Core.Common;
using Officify.Core.Common.Repositories;
using Officify.Core.Competitors.Entities;

namespace Officify.Core.Competitors.Repositories;

public record CompetitorQueryParameters(
    string? UserId = null,
    int PageSize = PagingDefaults.PageSize,
    int PageNumber = PagingDefaults.PageNumber
) : QueryParameters(PageSize, PageNumber);

public interface ICompetitorRepository
    : IRepository<CompetitorEntity, CompetitorQueryParameters> { }
