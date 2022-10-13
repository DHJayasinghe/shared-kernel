namespace SharedKernel.Entity;

public abstract class SoftDeleteAuditableEntity<TId> : BaseEntity<TId>
{
    public bool IsDeleted { get; set; }
}
