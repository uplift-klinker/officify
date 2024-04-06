using System.Diagnostics;
using Cake.Common;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;
using Path = System.IO.Path;

namespace Officify.Build.Host.Contexts;

public class OfficifyBuildContext(ICakeContext context) : FrostingContext(context)
{
    public List<Process> BackgroundProcesses { get; } = new();
    public string BuildConfiguration { get; } = context.Argument("configuration", "Release");
    public string RepositoryRoot { get; } =
        Path.Join(context.Environment.WorkingDirectory.FullPath, "..", "..");
    public string SolutionPath => Path.Join(RepositoryRoot, "Officify.sln");
    public string ServiceHostDirectory => Path.Join(RepositoryRoot, "src", "Officify.Service.Host");
    public string WebHostDirectory => Path.Join(RepositoryRoot, "src", "Officify.Web.Host");
    public int SignalREmulatorPort => 8888;
    public int ServiceHostPort => 5002;
    public int WebHostPort => 5001;

    public void StartBackgroundProcess(string fileName, Action<ProcessSettings> configureArgs)
    {
        var settings = new ProcessSettings { Arguments = new ProcessArgumentBuilder() };
        configureArgs.Invoke(settings);
        var process = new Process
        {
            EnableRaisingEvents = true,
            StartInfo = new ProcessStartInfo(fileName)
            {
                WorkingDirectory = settings.WorkingDirectory.FullPath,
                Arguments = settings.Arguments.Render()
            }
        };
        BackgroundProcesses.Add(process);
        process.Start();
    }
}
