using Officify.Infra.Host.Persistence;
using Pulumi.AzureNative.Resources;

using AzureStorage = Pulumi.AzureNative.Storage;
using AzureOperationalInsights = Pulumi.AzureNative.OperationalInsights;

namespace Officify.Infra.Host.Tests.Persistence;

public class PersistenceStackTests
{
    [Fact]
    public async Task WhenPersistenceStackIsDeployedThenCreatesResourceGroup()
    {
        var resourceGroups = await DeployAsync<ResourceGroup>();

        resourceGroups.Should().HaveCount(1);
        await resourceGroups[0].Name.Should().HaveValueAsync("rg-officify-dev-persist");
    }

    [Fact]
    public async Task WhenPersistenceStackIsDeployedThenCreatesStorageAccount()
    {
        var accounts = await DeployAsync<AzureStorage.StorageAccount>();

        accounts.Should().HaveCount(1);
        var account = accounts.First();
        await account.Name.Should().HaveValueAsync("stofficifydevpersist");

        var sku = await account.Sku.GetValueAsync();
        sku.Name.Should().Be($"{AzureStorage.SkuName.Standard_LRS}");
    }

    [Fact]
    public async Task WhenPersistenceStackIsDeployedThenCreatesLogWorkspace()
    {
        var workspaces = await DeployAsync<AzureOperationalInsights.Workspace>();

        workspaces.Should().HaveCount(1);
        var workspace = workspaces[0];
        await workspace.Name.Should().HaveValueAsync("log-officify-dev-persist");

        var sku = await workspace.Sku.GetValueAsync();
        sku?.Name.Should().Be($"{AzureOperationalInsights.WorkspaceSkuNameEnum.PerGB2018}");
    }

    private static async Task<TResource[]> DeployAsync<TResource>()
    {
        return await PulumiTesting.DeployAndGetResourcesOfType<PersistenceStack, TResource>(PersistenceStack.Name);
    }
}