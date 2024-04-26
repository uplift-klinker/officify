using Pulumi;

namespace Officify.Infra.Host.Tests.Support;

public static class EnumerableResourcesExtensions
{
    public static async Task<T> SingleOfTypeAsync<T>(
        this IEnumerable<Resource> resources,
        Func<T, Task<bool>> predicate
    )
    {
        var resourcesOfType = resources.OfType<T>().ToArray();
        var tasks = resourcesOfType.Select(async r => new
        {
            Resource = r,
            IsMatch = await predicate(r)
        });

        var results = await Task.WhenAll(tasks);
        var result = results.Single(r => r.IsMatch);
        return result.Resource;
    }

    public static async Task<T> SingleOfTypeAsync<T, TValue>(
        this IEnumerable<Resource> resources,
        Func<T, Output<TValue>> output,
        Func<TValue, bool> predicate
    )
    {
        return await resources.SingleOfTypeAsync<T>(async resource =>
        {
            var value = await output(resource).GetValueAsync();
            return predicate(value);
        });
    }
}
