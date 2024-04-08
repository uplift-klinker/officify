using Humanizer;
using Pulumi;

namespace Officify.Infra.Host.Common;

public class ResourceNaming(DeploymentInstance deployment)
{
    public string StackName => deployment.StackName;
    public string EnvironmentName => StackName.Split("-")[0];
    public string LayerName => StackName.Split("-")[1];
    public string ApplicationName => "officify";

    public string ApiAudienceIdentifier => $"https://{EnvironmentName}.{ApplicationName}.api.com";
    public string WebAppAuthClientName => $"{ApplicationName} {EnvironmentName} Web".Titleize();
    public string AuthTenantName => $"p3-uplift-auth-{EnvironmentName}".ToLowerInvariant();
    public string TestingUserName => $"{EnvironmentName}.{ApplicationName}.test.user";
    public string TestingUserEmail => $"{TestingUserName}@noreply.com";
    public string ResourceGroupName => GetResourceGroupName(LayerName);
    public string SiteStorageAccountName => $"st{EnvironmentName}{ApplicationName}{LayerName}site";
    public string BackendStorageAccountName => $"st{EnvironmentName}{ApplicationName}{LayerName}be";
    public string FunctionAppName => $"func-{EnvironmentName}-{ApplicationName}-{LayerName}";
    public string AppServicePlanName => $"plan-{EnvironmentName}-{ApplicationName}-{LayerName}";

    public static ResourceNaming FromDeployment()
    {
        return new ResourceNaming(Deployment.Instance);
    }

    public string GetResourceGroupName(string layerName)
    {
        return $"rg-{EnvironmentName}-{ApplicationName}-{layerName}";
    }
}
