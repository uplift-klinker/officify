using System.Reflection;
using Pulumi;

namespace Officify.Infra.Host.Common;

public static class TypeExtensions
{
    public static MethodInfo CreateDeploymentRunAsyncMethodForStack(this Type stackType)
    {
        var publicMethods = typeof(Deployment).GetMethods(
            BindingFlags.Static | BindingFlags.Public
        );
        var runAsyncMethod = publicMethods
            .Where(m => m.Name == "RunAsync")
            .Where(m => m.IsGenericMethod)
            .Where(m => m.ReturnType == typeof(Task<int>))
            .First(m => m.GetParameters().Length == 0);

        return runAsyncMethod.MakeGenericMethod(stackType);
    }
}
