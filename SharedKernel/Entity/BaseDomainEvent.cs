using MediatR;
using System;
using System.Linq;

namespace SharedKernel.Entity;

public abstract class BaseDomainEvent : INotification
{
    public DateTimeOffset DateOccurred { get; set; } = DateTimeOffset.UtcNow;
    public Guid Id { get; set; }
    public Guid TriggeredBy { get; set; }
    public string Origin { get; set; }

    public BaseDomainEvent()
    {
        Origin = string.Join(".", GetType().AssemblyQualifiedName.Split(".").Take(3));
    }
}
