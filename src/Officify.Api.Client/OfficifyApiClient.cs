using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Officify.Models;
using Officify.Models.Leaderboard;

namespace Officify.Api.Client;

public class OfficifyApiClient(
    IHttpClientFactory clientFactory,
    IOptions<OfficifyApiClientOptions> options
)
{
    public const string HttpClientName = "OfficifyApi";
    private string BaseUrl => options.Value.BaseUrl;
    private HttpClient HttpClient => clientFactory.CreateClient(HttpClientName);

    public async Task<ListResult<LeaderboardItemModel>> GetLeaderBoardsAsync()
    {
        return await GetAsync<ListResult<LeaderboardItemModel>>("/leaderboards")
            .ConfigureAwait(false);
    }

    protected async Task<T> GetAsync<T>(string route)
    {
        var uri = GetFullUrl(route);
        var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<T>().ConfigureAwait(false);
        if (result == null)
            throw new HttpRequestException("result was 'null'", null, response.StatusCode);
        return result;
    }

    private Uri GetFullUrl(string route)
    {
        return new Uri($"{BaseUrl}{route}");
    }
}
