using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Officify.ServiceDefaults.Core;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddServiceDefaultsCore(
        this IHostApplicationBuilder builder,
        Action<MeterProviderBuilder>? configureMetrics = default,
        Action<TracerProviderBuilder>? configureTracing = default)
    {
        builder.ConfigureOpenTelemetry();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler();

            // Turn on service discovery by default
            http.AddServiceDiscovery();
        });

        return builder;
    }

    public static IHostApplicationBuilder ConfigureOpenTelemetry(
        this IHostApplicationBuilder builder,
        Action<MeterProviderBuilder>? configureMetrics = default,
        Action<TracerProviderBuilder>? configureTracing = default)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
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

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter) builder.Services.AddOpenTelemetry().UseOtlpExporter();

        return builder;
    }
}