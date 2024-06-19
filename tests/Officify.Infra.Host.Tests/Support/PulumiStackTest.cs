using Pulumi;

namespace Officify.Infra.Host.Tests.Support;

public abstract class PulumiStackTest<TStack>(string name)
    where TStack : Stack, new()
{
    protected async Task<TResource[]> DeployAsync<TResource>()
    {
        return await PulumiTesting.DeployAndGetResourcesOfType<TStack, TResource>(name);
    }
}