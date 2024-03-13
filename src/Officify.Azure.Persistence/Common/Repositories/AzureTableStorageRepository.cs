using System.Linq.Expressions;
using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using Officify.Azure.Persistence.Common.Options;
using Officify.Core;
using Officify.Core.Common.Entities;
using Officify.Core.Common.Repositories;
using Officify.Models;

namespace Officify.Azure.Persistence.Common.Repositories;

public class AzureTableStorageRepository<TEntity, TQueryParameters>(
    TableServiceClient tableService,
    IOptions<AzurePersistenceOptions> options
) : IRepository<TEntity, TQueryParameters>
    where TEntity : Entity
    where TQueryParameters : QueryParameters
{
    private string TableName => options.Value.TableName;
    protected string PartitionKey => typeof(TEntity).Name;

    public virtual async Task<PagedListResult<TEntity>> Query(
        TQueryParameters parameters,
        CancellationToken cancellationToken = default
    )
    {
        var pageable = await QueryTableAsync(e => e.PartitionKey == PartitionKey, cancellationToken)
            .ConfigureAwait(false);
        return await PageResults(pageable, parameters, cancellationToken);
    }

    public async Task<ListResult<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        var entities = new List<TEntity>();
        var pageable = await QueryTableAsync(t => t.PartitionKey == PartitionKey, cancellationToken)
            .ConfigureAwait(false);
        await foreach (var tableEntity in pageable.ConfigureAwait(false))
        {
            var entity = tableEntity.ToEntity<TEntity>();
            if (entity == null)
                continue;
            entities.Add(entity);
        }

        return await entities.ToListResultAsync().ConfigureAwait(false);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await GetTableEntityById(id, cancellationToken);
        return result.Value?.ToEntity<TEntity>();
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await GetTableEntityById(id, cancellationToken);
        return result.Value != null;
    }

    public async Task<TEntity> SaveAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        var tableEntity = new TableEntity(PartitionKey, entity.Id.ToRowKey());
        tableEntity.PopulateFrom(entity);
        var tableClient = await EnsureTableExistsAsync(cancellationToken).ConfigureAwait(false);
        await tableClient
            .UpsertEntityAsync(tableEntity, TableUpdateMode.Replace, cancellationToken)
            .ConfigureAwait(false);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tableClient = await EnsureTableExistsAsync(cancellationToken).ConfigureAwait(false);
        await tableClient
            .DeleteEntityAsync(PartitionKey, id.ToRowKey(), cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<TableClient> EnsureTableExistsAsync(CancellationToken cancellationToken)
    {
        await tableService
            .CreateTableIfNotExistsAsync(TableName, cancellationToken)
            .ConfigureAwait(false);
        return tableService.GetTableClient(TableName);
    }

    private async Task<NullableResponse<TableEntity>> GetTableEntityById(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var tableClient = await EnsureTableExistsAsync(cancellationToken).ConfigureAwait(false);
        return await tableClient
            .GetEntityIfExistsAsync<TableEntity>(
                partitionKey: PartitionKey,
                rowKey: id.ToRowKey(),
                cancellationToken: cancellationToken
            )
            .ConfigureAwait(false);
    }

    protected async Task<AsyncPageable<TableEntity>> QueryTableAsync(
        Expression<Func<TableEntity, bool>> filter,
        CancellationToken cancellationToken
    )
    {
        var tableClient = await EnsureTableExistsAsync(cancellationToken).ConfigureAwait(false);
        return tableClient.QueryAsync(filter, cancellationToken: cancellationToken);
    }

    protected async Task<PagedListResult<TEntity>> PageResults(
        AsyncPageable<TableEntity> pageable,
        TQueryParameters parameters,
        CancellationToken cancellationToken
    )
    {
        var totalCount = 0;
        var skipCount = parameters.PageNumber - 1 * parameters.PageSize;
        var takeCount = parameters.PageSize;
        var entities = new List<TEntity>();
        await foreach (var page in pageable.WithCancellation(cancellationToken))
        {
            totalCount++;
            if (skipCount > 0 || entities.Count >= takeCount)
            {
                skipCount--;
                continue;
            }

            var entity = page.ToEntity<TEntity>();
            if (entity != null)
                entities.Add(entity);
        }

        return new PagedListResult<TEntity>(
            entities.ToArray(),
            parameters.PageSize,
            parameters.PageNumber,
            totalCount
        );
    }
}
