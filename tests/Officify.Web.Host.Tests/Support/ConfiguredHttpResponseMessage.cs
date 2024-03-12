using System.Net;
using System.Net.Http.Headers;

namespace Officify.Web.Host.Tests.Support;

public record ConfiguredHttpResponseMessage(
    HttpStatusCode StatusCode,
    byte[] Content,
    HttpResponseHeaders ResponseHeaders,
    HttpContentHeaders ContentHeaders
)
{
    public static async Task<ConfiguredHttpResponseMessage> FromHttpResponseMessage(
        HttpResponseMessage response
    )
    {
        var content = await response.Content.ReadAsByteArrayAsync();
        return new ConfiguredHttpResponseMessage(
            response.StatusCode,
            content,
            response.Headers,
            response.Content.Headers
        );
    }

    public HttpResponseMessage ToHttpResponseMessage()
    {
        var content = new ByteArrayContent(Content);
        foreach (var header in ContentHeaders)
            content.Headers.TryAddWithoutValidation(header.Key, header.Value);

        var response = new HttpResponseMessage(StatusCode) { Content = content };
        foreach (var header in ResponseHeaders)
            response.Headers.TryAddWithoutValidation(header.Key, header.Value);
        return response;
    }
};
