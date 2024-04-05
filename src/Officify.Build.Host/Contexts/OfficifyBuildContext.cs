using Cake.Common;
using Cake.Core;
using Cake.Frosting;

namespace Officify.Build.Host.Contexts;

public class OfficifyBuildContext(ICakeContext context) : FrostingContext(context)
{
    public string BuildConfiguration { get; } = context.Argument("configuration", "Release");
    public string RepositoryRoot { get; } =
        Path.Join(context.Environment.WorkingDirectory.FullPath, "..", "..");
    public string SolutionPath => Path.Join(RepositoryRoot, "Officify.sln");
    public string ServiceHostDirectory => Path.Join(RepositoryRoot, "src", "Officify.Service.Host");
    public string WebHostDirectory => Path.Join(RepositoryRoot, "src", "Officify.Web.Host");
    public int SignalREmulatorPort => 8888;
    public int ServiceHostPort => 5002;
    public int WebHostPort => 5001;
}
