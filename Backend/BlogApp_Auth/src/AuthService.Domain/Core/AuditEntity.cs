namespace AuthService.Domain.Core;

public class AuditEntity
{
    public long? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    public long? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }

    public long? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
}