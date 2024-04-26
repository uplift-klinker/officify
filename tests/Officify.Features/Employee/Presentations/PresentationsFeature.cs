using Officify.Features.Support;

namespace Officify.Features.Employee.Presentations;

public class PresentationsFeature : AuthRequiredTest
{
    [Test]
    [Ignore("Not ready for this need to finish auth setup")]
    public async Task WhenUserCreatesPresentationThenPresentationIsAvailable()
    {
        await Login(Settings.DefaultUser.Username, Settings.DefaultUser.Password);
    }
}
