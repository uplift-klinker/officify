using Cake.Common.Diagnostics;
using Cake.Frosting;
using Officify.Build.Host.Contexts;
using Polly;
using Polly.Retry;

namespace Officify.Build.Host.Tasks;

[TaskName("WaitForApp")]
public class WaitForAppTask : AsyncFrostingTask<OfficifyBuildContext>
{
    private HttpClient HttpClient { get; } = new();

    private ResiliencePipeline WaitingPipeline { get; } =
        new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions { BackoffType = DelayBackoffType.Exponential })
            .AddTimeout(TimeSpan.FromSeconds(30))
            .Build();

    public override async Task RunAsync(OfficifyBuildContext context)
    {
        await WaitForServiceToBeReady($"http://localhost:{context.WebHostPort}", context);
        await WaitForServiceToBeReady(
            $"http://localhost:{context.ServiceHostPort}/.health",
            context
        );
    }

    private async Task WaitForServiceToBeReady(string url, OfficifyBuildContext context)
    {
        context.Information("Waiting for service to be available at {0}", url);
        await WaitingPipeline.ExecuteAsync(
            async (token) =>
            {
                var response = await HttpClient.GetAsync(url, token);
                response.EnsureSuccessStatusCode();
            }
        );
        context.Information("Service at {0} is available", url);
    }
}
