using Officify.Infra.Host.Service;
using AzureResources = Pulumi.AzureNative.Resources;
using AzureWeb = Pulumi.AzureNative.Web;

namespace Officify.Infra.Host.Tests.Service;

public class ServiceStackTests() : PulumiStackTest<ServiceStack>(ServiceStack.Name)
{
    [Fact]
    public async Task WhenServiceStackIsDeployedThenCreatesResourceGroup()
    {
        var resourceGroups = await DeployAsync<AzureResources.ResourceGroup>();

        resourceGroups.Should().HaveCount(1);
        await resourceGroups[0].Name.Should().HaveValueAsync("rg-officify-dev-service");
    }

    [Fact]
    public async Task WhenServiceStackIsDeployedThenCreatesAppServicePlan()
    {
        var plans = await DeployAsync<AzureWeb.AppServicePlan>();

        plans.Should().HaveCount(1);
        await plans[0].Name.Should().HaveValueAsync("plan-officify-dev-service");
        await plans[0].Kind.Should().HaveValueAsync("Linux");
        await plans[0].Reserved.Should().HaveValueAsync(true);

        var sku = await plans[0].Sku.GetValueAsync();
        sku?.Name.Should().Be("Y1");
        sku?.Tier.Should().Be("Dynamic");
    }

    [Fact]
    public async Task WhenServiceStackIsDeployedThenCreatesFunctionApp()
    {
        var webApps = await DeployAsync<AzureWeb.WebApp>();

        webApps.Should().HaveCount(1);

        await webApps[0].Name.Should().HaveValueAsync("func-officify-dev-service");
        await webApps[0].Kind.Should().HaveValueAsync("FunctionApp");
        await webApps[0].HttpsOnly.Should().HaveValueAsync(true);

        var siteConfig = await webApps[0].SiteConfig.GetValueAsync();
        siteConfig?.Http20Enabled.Should().BeTrue();
        siteConfig?.LinuxFxVersion.Should().Be("DOTNET-ISOLATED|8.0");
    }
}