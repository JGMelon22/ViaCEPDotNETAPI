using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ViaCepDotNetAPI.Extensions;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection RegisterOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        string serviceName = "ViaCepDotNetApi";
        string serviceVersion = "1.0.0";
        ResourceBuilder resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: serviceVersion);

        Uri otlpEndpoint = new(configuration["OtlpExporter:Endpoint"]!);

        services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing.SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(otel =>
                {
                    otel.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    otel.Endpoint = otlpEndpoint;
                });
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddOtlpExporter(otel =>
                    {
                        otel.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                        otel.Endpoint = otlpEndpoint;
                    });
            });

        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddOpenTelemetry(options =>
            {
                options.IncludeScopes = true;
                options.SetResourceBuilder(resourceBuilder);
                // options.AddConsoleExporter();
                options.AddOtlpExporter(otel =>
                {
                    otel.Endpoint = otlpEndpoint;
                });
            });
        });

        return services;
    }
}