using System.Text.RegularExpressions;

namespace Officify.Features.Support;

public class AuthRequiredTest : OfficifyPageTest
{
    protected async Task Login(string username, string password)
    {
        await Page.GotoAsync(BaseUrl);
        await Page.GetByRoleRegex(AriaRole.Button, "login").ClickAsync();
        await Page.WaitForURLAsync(url => url.Contains("microsoft"));

        await Page.GetByLabel("username").FillAsync(username);
        await Page.GetByLabel("password").FillAsync(password);
        await Page.GetByRoleRegex(AriaRole.Button, "login").ClickAsync();
        await Page.WaitForURLAsync(url => url.Contains(BaseUrl));
    }
}
