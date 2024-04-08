using Officify.Infra.Host.Applications;
using Pulumi.AzureNative.Insights;
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
        >(SiteStack.LayerName);

        sites.Should().HaveCount(1);

        await sites[0].IndexDocument.Should().HaveValueAsync("index.html");
        await sites[0].Error404Document.Should().HaveValueAsync("index.html");
    }

    [Fact]
    public async Task WhenSiteStackDeployedThenCreatesSettingsJsonBlob()
    {
        var blobs = await PulumiTesting.DeployAndGetResourcesOfType<SiteStack, Blob>(
            SiteStack.LayerName
        );

        blobs.Should().HaveCountGreaterThan(0);

        var blobsWithNames = await Task.WhenAll(
            blobs.Select(async b => new { Name = await b.Name.GetValueAsync(), Blob = b })
        );
        var settingsBlob = blobsWithNames.Single(b => b.Name == "settings.json");
        await settingsBlob.Blob.Type.Should().HaveValueAsync(BlobType.Block);
        await settingsBlob.Blob.ContentType.Should().HaveValueAsync("text/json");
    }

    [Fact]
    public async Task WhenSiteStackDeployedThenCreatesApplicationInsights()
    {
        var insights = await PulumiTesting.DeployAndGetResourcesOfType<SiteStack, Component>(
            SiteStack.LayerName
        );

        insights.Should().HaveCount(1);

        await insights[0].Name.Should().HaveValueAsync("appi-dev-officify-site");
        await insights[0].ApplicationType.Should().HaveValueAsync("other");
        await insights[0].Kind.Should().HaveValueAsync("web");
    }
}
