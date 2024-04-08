using Officify.Infra.Host.Common;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;

namespace Officify.Infra.Host.Persistence;

public class PersistenceStack : OfficifyStackBase
{
    public const string LayerName = "persist";

    [Output("site-storage-account-name")]
    public Output<string> SiteStorageAccountName { get; set; }

    [Output("backend-storage-account-name")]
    public Output<string> BackendStorageAccountName { get; set; }

    public PersistenceStack()
        : base()
    {
        var resourceGroup = new ResourceGroup(
            "resource-group",
            new ResourceGroupArgs { ResourceGroupName = Naming.ResourceGroupName }
        );

        var siteStorageAccount = new StorageAccount(
            "site-storage",
            CreateStorageAccountArgs(resourceGroup, Naming.SiteStorageAccountName, true)
        );
        SiteStorageAccountName = siteStorageAccount.Name;

        var backendStorageAccount = new StorageAccount(
            "backend-storage",
            CreateStorageAccountArgs(resourceGroup, Naming.BackendStorageAccountName)
        );
        BackendStorageAccountName = backendStorageAccount.Name;
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
