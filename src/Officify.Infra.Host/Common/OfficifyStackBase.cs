using Pulumi;

namespace Officify.Infra.Host.Common;

public abstract class OfficifyStackBase(string layerName) : Stack
{
    public ResourceNaming Naming { get; } = ResourceNaming.FromDeployment(layerName);
}
