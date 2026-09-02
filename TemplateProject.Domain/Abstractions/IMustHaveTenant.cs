namespace QrAssignment.Domain.Abstractions
{
    public interface IMustHaveTenant
    {
        Guid? TenantId { get; set; }
    }
}
