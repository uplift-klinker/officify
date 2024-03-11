using Microsoft.AspNetCore.Mvc;
using Officify.Core.Common;
using Officify.Core.Competitions.Commands;
using Officify.Core.Competitions.Queries;
using Officify.Models.Competitions;
using Officify.Service.Host.Common;

namespace Officify.Service.Host.Competitions;

[Route("competitions")]
public class CompetitionsController(IMessageBus messageBus) : MessageBusController(messageBus)
{
    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        return await ExecuteAsync(new GetAllCompetitionsQuery()).ConfigureAwait(false);
    }

    [HttpGet("{id:guid}/leaderboard")]
    public async Task<IActionResult> GetLeaderboard(
        [FromRoute] Guid id,
        [FromQuery] int pageSize = PagingDefaults.PageSize,
        [FromQuery] int pageNumber = PagingDefaults.PageNumber
    )
    {
        return await ExecuteAsync(new GetLeaderboardForCompetitionQuery(id, pageSize, pageNumber))
            .ConfigureAwait(false);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateCompetition([FromBody] CreateCompetitionModel model)
    {
        return await ExecuteAsync(new CreateCompetitionCommand(model.Name, model.RankType))
            .ConfigureAwait(false);
    }

    [HttpPost("{competitionId:guid}/results")]
    public async Task<IActionResult> CreateCompetitionResult(
        [FromRoute] Guid competitionId,
        [FromBody] CreateCompetitionResultModel model
    )
    {
        var command = new CreateCompetitionResultCommand(
            competitionId,
            model.CompetitorId,
            model.ResultType,
            model.Result
        );
        return await ExecuteAsync(command).ConfigureAwait(false);
    }
}
