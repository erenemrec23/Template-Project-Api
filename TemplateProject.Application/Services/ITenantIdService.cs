namespace QrAssignment.Application.Services
{
    public interface ITenantIdService
    {
        Guid GetTenantId();
        bool TryGetTenantId(out Guid tenantId);
        void SetTenantId(Guid tenantId);
    }
}