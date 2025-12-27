namespace School.Domain.Common;

public abstract class AuditableEntity
{
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    public DateTime? ModifiedOn { get; private set; }

    protected void SetModified()
    {
        ModifiedOn = DateTime.UtcNow;
    }
}
