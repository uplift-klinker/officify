using Officify.Infra.Host.Common;
using Pulumi;
using Pulumi.Auth0;
using Pulumi.Random;

namespace Officify.Infra.Host.Auth;

public class AuthStack : OfficifyStackBase
{
    public const string LayerName = "auth";
    private const string UsernamePasswordAuthConnectionName = "Username-Password-Authentication";

    [Output("test-user-password")]
    public Output<string> TestUserPassword { get; set; }

    [Output("test-user-email")]
    public Output<string?> TestUserEmail { get; set; }

    [Output("web-client-id")]
    public Output<string> WebClientId { get; set; }

    [Output("web-client-secret")]
    public Output<string> WebClientSecret { get; set; }

    [Output("api-audience")]
    public Output<string> ApiAudience { get; set; }

    public AuthStack()
        : base(LayerName)
    {
        var apiResourceServer = CreateResourceServer();
        ApiAudience = apiResourceServer.Identifier;

        var webAppClient = CreateWebClient();
        var webAppClientCredentials = CreateWebClientCredentials(webAppClient);
        WebClientId = webAppClient.ClientId;
        WebClientSecret = Output.CreateSecret(webAppClientCredentials.ClientSecret);

        var testUserPassword = CreateRandomPassword();
        TestUserPassword = Output.CreateSecret(testUserPassword.Result);

        var testUser = CreateTestUser(testUserPassword);
        TestUserEmail = testUser.Email;
    }

    private ResourceServer CreateResourceServer()
    {
        return new ResourceServer(
            "api-resource-server",
            new ResourceServerArgs
            {
                Identifier = Naming.ApiAudienceIdentifier,
                AllowOfflineAccess = true
            }
        );
    }

    private Client CreateWebClient()
    {
        return new Client(
            "web-app-client",
            new ClientArgs { Name = Naming.WebAppAuthClientName, OidcConformant = true, }
        );
    }

    private ClientCredentials CreateWebClientCredentials(Client client)
    {
        return new ClientCredentials(
            "web-app-client-credentials",
            new ClientCredentialsArgs
            {
                ClientId = client.ClientId,
                AuthenticationMethod = "client_secret_post"
            }
        );
    }

    private static RandomPassword CreateRandomPassword()
    {
        return new RandomPassword(
            "test-user-pwd",
            new RandomPasswordArgs
            {
                MinLower = 5,
                MinNumeric = 5,
                MinSpecial = 5,
                MinUpper = 5,
                Length = 32
            }
        );
    }

    private User CreateTestUser(RandomPassword password)
    {
        return new User(
            "test-user",
            new UserArgs
            {
                Name = Naming.TestingUserName,
                Email = Naming.TestingUserEmail,
                EmailVerified = true,
                GivenName = "Test",
                FamilyName = "User",
                Password = password.Result,
                ConnectionName = UsernamePasswordAuthConnectionName,
            }
        );
    }
}
