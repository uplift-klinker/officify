using Humanizer;
using Officify.Infra.Host.Persistence;
using Pulumi;

namespace Officify.Infra.Host.Common;

public class ResourceNaming(DeploymentInstance deployment)
{
    public string StackName => deployment.StackName;
    public string EnvironmentName => StackName.Split("-")[0];
    public string LayerName => StackName.Split("-")[1];
    public string ApplicationName => "officify";

    public string ApiResourceServerName => $"{ApplicationName} {EnvironmentName} Api".Titleize();
    public string ApiAudienceIdentifier =>
        $"https://api.{EnvironmentName}.{ApplicationName}.p3-uplift.com";

    public string ApiClientSecretName => $"{ApiResourceServerName} Secret".Titleize();
    public string WebAppAuthClientName => $"{ApplicationName} {EnvironmentName} Web App".Titleize();
    public string WebAppClientSecretName => $"{WebAppAuthClientName} Secret".Titleize();
    public string AuthTenantName => $"p3-uplift-auth-{EnvironmentName}".ToLowerInvariant();
    public string TestingUserName => $"{EnvironmentName}.{ApplicationName}.test.user";
    public string TestingUserEmail => $"{TestingUserName}@noreply.com";
    public string ResourceGroupName => GetResourceGroupName(LayerName);
    public string FunctionAppName => $"func-{EnvironmentName}-{ApplicationName}-{LayerName}";

    public string AppServicePlanName => $"plan-{EnvironmentName}-{ApplicationName}-{LayerName}";
    public string WebAppStorageAccountName =>
        $"st{EnvironmentName}{ApplicationName}{PersistenceStack.LayerName}wa";
    public string BackendStorageAccountName =>
        $"st{EnvironmentName}{ApplicationName}{PersistenceStack.LayerName}be";

    public string LogAnalyticsWorkspaceName =>
        $"log-{EnvironmentName}-{ApplicationName}-{PersistenceStack.LayerName}";

    public string ApplicationInsightsName =>
        $"appi-{EnvironmentName}-{ApplicationName}-{LayerName}";

    public string SignalRServiceName => $"sigr-{EnvironmentName}-{ApplicationName}-{LayerName}";

    public static ResourceNaming FromDeployment()
    {
        return new ResourceNaming(Deployment.Instance);
    }

    public string GetResourceGroupName(string layerName)
    {
        return $"rg-{EnvironmentName}-{ApplicationName}-{layerName}";
    }
}
