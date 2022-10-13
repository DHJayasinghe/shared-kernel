using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedKernel.Helpers.HealthCheck;

public static class HealthCheckExtension
{
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration, string topicName = null)
    {
        //var credentials = Guid.Parse(configuration["ManagedIdentityClientId"]) != default
        //    ? new ChainedTokenCredential(new ManagedIdentityCredential(configuration["ManagedIdentityClientId"]))
        //    : new ChainedTokenCredential(new VisualStudioCredential(), new AzureCliCredential());

        var healthCheckBuilder = services
            .AddHealthChecks();

        healthCheckBuilder
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddSqlServer(
                connectionString: configuration.GetConnectionString("SqlServer"),
                name: "sqlserver-healthcheck",
                failureStatus: HealthStatus.Unhealthy,
                tags: new string[] { "database", "connection" },
                timeout: TimeSpan.FromSeconds(10));
        //if (topicName is not null)
        //    healthCheckBuilder
        //        .AddAzureServiceBusTopic(
        //            endpoint: configuration.GetConnectionString("EventBusNamespace"),
        //            topicName: topicName,
        //            credentials,
        //            name: "servicebus-healthcheck",
        //            failureStatus: HealthStatus.Unhealthy,
        //            tags: new string[] { "servicebus", "connection" });

        return services;
    }

    public static Task WriteResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var options = new JsonWriterOptions { Indented = true };
        var healthInfo = new
        {
            status = healthReport.Status.ToString(),
            results = healthReport.Entries.ToDictionary(healthReportEntry => healthReportEntry.Key, healthReportEntry => new
            {
                status = healthReportEntry.Value.Status.ToString(),
                description = healthReportEntry.Value.Description,
                tags = healthReportEntry.Value.Tags
            })
        };
        return context.Response.WriteAsync(JsonSerializer.Serialize(healthInfo, new JsonSerializerOptions { WriteIndented = true }));
    }

    public static object GetResponse(this HealthReport healthReport)
    {
        var healthInfo = new
        {
            status = healthReport.Status.ToString(),
            results = healthReport.Entries.ToDictionary(healthReportEntry => healthReportEntry.Key, healthReportEntry => new
            {
                status = healthReportEntry.Value.Status.ToString(),
                description = healthReportEntry.Value.Description,
                tags = healthReportEntry.Value.Tags
            })
        };
        return healthInfo;
    }
}
