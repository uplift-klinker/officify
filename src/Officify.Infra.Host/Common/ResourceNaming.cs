using Humanizer;
using Pulumi;

namespace Officify.Infra.Host.Common;

public class ResourceNaming(DeploymentInstance deployment, string layerName)
{
    public string StackName => deployment.StackName;

    public string ApplicationName => "officify";

    public string ApiAudienceIdentifier => $"https://{StackName}.{ApplicationName}.api.com";
    public string WebAppAuthClientName => $"{ApplicationName} {StackName} Web".Titleize();
    public string AuthTenantName => $"p3-uplift-auth-{StackName}".ToLowerInvariant();
    public string TestingUserName => $"{StackName}.{ApplicationName}.test.user";
    public string TestingUserEmail => $"{TestingUserName}@noreply.com";
    public string ResourceGroupName => GetResourceGroupName(layerName);
    public string SiteStorageAccountName => $"st{StackName}{ApplicationName}{layerName}site";
    public string BackendStorageAccountName => $"st{StackName}{ApplicationName}{layerName}be";
    public string FunctionAppName => $"func-{StackName}-{ApplicationName}-{layerName}";
    public string AppServicePlanName => $"plan-{StackName}-{ApplicationName}-{layerName}";

    public static ResourceNaming FromDeployment(string layer)
    {
        return new ResourceNaming(Deployment.Instance, layer);
    }

    public string GetResourceGroupName(string layer)
    {
        return $"rg-{StackName}-{ApplicationName}-{layer}";
    }
}
