using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Officify.Core.Common;
using Officify.Core.Competitors.Commands;
using Officify.Core.Competitors.Queries;
using Officify.Models.Competitors;
using Officify.Service.Host.Common;

namespace Officify.Service.Host.Competitors;

public class CompetitorsController(IMessageBus messageBus) : MessageBusController(messageBus)
{
    [Function("GetCompetitorById")]
    public async Task<IActionResult> GetCompetitorById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "competitors/{id:guid}")]
            HttpRequest request,
        Guid id
    )
    {
        return await ExecuteAsync(new GetCompetitorByIdQuery(id)).ConfigureAwait(false);
    }

    [Function("GetCompetitorByUserId")]
    public async Task<IActionResult> GetCompetitorByIdOrUserId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "competitors/{userId}")]
            HttpRequest request,
        string userId
    )
    {
        return await ExecuteAsync(new GetCompetitorByUserIdQuery(userId)).ConfigureAwait(false);
    }

    [Function("CreateCompetitor")]
    public async Task<IActionResult> CreateCompetitor(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "competitors")]
            HttpRequest request
    )
    {
        var model = await request.ReadContentAsJsonOrThrowAsync<CreateCompetitorModel>();
        return await ExecuteAsync(new CreateCompetitorCommand(model.Codename, model.UserId))
            .ConfigureAwait(false);
    }
}
