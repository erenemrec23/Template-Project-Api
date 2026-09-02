using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Feedbacks.DTOs; 
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{
    public interface IFeedBackRepository : IGenericRepository<Feedback>
    {

        // --- Listeler / export ---
        Task<Paginate<FeedBackListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);
        Task<Paginate<FeedBackListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);
        Task<List<FeedbackListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken cancellationToken);

        // --- Tekil (DTO) ---
        Task<FeedbackItemDto?> GetDtoByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<FeedbackItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken cancellationToken);

        // --- Değer listesiyle (IN) sorgular ---
        Task<List<Feedback>> GetByRevNumsAsync(List<long> revnums, CancellationToken cancellationToken);

    }
}
 