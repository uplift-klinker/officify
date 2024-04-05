using Officify.Auth.Infrastructure.Tests.Support;
using Pulumi.Auth0;
using Pulumi.Random;

namespace Officify.Auth.Infrastructure.Tests;

public class AuthStackTests
{
    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesApiResourceServer()
    {
        var resourceServers = await DeployAuthStackAndGetResourcesOfType<ResourceServer>();
        resourceServers.Should().HaveCount(1);

        var resourceServer = resourceServers.Single();
        await resourceServer.Identifier.Should().HaveValueAsync("https://dev.officify.api.com");
        await resourceServer.AllowOfflineAccess.Should().HaveValueAsync(true);
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesWebAppClient()
    {
        var clients = await DeployAuthStackAndGetResourcesOfType<Client>();
        clients.Should().HaveCount(1);

        await clients[0].Name.Should().HaveValueAsync("Officify Dev Web");
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesWebAppClientCredentials()
    {
        var credentials = await DeployAuthStackAndGetResourcesOfType<ClientCredentials>();
        credentials.Should().HaveCount(1);

        await credentials[0].AuthenticationMethod.Should().HaveValueAsync("client_secret_post");
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesTestUser()
    {
        var users = await DeployAuthStackAndGetResourcesOfType<User>();
        users.Should().HaveCount(1);

        var user = users.Single();
        await user.Name.Should().HaveValueAsync("dev.officify.test.user");
        await user.Email.Should().HaveValueAsync("dev.officify.test.user@noreply.com");
        await user.EmailVerified.Should().HaveValueAsync(true);
        await user.ConnectionName.Should().HaveValueAsync("Username-Password-Authentication");
        await user.GivenName.Should().HaveValueAsync("Test");
        await user.FamilyName.Should().HaveValueAsync("User");
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesTestUserPassword()
    {
        var password = await DeployAuthStackAndGetResourcesOfType<RandomPassword>();
        password.Should().HaveCount(1);
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenItHasOutputsForDownstreamUsage()
    {
        var stacks = await DeployAuthStackAndGetResourcesOfType<AuthStack>();
        stacks.Should().HaveCount(1);

        var stack = stacks.Single();
        stack.TestUserEmail.Should().NotBeNull();
        stack.WebClientId.Should().NotBeNull();
        stack.WebClientSecret.Should().NotBeNull();
        stack.TestUserPassword.Should().NotBeNull();
        stack.ApiAudience.Should().NotBeNull();
    }

    private static async Task<T[]> DeployAuthStackAndGetResourcesOfType<T>()
    {
        var resources = await PulumiTesting.TestAsync<AuthStack>();
        return resources.OfType<T>().ToArray();
    }
}
