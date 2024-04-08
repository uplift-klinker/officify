using Officify.Infra.Host.Applications;
using Pulumi.AzureNative.Storage;

namespace Officify.Infra.Host.Tests.Applications;

public class SiteStackTests
{
    [Fact]
    public async Task WhenSiteStackDeployedThenCreatesStaticSiteForSiteStorageAccount()
    {
        var sites = await PulumiTesting.DeployAndGetResourcesOfType<
            SiteStack,
            StorageAccountStaticWebsite
        >();

        sites.Should().HaveCount(1);

        await sites[0].IndexDocument.Should().HaveValueAsync("index.html");
        await sites[0].Error404Document.Should().HaveValueAsync("index.html");
    }
}
