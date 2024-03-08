using Microsoft.AspNetCore.Components.Web;

namespace Officify.Web.Host.Tests;

public class MainLayoutTests : OfficifyTestContext
{
    [Fact]
    public void WhenRenderedThenDrawerIsHidden()
    {
        var layout = RenderComponent<MainLayout>();

        layout.FindAllByClass("mud-drawer--closed").Should().HaveCount(1);
    }

    [Fact]
    public async Task WhenMenuIsToggledThenDrawerIsShown()
    {
        var layout = RenderComponent<MainLayout>();

        await layout.FindByRole("button", "menu").ClickAsync(new MouseEventArgs());

        layout.FindAllByClass("mud-drawer--open").Should().HaveCount(1);
    }
}
