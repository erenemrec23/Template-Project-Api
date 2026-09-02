using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Tenants.DTOs;
using QrAssignment.Application.Features.Tenants.Queries.DTOs;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{
    public interface ITenantRepository : IGenericRepository<Tenant>
    {
        // --- Listeler / export ---
        Task<Paginate<TenantListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);
        Task<Paginate<TenantListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);
        Task<List<TenantListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);

        // --- Tekil (DTO) ---
        Task<TenantItemDto?> GetDtoByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TenantItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken cancellationToken);


        // --- Değer listesiyle (IN) sorgular ---
        Task<List<Tenant>> GetByRevNumsAsync(List<long> revnums, CancellationToken cancellationToken);
        Task<List<Tenant>> GetByNamesAsync(List<string> names, CancellationToken cancellationToken);

        // NOT: GetByIdAsync, GetPassivedByIdAsync, SetActiveByIdAsync, SetPassiveByIdAsync,
        // BulkSetActiveByIdsAsync, BulkSetPassiveByIdsAsync artık IGenericRepository'den miras geliyor.
    }
}