using Officify.Infra.Host.Common;
using Officify.Infra.Host.Persistence;
using AzureStorage = Pulumi.AzureNative.Storage;

namespace Officify.Infra.Host.Web;

public class WebStack : OfficifyStackBase
{
    public const string Name = "web";

    public AzureStorage.StorageAccountStaticWebsite StaticSite { get; }

    public WebStack()
    {
        StaticSite = new AzureStorage.StorageAccountStaticWebsite("site",
            new AzureStorage.StorageAccountStaticWebsiteArgs
            {
                ResourceGroupName = Naming.ResourceGroupName(PersistenceStack.Name),
                AccountName = Naming.StorageAccountName(PersistenceStack.Name),
                Error404Document = "index.html",
                IndexDocument = "index.html",
            });
    }
}