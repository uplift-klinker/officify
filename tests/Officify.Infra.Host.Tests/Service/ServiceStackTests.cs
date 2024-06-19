using Officify.Infra.Host.Service;
using Pulumi.AzureNative.Resources;

namespace Officify.Infra.Host.Tests.Service;

public class ServiceStackTests
{
    [Fact]
    public async Task WhenServiceStackIsDeployedThenCreatesResourceGroup()
    {
        var resourceGroups =
            await PulumiTesting.DeployAndGetResourcesOfType<ServiceStack, ResourceGroup>(ServiceStack.Name);

        resourceGroups.Should().HaveCount(1);
        await resourceGroups[0].Name.Should().HaveValueAsync("rg-officify-dev-service");
    }
}