using System.Collections.Immutable;
using Pulumi;
using Pulumi.Testing;

namespace Officify.Infra.Host.Tests.Support;

public static class PulumiTesting
{
    public static Task<ImmutableArray<Resource>> TestAsync<T>(string stackName)
        where T : Stack, new()
    {
        return Deployment.TestAsync<T>(
            new PulumiMocks(),
            new TestOptions { StackName = stackName, IsPreview = false }
        );
    }

    public static async Task<T[]> DeployAndGetResourcesOfType<TStack, T>(
        string layerName,
        string env = "dev"
    )
        where TStack : Stack, new()
    {
        var resources = await TestAsync<TStack>($"{env}-{layerName}");
        return resources.OfType<T>().ToArray();
    }
}
