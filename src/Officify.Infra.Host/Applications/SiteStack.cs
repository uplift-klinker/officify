using Officify.Infra.Host.Common;
using Officify.Infra.Host.Persistence;
using Pulumi.AzureNative.Storage;

namespace Officify.Infra.Host.Applications;

public class SiteStack : OfficifyStackBase
{
    public const string LayerName = "site";

    public SiteStack()
        : base()
    {
        var staticSite = new StorageAccountStaticWebsite(
            "static-site",
            new StorageAccountStaticWebsiteArgs
            {
                AccountName = Naming.WebAppStorageAccountName,
                ResourceGroupName = Naming.GetResourceGroupName(PersistenceStack.LayerName),
                IndexDocument = "index.html",
                Error404Document = "index.html",
            }
        );
    }
}
