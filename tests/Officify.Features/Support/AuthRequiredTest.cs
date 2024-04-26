using System.Text.RegularExpressions;

namespace Officify.Features.Support;

public class AuthRequiredTest : PageTest
{
    protected FeatureTestSettings Settings => FeatureTestSettings.Instance;

    protected async Task Login(string username, string password)
    {
        await Page.GotoAsync(Settings.LoginUrl);
        await Page.GetByLabel("username").FillAsync(username);
        await Page.GetByLabel("password").FillAsync(password);
        await Page.GetByRole(
                AriaRole.Button,
                new PageGetByRoleOptions { NameRegex = new("login", RegexOptions.IgnoreCase) }
            )
            .ClickAsync();
    }
}
