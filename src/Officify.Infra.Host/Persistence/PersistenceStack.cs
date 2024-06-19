using Officify.Infra.Host.Common;

using AzureStorage = Pulumi.AzureNative.Storage;
using AzureOperationalInsights = Pulumi.AzureNative.OperationalInsights;
namespace Officify.Infra.Host.Persistence;

public class PersistenceStack : OfficifyStackBase
{
    public const string Name = "persist";

    public AzureStorage.StorageAccount StorageAccount { get; }

    public AzureOperationalInsights.Workspace Workspace { get; }

    public PersistenceStack()
    {
        StorageAccount = new AzureStorage.StorageAccount("staccount", new AzureStorage.StorageAccountArgs
        {
            ResourceGroupName = ResourceGroup.Name,
            AccountName = Naming.StorageAccountName(),
            Location = ResourceGroup.Location,
            PublicNetworkAccess = AzureStorage.PublicNetworkAccess.Disabled,
            Kind = AzureStorage.Kind.StorageV2,
            Sku = new AzureStorage.Inputs.SkuArgs
            {
                Name = AzureStorage.SkuName.Standard_LRS
            },
        });

        Workspace = new AzureOperationalInsights.Workspace("workspace", new AzureOperationalInsights.WorkspaceArgs
        {
            ResourceGroupName = ResourceGroup.Name,
            Location = ResourceGroup.Location,
            WorkspaceName = Naming.LogAnalyticsWorkspaceName(),
            Sku = new AzureOperationalInsights.Inputs.WorkspaceSkuArgs
            {
                Name = AzureOperationalInsights.WorkspaceSkuNameEnum.PerGB2018
            }
        });
    }
}