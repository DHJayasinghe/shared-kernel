using System;

namespace SharedKernel.Entity;

public abstract class AuditableEntity<TId> : BaseEntity<TId>
{
    public DateTime CreatedDateTime { get; set; }
    public DateTime LastUpdatedDateTime { get; set; }
    public string CreatedBy { get; set; }
    public string LastUpdatedBy { get; set; }
    public ChannelType Channel { get; set; }
}