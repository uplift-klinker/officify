using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using Officify.Azure.Persistence.Common.Options;
using Officify.Azure.Persistence.Common.Repositories;
using Officify.Core.Competitions.Entities;
using Officify.Core.Competitions.Repositories;
using Officify.Models;

namespace Officify.Azure.Persistence.Competitions;

public class AzureTableStorageCompetitionResultRepository(
    TableServiceClient tableService,
    IOptions<AzurePersistenceOptions> options
)
    : AzureTableStorageRepository<CompetitionResultEntity, CompetitionResultQueryParameters>(
        tableService,
        options
    ),
        ICompetitionResultRepository
{
    public override async Task<PagedListResult<CompetitionResultEntity>> Query(
        CompetitionResultQueryParameters parameters,
        CancellationToken cancellationToken = default
    )
    {
        var rankType = parameters.RankType?.ToString();
        var competitionId = parameters.CompetitionId;
        var pageable = await QueryTableAsync(
                t =>
                    t.PartitionKey != PartitionKey
                    && (competitionId == null || t.GetGuid("CompetitionId") == competitionId)
                    && (rankType == null || t.GetString("RankType") == rankType),
                cancellationToken: cancellationToken
            )
            .ConfigureAwait(false);
        return await PageResults(pageable, parameters, cancellationToken).ConfigureAwait(false);
    }
}
