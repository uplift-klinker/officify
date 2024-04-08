using Officify.Infra.Host.Common;
using Officify.Infra.Host.Persistence;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

namespace Officify.Infra.Host.Applications;

public class ApiStack : OfficifyStackBase
{
    public const string LayerName = "api";

    public ApiStack()
        : base()
    {
        var resourceGroup = new ResourceGroup(
            "api-group",
            new ResourceGroupArgs { ResourceGroupName = Naming.ResourceGroupName, }
        );

        var storageAccount = GetStorageAccount.Invoke(
            new GetStorageAccountInvokeArgs
            {
                AccountName = Naming.BackendStorageAccountName,
                ResourceGroupName = Naming.GetResourceGroupName(PersistenceStack.LayerName)
            }
        );

        var plan = new AppServicePlan(
            "function-plan",
            new AppServicePlanArgs
            {
                Name = Naming.AppServicePlanName,
                ResourceGroupName = resourceGroup.Name,
                Sku = new SkuDescriptionArgs { Name = "Y1", Tier = "Dynamic" }
            }
        );

        var functionApp = new WebApp(
            "function-app",
            new WebAppArgs
            {
                Name = Naming.FunctionAppName,
                ResourceGroupName = resourceGroup.Name,
                ServerFarmId = plan.Id,
                Kind = "functionapp",
                HttpsOnly = true,
                SiteConfig = new SiteConfigArgs
                {
                    Http20Enabled = true,
                    HealthCheckPath = "./health",
                    AppSettings =
                    [
                        CreateAppSetting("FUNCTIONS_EXTENSION_VERSION", "~4"),
                        CreateAppSetting("FUNCTIONS_WORKER_RUNTIME", "dotnet-isolated"),
                    ]
                }
            }
        );
    }

    private NameValuePairArgs CreateAppSetting(Input<string> name, Input<string> value)
    {
        return new NameValuePairArgs { Name = name, Value = value };
    }
}
