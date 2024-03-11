using System.Runtime.InteropServices.JavaScript;

namespace Officify.Models;

public record ListResult<T>(T[] Items, int TotalCount)
{
    public static ListResult<T> EmptyListResult() => new ListResult<T>(Array.Empty<T>(), 0);
}

public record PagedListResult<T>(T[] Items, int PageSize, int NumberOfPages, int TotalCount)
    : ListResult<T>(Items, TotalCount)
{
    public static PagedListResult<T> EmptyPagedResult() => new(Array.Empty<T>(), 0, 0, 0);
}
