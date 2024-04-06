using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.IO;
using Officify.Build.Host.Contexts;

namespace Officify.Build.Host;

public static class ProcessKiller
{
    public static void KillProcessByPort(int port, OfficifyBuildContext context)
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
        context.StartProcess(
            "kill",
            new ProcessSettings
            {
                Arguments = new ProcessArgumentBuilder().Append($"-9 {processId}"),
                Silent = true
            }
        );
    }
}
