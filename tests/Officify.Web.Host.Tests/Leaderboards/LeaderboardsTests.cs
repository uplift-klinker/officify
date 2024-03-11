using Officify.Models;
using Officify.Models.Leaderboard;

namespace Officify.Web.Host.Tests.Leaderboards;

public class LeaderboardsTests : OfficifyTestContext
{
    [Fact]
    public void WhenRenderedThenShowsLoadingLeaders()
    {
        SetupApiGetJsonResponse("/leaderboards", Array.Empty<LeaderboardItemModel>(), 1000);

        var leaderboards = RenderComponent<Pages.Leaderboards>();

        leaderboards.FindAllByRole("progressbar").Should().HaveCount(1);
        leaderboards.FindAllByRole("listitem").Should().BeEmpty();
    }

    [Fact]
    public void WhenLeaderboardsAreLoadedThenShowsLeaderboard()
    {
        var result = new ListResult<LeaderboardItemModel>(
            [
                new LeaderboardItemModel(Guid.NewGuid(), 1, "Sally", "12 minutes"),
                new LeaderboardItemModel(Guid.NewGuid(), 2, "Bob", "12.5 minutes"),
            ],
            4
        );
        SetupApiGetJsonResponse("/leaderboards", result);

        var leaderboards = RenderComponent<Pages.Leaderboards>();

        leaderboards.WaitForAssertion(() =>
        {
            leaderboards.FindAllByRole("listitem").Should().HaveCount(2);
        });
        leaderboards.FindAllByRole("progressbar").Should().BeEmpty();
    }
}
