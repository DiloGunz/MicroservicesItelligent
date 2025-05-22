using Microsoft.EntityFrameworkCore;

namespace ArticlesService.Domain.Base;

[Index(nameof(CreatedBy))]
[Index(nameof(UpdatedBy))]
[Index(nameof(CreatedAt))]
[Index(nameof(UpdatedAt))]
public abstract class AuditEntity 
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }
}