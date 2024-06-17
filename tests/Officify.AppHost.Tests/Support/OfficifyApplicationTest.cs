using Projects;

namespace Officify.AppHost.Tests.Support;

public abstract class OfficifyApplicationTest : IAsyncLifetime
{
    private DistributedApplication? _app;

    protected DistributedApplication App
    {
        get
        {
            if (_app == null)
            {
                throw new InvalidOperationException("Application was never started");
            }

            return _app;
        }
    }

    public async Task InitializeAsync()
    {
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<Officify_AppHost>();
        _app = await builder.BuildAsync();
        await _app.StartAsync();
    }

    public async Task DisposeAsync()
    {
        if (_app == null)
            return;

        await _app.DisposeAsync();
    }

    protected HttpClient CreateHttpClient(string resourceName)
    {
        return App.CreateHttpClient(resourceName);
    }
}