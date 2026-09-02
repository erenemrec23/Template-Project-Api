namespace QrAssignment.Domain.Abstractions
{
    public abstract class TenantBaseEntity : BaseEntity, IMustHaveTenant
    { 
        public Guid? TenantId { get; set; }
    }
}
