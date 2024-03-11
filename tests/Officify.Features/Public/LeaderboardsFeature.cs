namespace Officify.Features.Public;

public class LeaderboardsFeature : PageTest
{
    [Test]
    public async Task WhenNavigatingToLeaderboardThenShowsEmptyLeaders()
    {
        await Page.GotoAsync("http://localhost:5001/leaderboard");

        await Expect(
                Page.GetByRole(AriaRole.Listitem, new PageGetByRoleOptions { Name = "leader" })
            )
            .ToHaveCountAsync(0);
    }
}
