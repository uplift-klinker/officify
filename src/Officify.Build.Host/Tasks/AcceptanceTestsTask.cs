using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using Cake.Frosting;
using Officify.Build.Host.Contexts;

namespace Officify.Build.Host.Tasks;

[TaskName("AcceptanceTests")]
[IsDependentOn(typeof(BuildSolutionTask))]
[IsDependentOn(typeof(StartAppTask))]
[IsDependentOn(typeof(WaitForAppTask))]
public class AcceptanceTestsTask : FrostingTask<OfficifyBuildContext>
{
    public override void Run(OfficifyBuildContext context)
    {
        var projectFiles = context.GetFiles($"{context.RepositoryRoot}/tests/**/*.Features.csproj");
        foreach (var projectFile in projectFiles)
        {
            context.DotNetTest(
                projectFile.FullPath,
                new DotNetTestSettings
                {
                    Configuration = context.BuildConfiguration,
                    NoBuild = true,
                    NoRestore = true
                }
            );
        }
    }
}
