using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Officify.Web.Host.Tests.Support;

public class OfficifyTestContext : TestContext
{
    public string OfficifyApiUrl => "http://localhost:5000";
    public FakeHttpMessageHandler OfficifyApiHandler =>
        Services.GetRequiredService<FakeHttpClientFactory>().GetOfficifyApiHandler();

    public OfficifyTestContext()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddOfficifyWebHost(
            api =>
            {
                api.BaseUrl = OfficifyApiUrl;
            },
            mud =>
            {
                mud.SnackbarConfiguration.ShowTransitionDuration = 0;
                mud.SnackbarConfiguration.HideTransitionDuration = 0;
            }
        );
        Services.RemoveAll<IHttpClientFactory>();
        Services.AddSingleton<FakeHttpClientFactory>();
        Services.AddSingleton<IHttpClientFactory>(p =>
            p.GetRequiredService<FakeHttpClientFactory>()
        );
    }

    public void SetupApiGetJsonResponse<T>(string path, T value, int delayMilliseconds = 0)
    {
        OfficifyApiHandler.SetupGetJsonResponse(
            $"{OfficifyApiUrl}{path}",
            value,
            delayMilliseconds
        );
    }
}
