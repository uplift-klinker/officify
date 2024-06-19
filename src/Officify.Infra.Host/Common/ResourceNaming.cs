using Humanizer;
using Pulumi;

namespace Officify.Infra.Host.Common;

public class ResourceNaming(DeploymentInstance deploymentInstance)
{
    private string Stack => deploymentInstance.StackName;
    public string EnvironmentName => Stack.Split("-")[0];
    public string StackName => Stack.Split("-")[1];
    public string ApplicationName => "officify";

    private string[] BaseNameParts(string? stackName = null)
    {
        return [ApplicationName, EnvironmentName, stackName ?? StackName];
    }

    public string ResourceGroupName(string? stackName = null)
    {
        return GenerateName(parts: ["rg", .. BaseNameParts(stackName)]);
    }

    public string StorageAccountName()
    {
        return GenerateName(
            separator: "",
            parts: ["st", .. BaseNameParts()]
        );
    }

    public string LogAnalyticsWorkspaceName()
    {
        return GenerateName(parts: ["log", .. BaseNameParts()]);
    }

    public string AzureAdApplication(string appName)
    {
        return GenerateName(
            separator: " ",
            parts: [ApplicationName, EnvironmentName, appName]
        ).Titleize();
    }

    private static string GenerateName(string separator = "-", params string[] parts)
    {
        return string.Join(separator, parts);
    }
}