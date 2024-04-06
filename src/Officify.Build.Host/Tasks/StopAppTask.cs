using Cake.Frosting;
using Officify.Build.Host.Contexts;

namespace Officify.Build.Host.Tasks;

[TaskName("StopApp")]
public class StopAppTask : FrostingTask<OfficifyBuildContext>
{
    public override void Run(OfficifyBuildContext context)
    {
        ProcessKiller.KillProcessByPort(context.ServiceHostPort, context);
        ProcessKiller.KillProcessByPort(context.WebHostPort, context);
        ProcessKiller.KillProcessByPort(context.SignalREmulatorPort, context);
    }
}
