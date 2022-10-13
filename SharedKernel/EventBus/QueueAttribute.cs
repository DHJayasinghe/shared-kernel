using System;

namespace SharedKernel.EventBus;

public sealed class QueueAttribute : Attribute
{
    public string Name { get; }
    public QueueAttribute(string name) => Name = name;
}
