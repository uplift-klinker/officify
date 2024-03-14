using Officify.Auth.Infrastructure.Tests.Support;
using Pulumi.Auth0;

namespace Officify.Auth.Infrastructure.Tests;

public class AuthStackTests
{
    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesApiResourceServer()
    {
        var resources = await PulumiTesting.TestAsync<AuthStack>();

        var resourceServers = resources.OfType<ResourceServer>().ToArray();
        resourceServers.Should().HaveCount(1);
        var identifier = await resourceServers[0].Identifier.GetValueAsync();
        identifier.Should().Be($"https://dev.officify.api.com");
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesWebAppClient()
    {
        var resources = await PulumiTesting.TestAsync<AuthStack>();

        var clients = resources.OfType<Client>().ToArray();
        clients.Should().HaveCount(1);
    }
}
