using Pulumi;

namespace Officify.Infra.Host.Common;

public class ResourceNaming(DeploymentInstance deploymentInstance)
{
    private string Stack => deploymentInstance.StackName;
    public string EnvironmentName => Stack.Split("-")[0];
    public string StackName => Stack.Split("-")[1];
    public string ApplicationName => "officify";

    private string[] BaseNameParts()
    {
        return [ApplicationName, EnvironmentName, StackName];
    }

    public string ResourceGroupName(string? stackName = null)
    {
        return GenerateName(parts: ["rg", .. BaseNameParts()]);
    }

    private static string GenerateName(string separator = "-", params string[] parts)
    {
        return string.Join(separator, parts);
    }
}