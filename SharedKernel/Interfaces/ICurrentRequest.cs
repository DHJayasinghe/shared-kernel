using System;

namespace SharedKernel.Interfaces
{
    /// <summary>
    /// This need be implemented using .NET Core Middleware to read "Correlation-Id" HTTP header from a incoming HTTP request
    /// </summary>
    public interface ICurrentRequest
    {
        Guid CorrelationId { get; }
    }
}
