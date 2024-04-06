using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Officify.Core.Competitions.Commands;
using Officify.Core.Competitions.Queries;
using Officify.Models.Competitions;
using Officify.Service.Host.Common;

namespace Officify.Service.Host.Competitions;

public class CompetitionsController(ResponseDataBuilder responseBuilder)
{
    [Function("GetAllCompetitions")]
    public async Task<HttpResponseData> GetAll(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "competitions")]
            HttpRequestData request,
        CancellationToken cancellationToken = default
    )
    {
        return await responseBuilder
            .UseRequest(request)
            .ExecuteAsync(new GetAllCompetitionsQuery(), cancellationToken);
    }

    [Function("GetCompetitionLeaderboard")]
    public async Task<HttpResponseData> GetLeaderboard(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "get",
            Route = "competitions/{id:guid}/leaderboard"
        )]
            HttpRequestData request,
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var pageSize = request.GetIntQueryValueOrDefault("pageSize", 10);
        var pageNumber = request.GetIntQueryValueOrDefault("pageNumber", 1);
        var query = new GetLeaderboardForCompetitionQuery(id, pageSize, pageNumber);
        return await responseBuilder.UseRequest(request).ExecuteAsync(query, cancellationToken);
    }

    [Function("CreateCompetition")]
    public async Task<HttpResponseData> CreateCompetition(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "competitions")]
            HttpRequestData request,
        CancellationToken cancellationToken = default
    )
    {
        var model = await request.ReadContentAsJsonOrThrowAsync<CreateCompetitionModel>(
            cancellationToken
        );
        var command = new CreateCompetitionCommand(model.Name, model.RankType);
        return await responseBuilder.UseRequest(request).ExecuteAsync(command, cancellationToken);
    }

    [Function("CreateCompetitionResult")]
    public async Task<HttpResponseData> CreateCompetitionResult(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "post",
            Route = "competitions/{competitionId:guid}/results"
        )]
            HttpRequestData request,
        Guid competitionId,
        CancellationToken cancellationToken = default
    )
    {
        var model = await request.ReadContentAsJsonOrThrowAsync<CreateCompetitionResultModel>(
            cancellationToken
        );
        var command = new CreateCompetitionResultCommand(
            competitionId,
            model.CompetitorId,
            model.ResultType,
            model.Result
        );
        return await responseBuilder.UseRequest(request).ExecuteAsync(command, cancellationToken);
    }
}
