using Officify.Models;

namespace Officify.Core;

public static class EnumerableExtensions
{
    public static Task<ListResult<T>> ToListResultAsync<T>(this IEnumerable<T> source)
    {
        var items = source.ToArray();
        return Task.FromResult(new ListResult<T>(items, items.Length));
    }

    public static Task<PagedListResult<T>> ToPagedResultAsync<T>(
        this IEnumerable<T> source,
        int pageSize,
        int pageNumber,
        int totalCount
    )
    {
        var items = source.ToArray();
        return Task.FromResult(new PagedListResult<T>(items, pageSize, pageNumber, totalCount));
    }
}
