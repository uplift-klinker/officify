using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Frosting;
using Officify.Build.Host.Contexts;

namespace Officify.Build.Host.Tasks;

[TaskName("BuildSolution")]
[IsDependentOn(typeof(CleanSolutionTask))]
public class BuildSolutionTask : FrostingTask<OfficifyBuildContext>
{
    public override void Run(OfficifyBuildContext context)
    {
        context.DotNetBuild(
            context.SolutionPath,
            new DotNetBuildSettings
            {
                Configuration = context.BuildConfiguration,
                Verbosity = DotNetVerbosity.Minimal
            }
        );
    }
}
