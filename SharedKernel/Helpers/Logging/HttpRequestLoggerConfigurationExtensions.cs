using Serilog;
using Serilog.Configuration;
using System;

namespace SharedKernel.Helpers.Logging;

public static class HttpRequestLoggerConfigurationExtensions
{
    public static LoggerConfiguration WithHttpRequest(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
        return enrichmentConfiguration.With<HttpRequestEnricher>();
    }
}
