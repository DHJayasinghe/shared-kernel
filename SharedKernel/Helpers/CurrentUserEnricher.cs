using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Threading.Tasks;

namespace SharedKernel.Helpers;

/// <summary>
/// Middleware to enrich Serilog LogContext with app logged in user info. 
/// Use Enrich.FromLogContext() to your Serilog logger configuration.
/// To use this, in Startup.cs add the middleware LogCurrentUserMiddleware, 
/// and also note that the middleware should be added after UseAuthorization, 
/// in order to have context.User.Identity initialized
/// </summary>
public sealed class CurrentUserEnricher
{
    private readonly RequestDelegate next;

    public CurrentUserEnricher(RequestDelegate next) => this.next = next;

    public Task Invoke(HttpContext context)
    {
        var user = new CurrentUser(context);
        LogContext.PushProperty("CurrentUser", user, destructureObjects: true);

        return next(context);
    }
}
