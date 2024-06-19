using Officify.Infra.Host.Auth;
using Pulumi.AzureNative.Resources;

namespace Officify.Infra.Host.Tests.Auth;

public class AuthStackTests
{
    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesResourceGroup()
    {
        var resourceGroups =
            await PulumiTesting.DeployAndGetResourcesOfType<AuthStack, ResourceGroup>(AuthStack.Name);

        resourceGroups.Should().HaveCount(1);
        await resourceGroups[0].Name.Should().HaveValueAsync("rg-officify-dev-auth");
    }
}