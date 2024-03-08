using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace Officify.Features.Public;

public class Home : PageTest
{
    [Test]
    public async Task WhenUnauthenticatedThenNavigatesToDashboard()
    {
        await Page.GotoAsync("http://localhost:5001/");

        await Expect(Page).ToHaveURLAsync(".*home");
    }

    [Test]
    public async Task WhenNavigatingToPresentationThenShowsPresentations()
    {
        await Page.GotoAsync("http://localhost:5001");

        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Presentation" })
            .ClickAsync();

        await Expect(Page).ToHaveURLAsync(".*presentation");
    }
}
