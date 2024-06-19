using Pulumi;
using Pulumi.AzureNative.Resources;
using Deployment = Pulumi.Deployment;

namespace Officify.Infra.Host.Common;

public abstract class OfficifyStackBase : Stack
{
    public ResourceNaming Naming { get; } = new(Deployment.Instance);

    public string Location { get; } = "centralus";
    public ResourceGroup ResourceGroup { get; }

    protected OfficifyStackBase()
    {
        ResourceGroup = new ResourceGroup("rg", new ResourceGroupArgs
        {
            ResourceGroupName = Naming.ResourceGroupName(),
            Location = Location,
        });
    }
}