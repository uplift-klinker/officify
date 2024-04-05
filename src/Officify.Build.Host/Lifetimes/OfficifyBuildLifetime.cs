using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;
using Officify.Build.Host.Contexts;

namespace Officify.Build.Host.Lifetimes;

public class OfficifyBuildLifetime : FrostingLifetime<OfficifyBuildContext>
{
    public override void Setup(OfficifyBuildContext context, ISetupContext info)
    {
        KillProcessByPort(context.ServiceHostPort, context);
        KillProcessByPort(context.WebHostPort, context);
        KillProcessByPort(context.SignalREmulatorPort, context);
    }

    public override void Teardown(OfficifyBuildContext context, ITeardownContext info)
    {
        KillProcessByPort(context.ServiceHostPort, context);
        KillProcessByPort(context.WebHostPort, context);
        KillProcessByPort(context.SignalREmulatorPort, context);
    }

    private static void KillProcessByPort(int port, OfficifyBuildContext context)
    {
        var settings = new ProcessSettings
        {
            RedirectStandardOutput = true,
            Arguments = new ProcessArgumentBuilder().Append($"-Fp -i:{port}")
        };
        using var process = context.StartAndReturnProcess("lsof", settings);
        process.WaitForExit();
        var output = process.GetStandardOutput().FirstOrDefault();
        if (string.IsNullOrWhiteSpace(output))
        {
            context.Information("No processes found running on port {0}", port);
            return;
        }
        var processId = output.Replace("p", "");
        context.Information("Killing process on port {0}", port);
        context.StartProcess("kill", $"-9 {processId}");
    }
}
