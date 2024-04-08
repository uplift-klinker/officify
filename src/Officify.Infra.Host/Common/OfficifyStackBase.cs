using Pulumi;

namespace Officify.Infra.Host.Common;

public abstract class OfficifyStackBase : Stack
{
    public ResourceNaming Naming { get; } = ResourceNaming.FromDeployment();

    public bool IsDevelopment => Naming.EnvironmentName.EqualsIgnoreCase("dev");
}
