namespace QrAssignment.Domain.Entity.Audit;

public class SystemAuditLog
{
    public Guid Id { get; set; } = Guid.CreateVersion7(); 
    public Guid? TenantId { get; set; }
    public string TableName { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string? PrimaryKey { get; set; }
    public string? OldValues { get; set; } 
    public string? NewValues { get; set; } 

    public string ColumnValues { get; set; }
    public string? UserId { get; set; }
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
}