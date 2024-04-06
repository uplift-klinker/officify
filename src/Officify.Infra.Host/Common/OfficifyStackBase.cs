using Pulumi;

namespace Officify.Infra.Host.Common;

public abstract class OfficifyStackBase : Stack
{
    public ResourceNaming Naming { get; init; } = ResourceNaming.FromDeployment();
}
