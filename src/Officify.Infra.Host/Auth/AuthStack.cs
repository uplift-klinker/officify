using Officify.Infra.Host.Common;
using Officify.Infra.Host.Service;
using AzureAd = Pulumi.AzureAD;

namespace Officify.Infra.Host.Auth;

public class AuthStack : OfficifyStackBase
{
    public const string Name = "auth";

    public AzureAd.Application WebApplication { get; }
    public AzureAd.Application ServiceApplication { get; }
    public AzureAd.ApplicationIdentifierUri[] ServiceIdentifierUris { get; }

    public AuthStack()
    {
        WebApplication = new AzureAd.Application("web", new AzureAd.ApplicationArgs
        {
            DisplayName = Naming.AzureAdApplication("web"),
        });

        ServiceApplication = new AzureAd.Application("service", new AzureAd.ApplicationArgs
        {
            DisplayName = Naming.AzureAdApplication("service")
        });
        ServiceIdentifierUris =
        [
            new AzureAd.ApplicationIdentifierUri("service-uri", new AzureAd.ApplicationIdentifierUriArgs
            {
                ApplicationId = ServiceApplication.ApplicationId,
                IdentifierUri = Naming.AzureAdIdentifierUri(ServiceStack.Name)
            })
        ];
    }
}