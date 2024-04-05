using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Officify.Core.Common;
using Officify.Core.Health.Queries;
using Officify.Service.Host.Common;

namespace Officify.Service.Host.Health;

public class HealthController(IMessageBus messageBus) : MessageBusController(messageBus)
{
    [Function("Health")]
    public async Task<IActionResult> GetHealth(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ".health")] HttpRequest request
    )
    {
        return await ExecuteAsync(new HealthCheckQuery());
    }
}
