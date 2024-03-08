using Bunit;
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
}
