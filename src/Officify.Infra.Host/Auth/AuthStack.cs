using Officify.Infra.Host.Common;
using AzureAd = Pulumi.AzureAD;

namespace Officify.Infra.Host.Auth;

public class AuthStack : OfficifyStackBase
{
    public const string Name = "auth";

    public AzureAd.Application WebApplication { get; }
    public AzureAd.Application ServiceApplication { get; }

    public AuthStack()
    {
        WebApplication = new AzureAd.Application("web", new AzureAd.ApplicationArgs
        {
            DisplayName = Naming.AzureAdApplication("web")
        });

        ServiceApplication = new AzureAd.Application("service", new AzureAd.ApplicationArgs
        {
            DisplayName = Naming.AzureAdApplication("service")
        });
    }
}