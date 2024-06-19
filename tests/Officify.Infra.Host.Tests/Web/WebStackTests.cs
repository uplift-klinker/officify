using Officify.Infra.Host.Web;
using Pulumi.AzureNative.Resources;

namespace Officify.Infra.Host.Tests.Web;

public class WebStackTests() : PulumiStackTest<WebStack>(WebStack.Name)
{
    [Fact]
    public async Task WhenWebStackIsDeployedThenCreatesResourceGroup()
    {
        var resourceGroups = await DeployAsync<ResourceGroup>();

        resourceGroups.Should().HaveCount(1);
        await resourceGroups[0].Name.Should().HaveValueAsync("rg-officify-dev-web");
    }
}