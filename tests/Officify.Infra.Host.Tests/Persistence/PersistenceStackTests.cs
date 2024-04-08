using Officify.Infra.Host.Persistence;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;

namespace Officify.Infra.Host.Tests.Persistence;

public class PersistenceStackTests
{
    [Fact]
    public async Task WhenPersistenceStackIsDeployedThenCreatesResourceGroup()
    {
        var resourceGroups = await PulumiTesting.DeployAndGetResourcesOfType<
            PersistenceStack,
            ResourceGroup
        >();

        resourceGroups.Should().HaveCount(1);
        await resourceGroups[0].Name.Should().HaveValueAsync("rg-dev-officify-persist");
    }

    [Fact]
    public async Task WhenPersistenceStackIsDeployedThenCreatesSiteStorageAccount()
    {
        var storageAccounts = await PulumiTesting.DeployAndGetResourcesOfType<
            PersistenceStack,
            StorageAccount
        >();

        var siteStorage = storageAccounts[0];
        await siteStorage.Name.Should().HaveValueAsync("stdevofficifypersistsite");
        await siteStorage.Kind.Should().HaveValueAsync("StorageV2");
        await siteStorage.AccessTier.Should().HaveValueAsync("Hot");
        await siteStorage.EnableHttpsTrafficOnly.Should().HaveValueAsync(true);
        await siteStorage.AllowBlobPublicAccess.Should().HaveValueAsync(true);
    }

    [Fact]
    public async Task WhenPersistenceStackIsDeployedThenCreatesBackendStorageAccount()
    {
        var storageAccounts = await PulumiTesting.DeployAndGetResourcesOfType<
            PersistenceStack,
            StorageAccount
        >();

        var backendStorage = storageAccounts[1];
        await backendStorage.Name.Should().HaveValueAsync("stdevofficifypersistbe");
        await backendStorage.Kind.Should().HaveValueAsync("StorageV2");
        await backendStorage.AccessTier.Should().HaveValueAsync("Hot");
        await backendStorage.EnableHttpsTrafficOnly.Should().HaveValueAsync(true);
        await backendStorage.AllowBlobPublicAccess.Should().HaveValueAsync(false);
    }
}