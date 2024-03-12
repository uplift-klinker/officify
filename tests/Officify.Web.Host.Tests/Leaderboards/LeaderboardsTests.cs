using Officify.Models.Competitions;

namespace Officify.Web.Host.Tests.Leaderboards;

public class LeaderboardsTests : OfficifyTestContext
{
    [Fact]
    public async Task WhenRenderedThenShowsLoadingCompetitions()
    {
        await SetupCompetitions(Array.Empty<CompetitionModel>(), 1000);

        var leaderboards = RenderComponent<Pages.Leaderboards>();

        leaderboards.FindAllByRole("progressbar").Should().HaveCount(1);
        leaderboards.FindAllByRole("listitem").Should().BeEmpty();
    }

    [Fact]
    public async Task WhenLeaderboardsAreLoadedThenShowsLeaderboard()
    {
        var competition = new CompetitionModel(
            Guid.NewGuid(),
            "",
            CompetitionRankTypeModel.HighestScore
        );
        await SetupCompetitions([competition]);
        var leaderboard = new LeaderboardModel(
            competition.Id,
            "",
            [
                new LeaderboardItemModel(
                    1,
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Maverick",
                    CompetitionResultTypeModel.Duration,
                    100
                ),
                new LeaderboardItemModel(
                    2,
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Goose",
                    CompetitionResultTypeModel.Duration,
                    90
                ),
                new LeaderboardItemModel(
                    3,
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Ice Man",
                    CompetitionResultTypeModel.Duration,
                    80
                ),
                new LeaderboardItemModel(
                    4,
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Rooster",
                    CompetitionResultTypeModel.Duration,
                    70
                ),
            ],
            10,
            1,
            4
        );
        await SetupLeaderboard(leaderboard);

        var leaderboards = RenderComponent<Pages.Leaderboards>();

        leaderboards.WaitForAssertion(() =>
        {
            leaderboards.FindAllByRole("listitem").Should().HaveCount(4);
        });
        leaderboards.FindAllByRole("progressbar").Should().BeEmpty();
    }
}
