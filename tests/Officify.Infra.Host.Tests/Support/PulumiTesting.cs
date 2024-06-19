using System.Collections.Immutable;
using Pulumi;
using Pulumi.Testing;

namespace Officify.Infra.Host.Tests.Support;

public static class PulumiTesting
{
    public static Task<ImmutableArray<Resource>> TestAsync<T>(string layerName, string env = "dev")
        where T : Stack, new()
    {
        return Deployment.TestAsync<T>(
            new PulumiMocks(),
            new TestOptions { StackName = $"{env}-{layerName}", IsPreview = false }
        );
    }

    public static async Task<T[]> DeployAndGetResourcesOfType<TStack, T>(
        string layerName,
        string env = "dev"
    )
        where TStack : Stack, new()
    {
        var resources = await TestAsync<TStack>(layerName, env);
        return resources.OfType<T>().ToArray();
    }
}