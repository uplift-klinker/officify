using System.Runtime.InteropServices.JavaScript;

namespace Officify.Models;

public record ListResult<T>(T[] Items, int TotalCount)
{
    public static ListResult<T> EmptyListResult() => new ListResult<T>(Array.Empty<T>(), 0);

    public ListResult<TModel> As<TModel>(Func<T, TModel> transform)
    {
        var items = Items.Select(transform).ToArray();
        return new ListResult<TModel>(items, TotalCount);
    }
}
