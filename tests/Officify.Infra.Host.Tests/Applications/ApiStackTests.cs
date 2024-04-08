using Officify.Infra.Host.Applications;
using Pulumi.AzureNative.Web;

namespace Officify.Infra.Host.Tests.Applications;

public class ApiStackTests
{
    [Fact]
    public async Task WhenApiStackDeployedThenCreatesFunctionApp()
    {
        var webApps = await PulumiTesting.DeployAndGetResourcesOfType<ApiStack, WebApp>();
        webApps.Should().HaveCount(1);

        var functionApp = webApps[0];
        await functionApp.Name.Should().HaveValueAsync("func-dev-officify-api");
        await functionApp.HttpsOnly.Should().HaveValueAsync(true);

        var sitConfig = await functionApp.SiteConfig.GetValueAsync();
        var appSettings = sitConfig?.AppSettings.ToDictionary(d => d.Name ?? "", d => d.Value);
        appSettings.Should().Contain("FUNCTIONS_EXTENSION_VERSION", "~4");
        appSettings.Should().Contain("FUNCTIONS_WORKER_RUNTIME", "dotnet-isolated");
    }
}
