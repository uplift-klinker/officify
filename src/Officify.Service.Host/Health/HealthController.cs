using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Officify.Core.Health.Queries;
using Officify.Service.Host.Common;

namespace Officify.Service.Host.Health;

public class HealthController(ResponseDataBuilder responseBuilder)
{
    [Function("Health")]
    public async Task<HttpResponseData> GetHealth(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ".health")]
            HttpRequestData request,
        CancellationToken cancellationToken = default
    )
    {
        return await responseBuilder
            .UseRequest(request)
            .ExecuteAsync(new HealthCheckQuery(), cancellationToken);
    }
}
