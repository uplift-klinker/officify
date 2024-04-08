using Officify.Infra.Host.Common;

var stackName = Environment.GetEnvironmentVariable("PULUMI_STACK");
var stackType = stackName.GetStackTypeFromStackName();
var runAsyncMethod = stackType.CreateDeploymentRunAsyncMethodForStack();
return await runAsyncMethod.RunAsync();
