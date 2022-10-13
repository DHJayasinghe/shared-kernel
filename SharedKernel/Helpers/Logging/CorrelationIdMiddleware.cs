using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace SharedKernel.Helpers.Logging;

public sealed class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext httpContext)
    {
        string correlationId = Guid.NewGuid().ToString();
        httpContext.Request.Headers.Add("Correlation-Id", correlationId);
        await _next(httpContext);
    }
}
