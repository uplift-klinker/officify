using System.Collections.Immutable;
using Pulumi;
using Pulumi.Testing;

namespace Officify.Infra.Host.Tests.Support;

public static class PulumiTesting
{
    public static Task<ImmutableArray<Resource>> TestAsync<T>(string stackName = "dev")
        where T : Stack, new()
    {
        return Deployment.TestAsync<T>(
            new PulumiMocks(),
            new TestOptions { StackName = stackName, IsPreview = false }
        );
    }

    public static async Task<T[]> DeployAndGetResourcesOfType<TStack, T>()
        where TStack : Stack, new()
    {
        var resources = await TestAsync<TStack>();
        return resources.OfType<T>().ToArray();
    }
}
