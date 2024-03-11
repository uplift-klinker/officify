namespace Officify.Models;

public record PagedListResult<T>(T[] Items, int PageSize, int PageNumber, int TotalCount)
    : ListResult<T>(Items, TotalCount)
{
    public static PagedListResult<T> EmptyPagedResult() => new(Array.Empty<T>(), 0, 0, 0);

    public new PagedListResult<TModel> As<TModel>(Func<T, TModel> transform)
    {
        var items = Items.Select(transform).ToArray();
        return new PagedListResult<TModel>(items, PageSize, PageNumber, TotalCount);
    }
}
