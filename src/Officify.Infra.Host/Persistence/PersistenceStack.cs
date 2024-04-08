using Officify.Infra.Host.Common;
using Pulumi;
using Pulumi.AzureNative.OperationalInsights;
using Pulumi.AzureNative.OperationalInsights.Inputs;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using StorageAccountArgs = Pulumi.AzureNative.Storage.StorageAccountArgs;

namespace Officify.Infra.Host.Persistence;

public class PersistenceStack : OfficifyStackBase
{
    public const string LayerName = "persist";

    [Output("web-app-storage-account-name")]
    public Output<string> WebAppStorageAccountName { get; set; }

    [Output("backend-storage-account-name")]
    public Output<string> BackendStorageAccountName { get; set; }

    public PersistenceStack()
        : base()
    {
        var resourceGroup = new ResourceGroup(
            "resource-group",
            new ResourceGroupArgs { ResourceGroupName = Naming.ResourceGroupName }
        );

        var webAppStorage = new StorageAccount(
            "web-app-storage",
            CreateStorageAccountArgs(resourceGroup, Naming.WebAppStorageAccountName, true)
        );
        WebAppStorageAccountName = webAppStorage.Name;

        var backendStorageAccount = new StorageAccount(
            "backend-storage",
            CreateStorageAccountArgs(resourceGroup, Naming.BackendStorageAccountName)
        );
        BackendStorageAccountName = backendStorageAccount.Name;

        var logAnalyticsWorkspace = new Workspace(
            "log-analytics",
            new WorkspaceArgs
            {
                ResourceGroupName = resourceGroup.Name,
                WorkspaceName = Naming.LogAnalyticsWorkspaceName,
                RetentionInDays = 30,
                Sku = new WorkspaceSkuArgs { Name = WorkspaceSkuNameEnum.Free }
            }
        );
    }

    private static StorageAccountArgs CreateStorageAccountArgs(
        ResourceGroup resourceGroup,
        string accountName,
        bool publicAccess = false
    )
    {
        return new StorageAccountArgs
        {
            AccountName = accountName,
            ResourceGroupName = resourceGroup.Name,
            EnableHttpsTrafficOnly = true,
            Kind = Kind.StorageV2,
            AccessTier = AccessTier.Hot,
            AllowBlobPublicAccess = publicAccess,
            Sku = new SkuArgs { Name = SkuName.Standard_LRS }
        };
    }
}
