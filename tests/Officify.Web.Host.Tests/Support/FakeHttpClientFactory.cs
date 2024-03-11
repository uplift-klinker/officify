using System.Collections.Concurrent;
using Officify.Api.Client;

namespace Officify.Web.Host.Tests.Support;

public class FakeHttpClientFactory : IHttpClientFactory
{
    private readonly ConcurrentDictionary<string, FakeHttpMessageHandler> _handlers = new();

    public HttpClient CreateClient(string name)
    {
        return new HttpClient(GetHandler(name));
    }

    public FakeHttpMessageHandler GetHandler(string name = "")
    {
        return _handlers.GetOrAdd(name, _ => new FakeHttpMessageHandler());
    }

    public FakeHttpMessageHandler GetOfficifyApiHandler() =>
        GetHandler(OfficifyApiClient.HttpClientName);
}
