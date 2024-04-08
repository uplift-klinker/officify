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
        await resourceServer.Name.Should().HaveValueAsync("Officify Dev Api");
        await resourceServer
            .Identifier.Should()
            .HaveValueAsync("https://api.dev.officify.p3-uplift.com");
        await resourceServer.AllowOfflineAccess.Should().HaveValueAsync(true);
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesWebAppClient()
    {
        var clients = await PulumiTesting.DeployAndGetResourcesOfType<AuthStack, Client>(
            AuthStack.LayerName
        );
        clients.Should().HaveCount(1);

        var webClient = clients[0];
        await webClient.Name.Should().HaveValueAsync("Officify Dev Web App");

        var callbacks = await webClient.Callbacks.GetValueAsync();
        callbacks.Should().Contain("https://stdevofficifypersistwa.z14.web.core.windows.net/");
        callbacks.Should().Contain("http://localhost:5001/");
        callbacks.Should().Contain("https://localhost:5003/");
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedToDevThenAddsLocalhostCallbackUrls()
    {
        var clients = await PulumiTesting.DeployAndGetResourcesOfType<AuthStack, Client>(
            AuthStack.LayerName,
            "prod"
        );
        clients.Should().HaveCount(1);

        var webClient = clients[0];
        var callbacks = await webClient.Callbacks.GetValueAsync();
        callbacks.Should().HaveCount(1);
        callbacks.Should().Contain("https://stprodofficifypersistwa.z14.web.core.windows.net/");
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedToProdThenOnlyAddsSiteCallbackUrls() { }

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
        stack.WebAppClientId.Should().NotBeNull();
        stack.WebAppClientSecret.Should().NotBeNull();
        stack.TestUserPassword.Should().NotBeNull();
        stack.ApiAudience.Should().NotBeNull();
    }
}
