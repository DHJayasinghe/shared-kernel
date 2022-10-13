using System;
using System.Collections.Generic;

namespace SharedKernel.Entity;

/// <summary>
/// Base types for all Entities which track state using a given Id.
/// </summary>
public abstract class BaseEntity<TId>
{
    public TId Id { get; protected set; }

    public List<BaseDomainEvent> Events = new();
    protected Action<object, string> LazyLoader { get; set; }
}
