using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Linq;

namespace SharedKernel.Helpers.Logging;

internal sealed class HttpRequestEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpRequestEnricher() : this(new HttpContextAccessor()) { }

    public HttpRequestEnricher(IHttpContextAccessor contextAccessor) => _contextAccessor = contextAccessor;

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        HttpContext httpContext = _contextAccessor.HttpContext;
        if (httpContext is null)
            return;

        HttpRequest request = httpContext.Request;

        logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(GetCorrelationId())));
        logEvent.AddOrUpdateProperty(new LogEventProperty("Host", new ScalarValue(request.Host)));
        logEvent.AddOrUpdateProperty(new LogEventProperty("Protocol", new ScalarValue(request.Protocol)));
        logEvent.AddOrUpdateProperty(new LogEventProperty("Scheme", new ScalarValue(request.Protocol)));

        // Only set it if available. You're not sending sensitive data in a querystring right?!
        if (request.QueryString.HasValue)
            logEvent.AddOrUpdateProperty(new LogEventProperty("QueryString", new ScalarValue(request.QueryString.Value)));

        // Set the content-type of the Response at this point
        logEvent.AddOrUpdateProperty(new LogEventProperty("ContentType", new ScalarValue(httpContext.Response.ContentType)));
    }

    private string GetCorrelationId()
    {
        StringValues correlationId = default;
        _contextAccessor.HttpContext?.Request.Headers.TryGetValue("Correlation-Id", out correlationId);
        return correlationId.FirstOrDefault() ?? Guid.NewGuid().ToString();
    }
}