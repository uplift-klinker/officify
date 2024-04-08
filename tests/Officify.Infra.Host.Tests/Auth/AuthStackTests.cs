using Officify.Infra.Host.Auth;
using Pulumi.Auth0;
using Pulumi.Random;

namespace Officify.Infra.Host.Tests.Auth;

public class AuthStackTests
{
    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesApiResourceServer()
    {
        var resourceServers = await PulumiTesting.DeployAndGetResourcesOfType<
            AuthStack,
            ResourceServer
        >(AuthStack.LayerName);
        resourceServers.Should().HaveCount(1);

        var resourceServer = resourceServers.Single();
        await resourceServer.Identifier.Should().HaveValueAsync("https://dev.officify.api.com");
        await resourceServer.AllowOfflineAccess.Should().HaveValueAsync(true);
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesWebAppClient()
    {
        var clients = await PulumiTesting.DeployAndGetResourcesOfType<AuthStack, Client>(
            AuthStack.LayerName
        );
        clients.Should().HaveCount(1);

        await clients[0].Name.Should().HaveValueAsync("Officify Dev Web");
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesWebAppClientCredentials()
    {
        var credentials = await PulumiTesting.DeployAndGetResourcesOfType<
            AuthStack,
            ClientCredentials
        >(AuthStack.LayerName);
        credentials.Should().HaveCount(1);

        await credentials[0].AuthenticationMethod.Should().HaveValueAsync("client_secret_post");
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesTestUser()
    {
        var users = await PulumiTesting.DeployAndGetResourcesOfType<AuthStack, User>(
            AuthStack.LayerName
        );
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
        var password = await PulumiTesting.DeployAndGetResourcesOfType<AuthStack, RandomPassword>(
            AuthStack.LayerName
        );
        password.Should().HaveCount(1);
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenItHasOutputsForDownstreamUsage()
    {
        var stacks = await PulumiTesting.DeployAndGetResourcesOfType<AuthStack, AuthStack>(
            AuthStack.LayerName
        );
        stacks.Should().HaveCount(1);

        var stack = stacks.Single();
        stack.TestUserEmail.Should().NotBeNull();
        stack.WebClientId.Should().NotBeNull();
        stack.WebClientSecret.Should().NotBeNull();
        stack.TestUserPassword.Should().NotBeNull();
        stack.ApiAudience.Should().NotBeNull();
    }
}
