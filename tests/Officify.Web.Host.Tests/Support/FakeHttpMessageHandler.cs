using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Json;

namespace Officify.Web.Host.Tests.Support;

public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly ConcurrentBag<ConfiguredHttpResponse> _configuredResponses = [];

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        var response = await LocateResponse(request, cancellationToken);
        return response ?? new HttpResponseMessage(HttpStatusCode.NotFound);
    }

    public void SetupResponse(ConfiguredHttpResponse response)
    {
        _configuredResponses.Add(response);
    }

    public async Task SetupGetResponse(
        string url,
        HttpResponseMessage response,
        int delayMilliseconds = 0
    )
    {
        var configuredResponse = await ConfiguredHttpResponseMessage.FromHttpResponseMessage(
            response
        );
        SetupResponse(
            new ConfiguredHttpResponse(
                HttpMethod.Get,
                new Uri(url),
                configuredResponse,
                DelayMilliseconds: delayMilliseconds
            )
        );
    }

    public async Task SetupGetJsonResponse<T>(string url, T value, int delayMilliseconds = 0)
    {
        await SetupGetResponse(
            url,
            new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(value) },
            delayMilliseconds
        );
    }

    private async Task<HttpResponseMessage?> LocateResponse(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        var content =
            request.Content == null
                ? Array.Empty<byte>()
                : await request.Content.ReadAsByteArrayAsync(cancellationToken);

        var url = request.RequestUri == null ? new UriBuilder().Uri : request.RequestUri;

        foreach (var response in _configuredResponses)
        {
            var httpResponse = await response.GetResponse(request.Method, url, content);
            if (httpResponse != null)
                return httpResponse;
        }

        return null;
    }
}
