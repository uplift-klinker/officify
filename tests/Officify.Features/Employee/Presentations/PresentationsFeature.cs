using Officify.Features.Support;

namespace Officify.Features.Employee.Presentations;

public class PresentationsFeature : AuthRequiredTest
{
    [Test]
    public async Task WhenUserCreatesPresentationThenPresentationIsAvailable()
    {
        await Login(Settings.DefaultUser.Username, Settings.DefaultUser.Password);
    }
}
