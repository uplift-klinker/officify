using Pulumi;

namespace Officify.Auth.Infrastructure.Tests.Support;

public static class PulumiOutputExtensions
{
    public static Task<T> GetValueAsync<T>(this Output<T> output)
    {
        var completionSource = new TaskCompletionSource<T>();
        output.Apply(value =>
        {
            completionSource.SetResult(value);
            return value;
        });
        return completionSource.Task;
    }
}
