using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.IO;
using Cake.Docker;
using Cake.Frosting;
using Officify.Build.Host.Contexts;

namespace Officify.Build.Host.Tasks;

[TaskName("StartApp")]
[IsDependentOn(typeof(GenerateLocalFunctionSettingsTask))]
public class StartAppTask : FrostingTask<OfficifyBuildContext>
{
    public override void Run(OfficifyBuildContext context)
    {
        StartDockerServices(context);
        StartServiceHost(context);
        StartWebHost(context);
        StartSignalREmulator(context);
    }

    private static void StartDockerServices(OfficifyBuildContext context)
    {
        context.Information("Starting docker containers");
        context.DockerComposeUp(new DockerComposeUpSettings { Build = true, Detach = true });
    }

    private static void StartServiceHost(OfficifyBuildContext context)
    {
        context.Information("Starting service host");
        context.StartAndReturnProcess(
            "func",
            new ProcessSettings
            {
                WorkingDirectory = context.ServiceHostDirectory,
                Arguments = new ProcessArgumentBuilder()
                    .Append("start")
                    .Append("--verbose")
                    .Append($"--port {context.ServiceHostPort}")
            }
        );
    }

    private static void StartWebHost(OfficifyBuildContext context)
    {
        context.Information("Starting web host");
        context.StartAndReturnProcess(
            "dotnet",
            new ProcessSettings
            {
                WorkingDirectory = context.WebHostDirectory,
                Arguments = new ProcessArgumentBuilder()
                    .Append("run")
                    .Append($"--urls http://0.0.0.0:{context.WebHostPort}")
            }
        );
    }

    private static void StartSignalREmulator(OfficifyBuildContext context)
    {
        context.Information("Starting SignalR Emulator");
        context.StartAndReturnProcess(
            "dotnet",
            new ProcessSettings
            {
                WorkingDirectory = context.RepositoryRoot,
                Arguments = new ProcessArgumentBuilder()
                    .AppendQuoted("asrs-emulator")
                    .AppendQuoted("start")
            }
        );
    }
}
