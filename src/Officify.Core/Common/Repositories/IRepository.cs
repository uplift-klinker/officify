using Officify.Core.Common.Entities;
using Officify.Models;

namespace Officify.Core.Common.Repositories;

public record QueryParameters(
    int PageSize = PagingDefaults.PageSize,
    int PageNumber = PagingDefaults.PageNumber
);

public interface IRepository<TEntity, in TQueryParameters>
    where TEntity : Entity
    where TQueryParameters : QueryParameters
{
    Task<PagedListResult<TEntity>> Query(
        TQueryParameters parameters,
        CancellationToken cancellationToken = default
    );
    Task<ListResult<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
