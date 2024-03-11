using System.Text.RegularExpressions;

namespace Officify.Features.Public;

public class HomeFeature : PageTest
{
    [Test]
    public async Task WhenUnauthenticatedThenNavigatesToDashboard()
    {
        await Page.GotoAsync("http://localhost:5001/");

        await Expect(Page).ToHaveURLAsync(new Regex(".*home"));
    }

    [Test]
    public async Task WhenNavigatingToLeaderboardThenShowsLeaderboards()
    {
        await Page.GotoAsync("http://localhost:5001");

        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Leaderboard" })
            .ClickAsync();

        await Expect(Page).ToHaveURLAsync(new Regex(".*leaderboard"));
    }
}
