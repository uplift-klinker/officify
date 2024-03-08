using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace Officify.Web.Host.Tests.Support;

public class OfficifyTestContext : TestContext
{
    public OfficifyTestContext()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddMudServices(opts =>
        {
            opts.SnackbarConfiguration.ShowTransitionDuration = 0;
            opts.SnackbarConfiguration.HideTransitionDuration = 0;
        });
        Services.AddOptions();
        Services.AddScoped(sp => new HttpClient());
    }
}
