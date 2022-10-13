using SharedKernel.Interfaces;
using System;

namespace SharedKernel.Helpers.Logging;

public sealed class CurrentRequestForWorker : ICurrentRequest
{
    public Guid CorrelationId => Guid.NewGuid();
}