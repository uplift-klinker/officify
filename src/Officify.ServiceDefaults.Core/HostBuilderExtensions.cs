using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Officify.ServiceDefaults.Core;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddServiceDefaultsCore(
        this IHostBuilder builder,
        Action<MeterProviderBuilder>? configureMetrics = default,
        Action<TracerProviderBuilder>? configureTracing = default)
    {
        builder.ConfigureOpenTelemetry(configureMetrics, configureTracing);
        builder.ConfigureServices(services =>
        {
            services.AddServiceDiscovery();
        });
        builder.ConfigureServices(services =>
        {
            services.ConfigureHttpClientDefaults(http =>
            {
                // Turn on resilience by default
                http.AddStandardResilienceHandler();

                // Turn on service discovery by default
                http.AddServiceDiscovery();
            });
        });

        return builder;
    }

    public static IHostBuilder ConfigureOpenTelemetry(
        this IHostBuilder builder,
        Action<MeterProviderBuilder>? configureMetrics = default,
        Action<TracerProviderBuilder>? configureTracing = default)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.AddOpenTelemetry(l =>
            {
                l.IncludeScopes = true;
                l.IncludeFormattedMessage = true;
            });
        });

        builder.ConfigureServices(services =>
        {
            services.AddOpenTelemetry()
                .WithMetrics(metrics =>
                {
                    metrics.AddHttpClientInstrumentation()
                        .AddRuntimeInstrumentation();

                    configureMetrics?.Invoke(metrics);
                })
                .WithTracing(tracing =>
                {
                    tracing.AddHttpClientInstrumentation();
                    configureTracing?.Invoke(tracing);
                });
        });

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static IHostBuilder AddOpenTelemetryExporters(this IHostBuilder builder)
    {
        return builder.ConfigureServices((ctx, services) =>
        {
            var useOtlpExporter = !string.IsNullOrWhiteSpace(ctx.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
            if (!useOtlpExporter)
                return;
            services.AddOpenTelemetry()
                .UseOtlpExporter();
        });
    }
}