using Bunit;
using Officify.Web.Host.Shell;

namespace Officify.Web.Host.Tests.Shell;

public class AppDrawerTests : OfficifyTestContext
{
    [Fact]
    public void WhenClosedThenDrawerIsHidden()
    {
        var drawer = RenderComponent<AppDrawer>(p => p.Add(d => d.IsOpen, false));

        drawer.FindAllByClass("mud-drawer--closed").Should().HaveCount(1);
        drawer.FindAllByClass("mud-drawer--open").Should().BeEmpty();
    }

    [Fact]
    public void WhenDrawerOpenedThenDrawerIsShown()
    {
        var drawer = RenderComponent<AppDrawer>(p => p.Add(d => d.IsOpen, true));

        drawer.FindAllByClass("mud-drawer--open").Should().HaveCount(1);
        drawer.FindAllByClass("mud-drawer--closed").Should().BeEmpty();
    }
}
