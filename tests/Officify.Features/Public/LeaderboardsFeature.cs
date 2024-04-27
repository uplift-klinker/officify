using Officify.Features.Support;

namespace Officify.Features.Public;

public class LeaderboardsFeature : OfficifyPageTest
{
    [Test]
    public async Task WhenNavigatingToLeaderboardThenShowsEmptyLeaders()
    {
        await Page.GotoAsync($"{BaseUrl}/leaderboard");

        await Expect(
                Page.GetByRole(AriaRole.Listitem, new PageGetByRoleOptions { Name = "leader" })
            )
            .ToHaveCountAsync(0);
    }
}
