using Microsoft.AspNetCore.Http;
using SharedKernel.Entity;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace SharedKernel.Helpers;

public sealed class CurrentUser : ICurrentUser
{
    private readonly HttpContext _httpContext;
    public CurrentUser(IHttpContextAccessor httpContextAccessor) => _httpContext = httpContextAccessor.HttpContext;
    public CurrentUser(HttpContext httpContext) => _httpContext = httpContext;

    public bool IsAuthenticated => _httpContext.User.Identity.IsAuthenticated;

    private string GetNameIdentifier() => IsAuthenticated ? (_httpContext.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value : null;
    public Guid Id => GetNameIdentifier() != null ? Guid.Parse(GetNameIdentifier()) : Guid.Empty;

    private string GetOrganizationId() => IsAuthenticated ? (_httpContext.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "organization_id")?.Value : null;
    public Guid OrganizationId => GetOrganizationId() != null ? Guid.Parse(GetOrganizationId()) : Guid.Empty;

    private string GetEmail() => IsAuthenticated ? (_httpContext.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value : null;
    public string Email => GetEmail() ?? "anonymous";

    private string GetClientId() => IsAuthenticated ? (_httpContext.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "client_id")?.Value : null;
    public Guid ClientId => GetClientId() != null ? Guid.Parse(GetClientId()) : Guid.Empty;

    public ChannelType ChannelType => _httpContext.Request.Headers.TryGetValue("ChannelId", out var channel)
            && Enum.TryParse<ChannelType>(channel.FirstOrDefault(), out var _channelType)
        ? _channelType
        : ChannelType.Web;

    public List<string> Roles => IsAuthenticated
        ? (_httpContext.User.Identity as ClaimsIdentity).Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
        .ToList()
        : default;

    public bool IsAdmin => Roles?.Any(r => r.EndsWith("admin")) ?? false;

    public string Actor => IsAuthenticated ? (_httpContext.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "act")?.Value : null;

    public CountryClaim Country
    {

        get
        {
            var claimValue = IsAuthenticated ? (_httpContext.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "country")?.Value : null;
            return claimValue != null ? JsonSerializer.Deserialize<CountryClaim>(claimValue) : null;
        }
    }

    public Guid UserReferenceId
    {
        get
        {
            var claimValue = IsAuthenticated ? (_httpContext.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "sub_ref")?.Value : null;
            return claimValue != null ? Guid.Parse(claimValue) : default;
        }
    }

}

