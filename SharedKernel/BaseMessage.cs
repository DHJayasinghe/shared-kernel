using MediatR;
using System;

namespace SharedKernel;

public class BaseMessage : INotification
{
    public DateTimeOffset DateTriggered { get; set; } = DateTimeOffset.UtcNow;
    public Guid Id { get; set; }
    public Guid TriggeredBy { get; set; }
}