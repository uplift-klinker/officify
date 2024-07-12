using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Officify.ServiceDefaults.Core;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Officify.ServiceDefaults.AspNetCore;

public static class AspNetCoreHostApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddAspNetCoreServiceProviderDefaults(
        this WebApplicationBuilder builder
    )
    {
        builder.Host.AddServiceDefaultsCore(
            metrics => metrics.AddAspNetCoreInstrumentation(),
            tracing => tracing.AddAspNetCoreInstrumentation()
        );
        builder.AddDefaultHealthChecks();
        builder.AddOpenTelemetryApplicationInsights();

        return builder;
    }

    public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    public static IHostApplicationBuilder AddOpenTelemetryApplicationInsights(this IHostApplicationBuilder builder)
    {
        if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        {
            builder.Services.AddOpenTelemetry()
                .UseAzureMonitor();
        }

        return builder;
    }
}