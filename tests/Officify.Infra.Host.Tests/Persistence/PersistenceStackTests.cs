using Officify.Infra.Host.Persistence;
using Pulumi.AzureNative.Resources;

namespace Officify.Infra.Host.Tests.Persistence;

public class PersistenceStackTests
{
    [Fact]
    public async Task WhenPersistenceStackIsDeployedThenCreatesResourceGroup()
    {
        var resourceGroups =
            await PulumiTesting.DeployAndGetResourcesOfType<PersistenceStack, ResourceGroup>(PersistenceStack.Name);

        resourceGroups.Should().HaveCount(1);
        await resourceGroups[0].Name.Should().HaveValueAsync("rg-officify-dev-persist");
    }
}