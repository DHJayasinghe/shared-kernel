using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Helpers.Logging;

public static class ApplicationTelemetryExtension
{
    public static IServiceCollection AddApplicationTelemetry(this IServiceCollection services)
    {
        services.AddApplicationInsightsTelemetry();
        services.AddApplicationInsightsTelemetryProcessor<IgnoreRequestPathsTelemetryProcessor>();
        return services;
    }
}