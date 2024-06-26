using Pulumi;

namespace Officify.Infra.Host.Tests.Support;

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

    public static async Task<TResult[]> GetValuesAsync<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, Output<TResult>> selector
    )
    {
        return await Task.WhenAll(source.Select(selector).Select(o => o.GetValueAsync()));
    }

    public static PulumiOutputAssertions<T> Should<T>(this Output<T> output)
    {
        return new PulumiOutputAssertions<T>(output);
    }
}