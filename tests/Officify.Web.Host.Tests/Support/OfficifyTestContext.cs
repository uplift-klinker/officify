using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Officify.Models;
using Officify.Models.Competitions;
using LeaderboardItemModel = Officify.Models.Leaderboard.LeaderboardItemModel;

namespace Officify.Web.Host.Tests.Support;

public class OfficifyTestContext : TestContext
{
    public string OfficifyApiUrl => "http://localhost:5000";
    public FakeHttpMessageHandler OfficifyApiHandler =>
        Services.GetRequiredService<FakeHttpClientFactory>().GetOfficifyApiHandler();

    public OfficifyTestContext()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddOfficifyWebHost(
            api =>
            {
                api.BaseUrl = OfficifyApiUrl;
            },
            mud =>
            {
                mud.SnackbarConfiguration.ShowTransitionDuration = 0;
                mud.SnackbarConfiguration.HideTransitionDuration = 0;
            }
        );
        Services.RemoveAll<IHttpClientFactory>();
        Services.AddSingleton<FakeHttpClientFactory>();
        Services.AddSingleton<IHttpClientFactory>(p =>
            p.GetRequiredService<FakeHttpClientFactory>()
        );
    }

    public async Task SetupApiGetJsonResponse<T>(string path, T value, int delayMilliseconds = 0)
    {
        await OfficifyApiHandler.SetupGetJsonResponse(
            $"{OfficifyApiUrl}{path}",
            value,
            delayMilliseconds
        );
    }

    public async Task SetupCompetitions(
        IEnumerable<CompetitionModel> competitions,
        int delayMilliseconds = 0
    )
    {
        var items = competitions.ToArray();
        await SetupApiGetJsonResponse(
            "/competitions",
            new ListResult<CompetitionModel>(items, items.Length),
            delayMilliseconds
        );
    }

    public async Task SetupLeaderboard(LeaderboardModel leaderboard, int delayMilliseconds = 0)
    {
        await SetupApiGetJsonResponse(
            $"/competitions/{leaderboard.CompetitionId}/leaderboard",
            leaderboard,
            delayMilliseconds
        );
    }
}
