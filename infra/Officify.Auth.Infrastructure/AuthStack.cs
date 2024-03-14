using Officify.Core.Infrastructure;
using Pulumi.Auth0;

namespace Officify.Auth.Infrastructure;

public class AuthStack : OfficifyStackBase
{
    public ResourceServer ApiResourceServer { get; }

    public Client WebAppClient { get; }

    public AuthStack()
    {
        ApiResourceServer = new ResourceServer(
            "api-resource-server",
            new ResourceServerArgs { Identifier = Naming.ApiAudienceIdentifier }
        );

        WebAppClient = new Client("web-app-client", new ClientArgs { });
    }
}
