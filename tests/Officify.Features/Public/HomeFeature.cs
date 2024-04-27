using System.Text.RegularExpressions;
using Officify.Features.Support;

namespace Officify.Features.Public;

public class HomeFeature : OfficifyPageTest
{
    [Test]
    public async Task WhenUnauthenticatedThenNavigatesToDashboard()
    {
        await Page.GotoAsync(BaseUrl);

        await Expect(Page).ToHaveURLAsync(new Regex(".*home"));
    }

    [Test]
    public async Task WhenNavigatingToLeaderboardThenShowsLeaderboards()
    {
        await Page.GotoAsync(BaseUrl);

        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Leaderboard" })
            .ClickAsync();

        await Expect(Page).ToHaveURLAsync(new Regex(".*leaderboard"));
    }
}
