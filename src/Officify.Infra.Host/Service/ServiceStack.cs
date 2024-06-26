using Officify.Infra.Host.Common;

using AzureWeb = Pulumi.AzureNative.Web;

namespace Officify.Infra.Host.Service;

public class ServiceStack : OfficifyStackBase
{
    public const string Name = "service";

    public AzureWeb.AppServicePlan AppPlan { get; }
    public AzureWeb.WebApp FunctionApp { get; }

    public ServiceStack()
    {
        AppPlan = new AzureWeb.AppServicePlan("plan", new AzureWeb.AppServicePlanArgs
        {
            ResourceGroupName = ResourceGroup.Name,
            Location = ResourceGroup.Location,
            Name = Naming.AppServicePlanName(),
            Kind = "Linux",
            Reserved = true,
            Sku = new AzureWeb.Inputs.SkuDescriptionArgs
            {
                Name = "Y1",
                Tier = "Dynamic"
            }
        });

        FunctionApp = new AzureWeb.WebApp("function", new AzureWeb.WebAppArgs
        {
            ResourceGroupName = ResourceGroup.Name,
            Location = ResourceGroup.Location,
            Name = Naming.FunctionAppName(),
            Kind = "FunctionApp",
            HttpsOnly = true,
            ServerFarmId = AppPlan.Id,
            SiteConfig = new AzureWeb.Inputs.SiteConfigArgs
            {
                Http20Enabled = true,
                LinuxFxVersion = "DOTNET-ISOLATED|8.0",
            }
        });
    }
}