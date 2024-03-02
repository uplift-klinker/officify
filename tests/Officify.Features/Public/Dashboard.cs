using Microsoft.Playwright.NUnit;

namespace Officify.Features.Public;

public class Dashboard : PageTest
{
    [Test]
    public async Task WhenUnauthenticatedThenNavigatesToDashboard()
    {
        await Page.GotoAsync("http://localhost:5000/");

        await Expect(Page).ToHaveURLAsync(".*dashboard");
    }
}
