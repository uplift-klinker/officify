using Officify.Infra.Host.Common;
using Pulumi;
using Pulumi.AzureAD;
using Pulumiverse.Time;

namespace Officify.Infra.Host.Auth;

public class AuthStack : OfficifyStackBase
{
    public const string LayerName = "auth";

    [Output("web-app-client-id")]
    public Output<string> WebAppClientId { get; set; }

    [Output("web-app-client-secret")]
    public Output<string> WebAppClientSecret { get; set; }

    [Output("api-audience")]
    public Output<string> ApiAudience { get; set; }

    public AuthStack()
    {
        var secretTiming = new Rotating("rotating-secret", new RotatingArgs { RotationYears = 1 });

        var webClientApp = new ApplicationRegistration(
            "ad-web-client",
            new ApplicationRegistrationArgs { DisplayName = Naming.WebAppAuthClientName, }
        );
        var redirectUris = new ApplicationRedirectUris(
            "ad-web-client-redirects",
            new ApplicationRedirectUrisArgs
            {
                ApplicationId = webClientApp.Id,
                RedirectUris = GetCallbackUrls(),
                Type = "SPA"
            }
        );
        var webClientSecret = new ApplicationPassword(
            "ad-web-client-secret",
            new ApplicationPasswordArgs
            {
                ApplicationId = webClientApp.Id,
                DisplayName = Naming.WebAppClientSecretName,
                RotateWhenChanged = { { "rotation", secretTiming.Id } }
            }
        );
        WebAppClientId = webClientApp.ClientId;
        WebAppClientSecret = Output.CreateSecret(webClientSecret.Value);

        var apiClientApp = new ApplicationRegistration(
            "ad-api-client",
            new ApplicationRegistrationArgs { DisplayName = Naming.ApiResourceServerName }
        );
        var apiIdentifier = new ApplicationIdentifierUri(
            "ad-api-client-identifier",
            new ApplicationIdentifierUriArgs
            {
                ApplicationId = apiClientApp.Id,
                IdentifierUri = Naming.ApiAudienceIdentifier,
            }
        );
        var apiClientSecret = new ApplicationPassword(
            "ad-api-client-secret",
            new ApplicationPasswordArgs
            {
                ApplicationId = apiClientApp.Id,
                DisplayName = Naming.ApiClientSecretName,
                RotateWhenChanged = { { "rotation", secretTiming.Id } }
            }
        );
        ApiAudience = apiIdentifier.IdentifierUri;
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
