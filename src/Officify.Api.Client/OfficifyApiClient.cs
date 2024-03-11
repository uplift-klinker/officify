using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Officify.Models;
using Officify.Models.Competitions;
using LeaderboardItemModel = Officify.Models.Leaderboard.LeaderboardItemModel;

namespace Officify.Api.Client;

public class OfficifyApiClient(
    IHttpClientFactory clientFactory,
    IOptions<OfficifyApiClientOptions> options
)
{
    public const string HttpClientName = "OfficifyApi";
    private string BaseUrl => options.Value.BaseUrl;
    private HttpClient HttpClient => clientFactory.CreateClient(HttpClientName);

    public async Task<ListResult<CompetitionModel>> GetCompetitionsAsync()
    {
        return await GetAsync<ListResult<CompetitionModel>>("/competitions").ConfigureAwait(false);
    }

    public async Task<LeaderboardModel> GetLeaderboardAsync(GetLeaderboardModel model)
    {
        var route =
            $"/competitions/{model.CompetitionId}/leaderboard?pageSize={model.PageSize}&pageNumber={model.PageNumber}";
        return await GetAsync<LeaderboardModel>(route).ConfigureAwait(false);
    }

    public async Task<CompetitionModel> CreateCompetition(CreateCompetitionModel model)
    {
        return await PostAsync<CreateCompetitionModel, CompetitionModel>("/competitions", model);
    }

    public async Task<CompetitionResultModel> CreateCompetitionResult(
        CreateCompetitionResultModel model
    )
    {
        var route = $"/competitions/{model.CompetitionId}/results";
        return await PostAsync<CreateCompetitionResultModel, CompetitionResultModel>(route, model);
    }

    public async Task<ListResult<LeaderboardItemModel>> GetLeaderBoardsAsync()
    {
        return await GetAsync<ListResult<LeaderboardItemModel>>("/leaderboards")
            .ConfigureAwait(false);
    }

    private async Task<T> GetAsync<T>(string route)
    {
        var uri = GetFullUrl(route);
        var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<T>().ConfigureAwait(false);
        if (result == null)
            throw new HttpRequestException("result was 'null'", null, response.StatusCode);
        return result;
    }

    private async Task<TResult> PostAsync<TRequest, TResult>(string route, TRequest request)
    {
        var uri = GetFullUrl(route);
        var response = await HttpClient.PostAsJsonAsync(uri, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false);
        if (result == null)
            throw new HttpRequestException("result was 'null'", null, response.StatusCode);
        return result;
    }

    private Uri GetFullUrl(string route)
    {
        return new Uri($"{BaseUrl}{route}");
    }
}
