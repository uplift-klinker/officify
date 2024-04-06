using Microsoft.Azure.Functions.Worker.Http;
using Officify.Service.Host.Common;

namespace Officify.Service.Host;

public static class HttpRequestDataExtensions
{
    public static int GetIntQueryValueOrDefault(
        this HttpRequestData request,
        string key,
        int defaultValue = default
    )
    {
        var queryValue = request.Query.Get(key);
        return int.TryParse(queryValue, out var value) ? value : defaultValue;
    }

    public static async Task<T> ReadContentAsJsonOrThrowAsync<T>(
        this HttpRequestData request,
        CancellationToken cancellationToken = default
    )
    {
        var body = await request
            .ReadFromJsonAsync<T>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        if (body == null)
            throw new BadRequestException();
        return body;
    }
}
