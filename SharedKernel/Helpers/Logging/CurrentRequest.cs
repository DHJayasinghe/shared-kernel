using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SharedKernel.Interfaces;
using System;
using System.Linq;

namespace SharedKernel.Helpers.Logging;

public sealed class CurrentRequest : ICurrentRequest
{
    private readonly HttpContext _httpContext;
    public CurrentRequest(IHttpContextAccessor httpContextAccessor) => _httpContext = httpContextAccessor.HttpContext;

    public Guid CorrelationId
    {
        get
        {
            if (!(_httpContext?.Request.Headers.TryGetValue("Correlation-Id", out StringValues correlationId) ?? false))
                return Guid.NewGuid();

            if (!Guid.TryParse(correlationId.FirstOrDefault(), out Guid _correlationId))
                return Guid.NewGuid();

            return _correlationId;
        }
    }
}