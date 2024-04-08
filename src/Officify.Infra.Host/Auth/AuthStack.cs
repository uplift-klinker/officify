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

    [Output("web-app-client-id")]
    public Output<string> WebAppClientId { get; set; }

    [Output("web-app-client-secret")]
    public Output<string> WebAppClientSecret { get; set; }

    [Output("api-audience")]
    public Output<string> ApiAudience { get; set; }

    public AuthStack()
        : base()
    {
        var apiResourceServer = CreateBackendResourceServer();
        ApiAudience = apiResourceServer.Identifier;

        var webAppClient = CreateWebAppClient();
        var webAppClientCredentials = CreateWebAppClientCredentials(webAppClient);
        WebAppClientId = webAppClient.ClientId;
        WebAppClientSecret = Output.CreateSecret(webAppClientCredentials.ClientSecret);

        var testUserPassword = CreateRandomPassword();
        TestUserPassword = Output.CreateSecret(testUserPassword.Result);

        var testUser = CreateTestUser(testUserPassword);
        TestUserEmail = testUser.Email;
    }

    private ResourceServer CreateBackendResourceServer()
    {
        return new ResourceServer(
            "backend-resource-server",
            new ResourceServerArgs
            {
                Identifier = Naming.ApiAudienceIdentifier,
                AllowOfflineAccess = true
            }
        );
    }

    private Client CreateWebAppClient()
    {
        return new Client(
            "web-app-client",
            new ClientArgs
            {
                Name = Naming.WebAppAuthClientName,
                OidcConformant = true,
                Callbacks = GetCallbackUrls()
            }
        );
    }

    private ClientCredentials CreateWebAppClientCredentials(Client client)
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

    private string[] GetCallbackUrls()
    {
        if (!IsDevelopment)
        {
            return [$"https://{Naming.WebAppStorageAccountName}.z14.web.core.windows.net/"];
        }
        ;

        return
        [
            $"https://{Naming.WebAppStorageAccountName}.z14.web.core.windows.net/",
            "http://localhost:5001/",
            "https://localhost:5003/"
        ];
    }
}
