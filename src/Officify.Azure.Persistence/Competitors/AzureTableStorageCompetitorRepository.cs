using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using Officify.Azure.Persistence.Common.Options;
using Officify.Azure.Persistence.Common.Repositories;
using Officify.Core.Competitors.Entities;
using Officify.Core.Competitors.Repositories;
using Officify.Models;

namespace Officify.Azure.Persistence.Competitors;

public class AzureTableStorageCompetitorRepository(
    TableServiceClient tableService,
    IOptions<AzurePersistenceOptions> options
)
    : AzureTableStorageRepository<CompetitorEntity, CompetitorQueryParameters>(
        tableService,
        options
    ),
        ICompetitorRepository
{
    public override async Task<PagedListResult<CompetitorEntity>> Query(
        CompetitorQueryParameters parameters,
        CancellationToken cancellationToken = default
    )
    {
        var userId = parameters.UserId;
        var pageable = await QueryTableAsync(
                t =>
                    t.PartitionKey == PartitionKey
                    && (userId == null || t.GetString("UserId") == userId),
                cancellationToken
            )
            .ConfigureAwait(false);

        return await PageResults(pageable, parameters, cancellationToken).ConfigureAwait(false);
    }
}
