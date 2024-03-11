using System.Collections.Concurrent;
using Officify.Core.Common.Entities;
using Officify.Core.Common.Repositories;
using Officify.Models;

namespace Officify.Core.Tests.Support;

public class FakeRepository<TEntity, TQueryParameters> : IRepository<TEntity, TQueryParameters>
    where TEntity : Entity
    where TQueryParameters : QueryParameters
{
    protected readonly ConcurrentDictionary<Guid, TEntity> _entities = new();

    public virtual async Task<PagedListResult<TEntity>> Query(
        TQueryParameters queryParameters,
        CancellationToken cancellationToken = default
    )
    {
        var listResult = await GetAllAsync(cancellationToken).ConfigureAwait(false);
        return await PageResults(listResult.Items, queryParameters).ConfigureAwait(false);
    }

    public async Task<ListResult<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _entities.Values.ToListResultAsync().ConfigureAwait(false);
    }

    public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_entities.GetValueOrDefault(id));
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetByIdAsync(id, cancellationToken) != null;
    }

    public Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _entities.AddOrUpdate(entity.Id, entity, (_, __) => entity);
        return Task.FromResult(entity);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _entities.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    protected async Task<PagedListResult<TEntity>> PageResults(
        IEnumerable<TEntity> items,
        TQueryParameters parameters
    )
    {
        var pageIndex = parameters.PageNumber - 1;
        var itemsArray = items.ToArray();
        var included = itemsArray.Skip(parameters.PageSize * pageIndex).Take(parameters.PageSize);
        return await included
            .ToPagedResultAsync(parameters.PageSize, parameters.PageNumber, itemsArray.Length)
            .ConfigureAwait(false);
    }
}
