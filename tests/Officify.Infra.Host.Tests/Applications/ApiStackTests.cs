using Officify.Infra.Host.Applications;
using Pulumi.AzureNative.Insights;
using Pulumi.AzureNative.Web;

namespace Officify.Infra.Host.Tests.Applications;

public class ApiStackTests
{
    [Fact]
    public async Task WhenApiStackDeployedThenCreatesFunctionApp()
    {
        var webApps = await PulumiTesting.DeployAndGetResourcesOfType<ApiStack, WebApp>(
            ApiStack.LayerName
        );
        webApps.Should().HaveCount(1);

        var functionApp = webApps[0];
        await functionApp.Name.Should().HaveValueAsync("func-dev-officify-api");
        await functionApp.HttpsOnly.Should().HaveValueAsync(true);

        var sitConfig = await functionApp.SiteConfig.GetValueAsync();
        var appSettings = sitConfig?.AppSettings.ToDictionary(d => d.Name ?? "", d => d.Value);
        appSettings.Should().Contain("FUNCTIONS_EXTENSION_VERSION", "~4");
        appSettings.Should().Contain("FUNCTIONS_WORKER_RUNTIME", "dotnet-isolated");
        appSettings.Should().ContainKey("APPLICATIONINSIGHTS_CONNECTION_STRING");
    }

    [Fact]
    public async Task WhenApiStackDeployedThenCreatesApplicationInsights()
    {
        var insights = await PulumiTesting.DeployAndGetResourcesOfType<ApiStack, Component>(
            ApiStack.LayerName
        );

        insights.Should().HaveCount(1);
        await insights[0].Name.Should().HaveValueAsync("appi-dev-officify-api");
        await insights[0].Kind.Should().HaveValueAsync("web");
        await insights[0].ApplicationType.Should().HaveValueAsync("other");
    }
}
