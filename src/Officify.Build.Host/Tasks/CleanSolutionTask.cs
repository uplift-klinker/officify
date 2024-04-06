using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Clean;
using Cake.Frosting;
using Officify.Build.Host.Contexts;

namespace Officify.Build.Host.Tasks;

[TaskName("CleanSolution")]
public class CleanSolutionTask : FrostingTask<OfficifyBuildContext>
{
    public override void Run(OfficifyBuildContext context)
    {
        context.DotNetClean(
            context.SolutionPath,
            new DotNetCleanSettings
            {
                Configuration = context.BuildConfiguration,
                Verbosity = DotNetVerbosity.Minimal
            }
        );
    }
}
