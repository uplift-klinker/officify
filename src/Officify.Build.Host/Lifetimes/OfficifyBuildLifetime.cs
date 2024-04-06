using Cake.Core;
using Cake.Frosting;
using Officify.Build.Host.Contexts;

namespace Officify.Build.Host.Lifetimes;

public class OfficifyBuildLifetime : FrostingLifetime<OfficifyBuildContext>
{
    public override void Setup(OfficifyBuildContext context, ISetupContext info)
    {
        ProcessKiller.KillProcessByPort(context.ServiceHostPort, context);
        ProcessKiller.KillProcessByPort(context.WebHostPort, context);
        ProcessKiller.KillProcessByPort(context.SignalREmulatorPort, context);
    }

    public override void Teardown(OfficifyBuildContext context, ITeardownContext info) { }
}
