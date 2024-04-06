using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Officify.Core.Competitors.Commands;
using Officify.Core.Competitors.Queries;
using Officify.Models.Competitors;
using Officify.Service.Host.Common;

namespace Officify.Service.Host.Competitors;

public class CompetitorsController(ResponseDataBuilder responseBuilder)
{
    [Function("GetCompetitorById")]
    public async Task<HttpResponseData> GetCompetitorById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "competitors/{id:guid}")]
            HttpRequestData request,
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var query = new GetCompetitorByIdQuery(id);
        return await responseBuilder.UseRequest(request).ExecuteAsync(query, cancellationToken);
    }

    [Function("GetCompetitorByUserId")]
    public async Task<HttpResponseData> GetCompetitorByIdOrUserId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "competitors/{userId}")]
            HttpRequestData request,
        string userId,
        CancellationToken cancellationToken = default
    )
    {
        var query = new GetCompetitorByUserIdQuery(userId);
        return await responseBuilder.UseRequest(request).ExecuteAsync(query, cancellationToken);
    }

    [Function("CreateCompetitor")]
    public async Task<HttpResponseData> CreateCompetitor(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "competitors")]
            HttpRequestData request,
        CancellationToken cancellationToken = default
    )
    {
        var model = await request.ReadContentAsJsonOrThrowAsync<CreateCompetitorModel>(
            cancellationToken
        );
        var command = new CreateCompetitorCommand(model.Codename, model.UserId);
        return await responseBuilder.UseRequest(request).ExecuteAsync(command, cancellationToken);
    }
}
