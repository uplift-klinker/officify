using Officify.Infra.Host.Auth;
using Pulumi.AzureNative.Resources;

using AzureAd = Pulumi.AzureAD;
namespace Officify.Infra.Host.Tests.Auth;

public class AuthStackTests() : PulumiStackTest<AuthStack>(AuthStack.Name)
{
    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesResourceGroup()
    {
        var resourceGroups = await DeployAsync<ResourceGroup>();

        resourceGroups.Should().HaveCount(1);
        await resourceGroups[0].Name.Should().HaveValueAsync("rg-officify-dev-auth");
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesWebApplication()
    {
        var applications = await DeployAsync<AzureAd.Application>();

        applications.Should().HaveCount(2);
        var displayNames = await Task.WhenAll(applications.Select(async a => await a.DisplayName.GetValueAsync()));
        displayNames.Should().Contain("Officify Dev Web");
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesApiApplication()
    {
        var applications = await DeployAsync<AzureAd.Application>();

        applications.Should().HaveCount(2);
        var displayNames = await Task.WhenAll(applications.Select(a => a.DisplayName.GetValueAsync()));
        displayNames.Should().Contain("Officify Dev Service");
    }
}