using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using Cake.Frosting;
using Officify.Build.Host.Contexts;

namespace Officify.Build.Host.Tasks;

[TaskName("UnitTests")]
[IsDependentOn(typeof(BuildSolutionTask))]
public class UnitTestsTask : FrostingTask<OfficifyBuildContext>
{
    public override void Run(OfficifyBuildContext context)
    {
        var projectFiles = context.GetFiles($"{context.RepositoryRoot}/tests/**/*.Tests.csproj");
        foreach (var projectFile in projectFiles)
        {
            context.DotNetTest(
                projectFile.FullPath,
                new DotNetTestSettings
                {
                    NoRestore = true,
                    NoBuild = true,
                    Configuration = context.BuildConfiguration
                }
            );
        }
    }
}
