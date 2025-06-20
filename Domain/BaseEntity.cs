using System.ComponentModel.DataAnnotations;

namespace Domain;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreatedOn { get; init; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}
