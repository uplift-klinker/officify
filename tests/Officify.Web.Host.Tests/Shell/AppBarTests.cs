using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Officify.Web.Host.Shell;

namespace Officify.Web.Host.Tests.Shell;

public class AppBarTests : OfficifyTestContext
{
    [Fact]
    public async Task WhenToggleDrawerButtonClickedThenNotifiesToToggleDrawer()
    {
        var toggleCount = 0;
        var appBar = RenderComponent<AppBar>(param =>
        {
            param.Add(a => a.OnToggleDrawer, () => toggleCount++);
        });

        await appBar.Find("button").ClickAsync(new MouseEventArgs());

        toggleCount.Should().Be(1);
    }

    [Fact]
    public async Task WhenToggleDarkModeThenNotifiesToToggleDarkMode()
    {
        var toggleCount = 0;
        var appBar = RenderComponent<AppBar>(param =>
        {
            param.Add(a => a.OnToggleDarkMode, () => toggleCount++).Add(a => a.IsDarkMode, false);
        });

        await appBar
            .Find("input[type=\"checkbox\"]")
            .ChangeAsync(new ChangeEventArgs { Value = true });

        toggleCount.Should().Be(1);
    }
}
