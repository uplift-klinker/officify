using Microsoft.AspNetCore.Http;

namespace Officify.Service.Host;

public static class HttpRequestExtensions
{
    public static int GetIntQueryValueOrDefault(
        this HttpRequest request,
        string key,
        int defaultValue = default
    )
    {
        return request.Query.TryGetValue(key, out var values) && int.TryParse(values, out var value)
            ? value
            : defaultValue;
    }

    public static async Task<T> ReadContentAsJsonOrThrowAsync<T>(this HttpRequest request)
    {
        var body = await request.ReadFromJsonAsync<T>().ConfigureAwait(false);
        if (body == null)
            throw new BadHttpRequestException("invalid request");
        return body;
    }
}
