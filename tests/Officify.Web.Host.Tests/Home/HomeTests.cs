using AngleSharp.Dom;
using Bunit;

namespace Officify.Web.Host.Tests.Home;

public class HomeTests : OfficifyTestContext
{
    [Fact]
    public void WhenRenderedThenShowsLeaderboardLink()
    {
        var homePage = RenderComponent<Pages.Home>();

        var links = homePage.FindAll("a");
        links.Should().Contain(e => e.GetInnerText().Contains("Leaderboard"));
    }
}
