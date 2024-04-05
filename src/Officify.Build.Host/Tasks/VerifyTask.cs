using Cake.Frosting;
using Officify.Build.Host.Contexts;

namespace Officify.Build.Host.Tasks;

[TaskName("Verify")]
[IsDependentOn(typeof(BuildSolutionTask))]
[IsDependentOn(typeof(UnitTestsTask))]
[IsDependentOn(typeof(AcceptanceTestsTask))]
public class VerifyTask : FrostingTask<OfficifyBuildContext> { }
