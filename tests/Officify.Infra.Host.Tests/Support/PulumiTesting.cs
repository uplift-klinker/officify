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
}
