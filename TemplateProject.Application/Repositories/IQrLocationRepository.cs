using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.QrLocations.DTOs;
using QrAssignment.Application.Features.QrLocations.Queries.DTOs;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{
    public interface IQrLocationRepository : IGenericRepository<QrLocation>
    {
        // --- Listeler / export ---
        Task<Paginate<QrLocationListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);
        Task<Paginate<QrLocationListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);
        Task<List<QrLocationListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);

        // --- Tekil (DTO) ---
        Task<QrLocationItemDto?> GetDtoByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<QrLocationItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken cancellationToken);

        // --- Değer listesiyle (IN) sorgular ---
        Task<List<QrLocation>> GetByRevNumsAsync(List<long> revnums, CancellationToken cancellationToken);
        Task<List<QrLocation>> GetByNamesAsync(List<string> names, CancellationToken cancellationToken);

        // NOT: GetByIdAsync, GetPassivedByIdAsync, SetActiveByIdAsync, SetPassiveByIdAsync,
        // BulkSetActiveByIdsAsync, BulkSetPassiveByIdsAsync artık IGenericRepository'den miras geliyor.
    }
}
