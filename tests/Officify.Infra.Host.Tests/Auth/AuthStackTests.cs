using Officify.Infra.Host.Auth;
using Pulumi.Auth0;
using Pulumi.AzureAD;
using Pulumi.Random;
using User = Pulumi.Auth0.User;

namespace Officify.Infra.Host.Tests.Auth;

public class AuthStackTests
{
    [Fact]
    public async Task WhenAuthStackCreatedThenAddsApiApplication()
    {
        var resources = await PulumiTesting.TestAsync<AuthStack>(AuthStack.LayerName);
        var apiRegistration = await resources.SingleOfTypeAsync<ApplicationRegistration, string>(
            r => r.DisplayName,
            name => name.Contains("Api", StringComparison.OrdinalIgnoreCase)
        );

        await apiRegistration.DisplayName.Should().HaveValueAsync("Officify Dev Api");
        var identifierUri = resources.OfType<ApplicationIdentifierUri>().Single();
        await identifierUri
            .IdentifierUri.Should()
            .HaveValueAsync("https://api.dev.officify.p3-uplift.com");
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesWebAppClient()
    {
        var resources = await PulumiTesting.TestAsync<AuthStack>(AuthStack.LayerName);
        var webClient = await resources.SingleOfTypeAsync<ApplicationRegistration, string>(
            r => r.DisplayName,
            name => name.Contains("Web", StringComparison.OrdinalIgnoreCase)
        );
        var redirectUris = resources.OfType<ApplicationRedirectUris>().Single();

        await webClient.DisplayName.Should().HaveValueAsync("Officify Dev Web App");

        var callbacks = await redirectUris.RedirectUris.GetValueAsync();
        callbacks.Should().Contain("https://stdevofficifypersistwa.z14.web.core.windows.net/");
        callbacks.Should().Contain("http://localhost:5001/");
        callbacks.Should().Contain("https://localhost:5003/");
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedToDevThenAddsLocalhostCallbackUrls()
    {
        var redirectUris = await PulumiTesting.DeployAndGetResourcesOfType<
            AuthStack,
            ApplicationRedirectUris
        >(AuthStack.LayerName, "prod");
        redirectUris.Should().HaveCount(1);

        var callbacks = await redirectUris[0].RedirectUris.GetValueAsync();
        callbacks.Should().HaveCount(1);
        callbacks.Should().Contain("https://stprodofficifypersistwa.z14.web.core.windows.net/");
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenCreatesWebAppClientCredentials()
    {
        var resources = await PulumiTesting.TestAsync<AuthStack>(AuthStack.LayerName);
        var webPassword = await resources.SingleOfTypeAsync<ApplicationPassword, string>(
            r => r.DisplayName,
            name => name.Contains("Web", StringComparison.OrdinalIgnoreCase)
        );

        webPassword.Should().NotBeNull();
    }

    [Fact]
    public async Task WhenAuthStackIsDeployedThenItHasOutputsForDownstreamUsage()
    {
        var stacks = await PulumiTesting.DeployAndGetResourcesOfType<AuthStack, AuthStack>(
            AuthStack.LayerName
        );
        stacks.Should().HaveCount(1);

        var stack = stacks.Single();
        stack.WebAppClientId.Should().NotBeNull();
        stack.WebAppClientSecret.Should().NotBeNull();
        stack.ApiAudience.Should().NotBeNull();
    }
}
