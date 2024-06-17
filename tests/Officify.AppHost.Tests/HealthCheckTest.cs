using System.Net;
using Officify.AppHost.Tests.Support;

namespace Officify.AppHost.Tests;

public class HealthCheckTest : OfficifyApplicationTest
{
    [Fact]
    public async Task WhenCheckingHealthOfApiThenReturnsOk()
    {
        var httpClient = CreateHttpClient("api");
        var response = await httpClient.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task WhenGettingWebAppThenReturnsOk()
    {
        var httpClient = CreateHttpClient("web");
        var response = await httpClient.GetAsync("/");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}