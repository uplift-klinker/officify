using Humanizer;
using Pulumi;

namespace Officify.Core.Infrastructure;

public class ResourceNaming(DeploymentInstance deployment)
{
    public string StackName => deployment.StackName;

    public string ApplicationName => "officify";

    public string ApiAudienceIdentifier => $"https://{StackName}.{ApplicationName}.api.com";
    public string WebAppAuthClientName => $"{ApplicationName} {StackName} Web".Titleize();
    public string WebAppStorageAccountName => $"st{ApplicationName}{StackName}";
    public string ApiStorageAccountName => $"st{ApplicationName}{StackName}";
    public string AuthTenantName => $"p3-uplift-auth-{StackName}".ToLowerInvariant();
    public string TestingUserName => $"{StackName}.{ApplicationName}.test.user";
    public string TestingUserEmail => $"{TestingUserName}@noreply.com";

    public static ResourceNaming FromDeployment()
    {
        return new ResourceNaming(Deployment.Instance);
    }
}
