using Microsoft.AspNetCore.Mvc;
using Officify.Core.Common;
using Officify.Core.Competitors.Commands;
using Officify.Core.Competitors.Queries;
using Officify.Models.Competitors;
using Officify.Service.Host.Common;

namespace Officify.Service.Host.Competitors;

[Route("competitors")]
public class CompetitorsController(IMessageBus messageBus) : MessageBusController(messageBus)
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCompetitorById(Guid id)
    {
        return await ExecuteAsync(new GetCompetitorByIdQuery(id)).ConfigureAwait(false);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCompetitorByIdOrUserId(string userId)
    {
        return await ExecuteAsync(new GetCompetitorByUserIdQuery(userId)).ConfigureAwait(false);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateCompetitor([FromBody] CreateCompetitorModel model)
    {
        return await ExecuteAsync(new CreateCompetitorCommand(model.Codename, model.UserId))
            .ConfigureAwait(false);
    }
}
