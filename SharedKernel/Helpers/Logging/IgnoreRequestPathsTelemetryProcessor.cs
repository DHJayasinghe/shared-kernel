using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace SharedKernel.Helpers.Logging;

internal class IgnoreRequestPathsTelemetryProcessor : ITelemetryProcessor
{
    private readonly ITelemetryProcessor _next;

    public IgnoreRequestPathsTelemetryProcessor(ITelemetryProcessor next) => _next = next;

    public void Process(ITelemetry item)
    {
        if (item is RequestTelemetry request &&
             //(request.Url.AbsolutePath.StartsWith("/hc") ||
             request.Url.AbsolutePath.StartsWith("/swagger/"))
        {
            return;
        }

        _next.Process(item);
    }
}