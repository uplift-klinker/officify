using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Officify.Core.Common;
using Officify.Core.Competitions.Commands;
using Officify.Core.Competitions.Queries;
using Officify.Models.Competitions;
using Officify.Service.Host.Common;

namespace Officify.Service.Host.Competitions;

public class CompetitionsController(IMessageBus messageBus) : MessageBusController(messageBus)
{
    [Function("GetAllCompetitions")]
    public async Task<IActionResult> GetAll(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "competitions")]
            HttpRequest request
    )
    {
        return await ExecuteAsync(new GetAllCompetitionsQuery()).ConfigureAwait(false);
    }

    [Function("GetCompetitionLeaderboard")]
    public async Task<IActionResult> GetLeaderboard(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "competitions/{id:guid}/leaderboard")]
            HttpRequest request,
        Guid id
    )
    {
        var pageSize = request.GetIntQueryValueOrDefault("pageSize", 10);
        var pageNumber = request.GetIntQueryValueOrDefault("pageNumber", 1);
        return await ExecuteAsync(new GetLeaderboardForCompetitionQuery(id, pageSize, pageNumber))
            .ConfigureAwait(false);
    }

    [Function("CreateCompetition")]
    public async Task<IActionResult> CreateCompetition(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", "competitions")] HttpRequest request
    )
    {
        var model = await request.ReadContentAsJsonOrThrowAsync<CreateCompetitionModel>();
        return await ExecuteAsync(new CreateCompetitionCommand(model.Name, model.RankType))
            .ConfigureAwait(false);
    }

    [Function("CreateCompetitionResult")]
    public async Task<IActionResult> CreateCompetitionResult(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "post",
            "competitions/{competitionId:guid}/results"
        )]
            HttpRequest request,
        Guid competitionId
    )
    {
        var model = await request.ReadContentAsJsonOrThrowAsync<CreateCompetitionResultModel>();
        var command = new CreateCompetitionResultCommand(
            competitionId,
            model.CompetitorId,
            model.ResultType,
            model.Result
        );
        return await ExecuteAsync(command).ConfigureAwait(false);
    }
}
