using System.Reflection;

namespace Officify.Infra.Host.Common;

public static class MethodInfoExtensions
{
    public static async Task<int> RunAsync(this MethodInfo method)
    {
        var runResult = method.Invoke(null, null);
        if (runResult is Task<int> runTask)
        {
            return await runTask;
        }

        throw new InvalidOperationException("Failed to properly invoke RunAsync");
    }
}