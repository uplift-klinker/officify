using Officify.Core.Common.Queries;
using Officify.Models.Health;

namespace Officify.Core.Health.Queries;

public record HealthCheckQuery : IQuery<HealthCheckResultModel>;

public class HealthCheckQueryHandler : IQueryHandler<HealthCheckQuery, HealthCheckResultModel>
{
    public Task<HealthCheckResultModel> Handle(
        HealthCheckQuery request,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new HealthCheckResultModel("Healthy"));
    }
}
