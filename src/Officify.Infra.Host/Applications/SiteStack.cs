using System.Text.Json;
using Officify.Infra.Host.Common;
using Officify.Infra.Host.Persistence;
using Officify.Models.Settings;
using Pulumi;
using Pulumi.AzureNative.Insights;
using Pulumi.AzureNative.OperationalInsights;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;

namespace Officify.Infra.Host.Applications;

public class SiteStack : OfficifyStackBase
{
    public const string LayerName = "site";
    private static readonly JsonSerializerOptions WebSerializerSettings =
        new(JsonSerializerDefaults.Web);

    public SiteStack()
    {
        var resourceGroup = new ResourceGroup(
            "site-resource-group",
            new ResourceGroupArgs { ResourceGroupName = Naming.ResourceGroupName }
        );

        var frontendStorageAccount = GetStorageAccount.Invoke(
            new GetStorageAccountInvokeArgs
            {
                AccountName = Naming.WebAppStorageAccountName,
                ResourceGroupName = Naming.GetResourceGroupName(PersistenceStack.LayerName),
            }
        );
        var workspace = GetWorkspace.Invoke(
            new GetWorkspaceInvokeArgs
            {
                ResourceGroupName = Naming.GetResourceGroupName(PersistenceStack.LayerName),
                WorkspaceName = Naming.LogAnalyticsWorkspaceName
            }
        );

        var insights = new Component(
            "site-insights",
            new ComponentArgs
            {
                ResourceName = Naming.ApplicationInsightsName,
                ResourceGroupName = resourceGroup.Name,
                Kind = "web",
                ApplicationType = "other",
                WorkspaceResourceId = workspace.Apply(w => w.Id)
            }
        );

        insights.ConnectionString.Apply(
            (connectionString) =>
            {
                var settings = new SettingsModel(new TelemetrySettings(connectionString));
                var settingsAsset = new StringAsset(
                    JsonSerializer.Serialize(settings, WebSerializerSettings)
                );
                return new Blob(
                    "settings-json",
                    new BlobArgs
                    {
                        AccountName = Naming.WebAppStorageAccountName,
                        ResourceGroupName = Naming.GetResourceGroupName(PersistenceStack.LayerName),
                        Type = BlobType.Block,
                        ContentType = "text/json",
                        BlobName = "settings.json",
                        ContainerName = "$web",
                        Source = settingsAsset
                    }
                );
            }
        );

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
