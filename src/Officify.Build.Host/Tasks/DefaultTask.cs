using Cake.Frosting;
using Officify.Build.Host.Contexts;

namespace Officify.Build.Host.Tasks;

[TaskName("Default")]
[IsDependentOn(typeof(VerifyTask))]
public class DefaultTask : FrostingTask<OfficifyBuildContext> { }
