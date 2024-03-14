using Pulumi;

namespace Officify.Core.Infrastructure;

public abstract class OfficifyStackBase : Stack
{
    public ResourceNaming Naming { get; init; } = ResourceNaming.FromDeployment();
}
