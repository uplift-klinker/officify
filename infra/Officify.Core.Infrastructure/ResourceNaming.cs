using Pulumi;

namespace Officify.Core.Infrastructure;

public class ResourceNaming(DeploymentInstance deployment)
{
    public string StackName => deployment.StackName;

    public string ApplicationName => "officify";

    public string ApiAudienceIdentifier => $"https://{StackName}.{ApplicationName}.api.com";

    public string AuthTenantName => $"p3-uplift-auth-{StackName}".ToLowerInvariant();

    public static ResourceNaming FromDeployment()
    {
        return new ResourceNaming(Deployment.Instance);
    }
}
