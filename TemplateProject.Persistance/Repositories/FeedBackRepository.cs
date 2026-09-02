using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Feedbacks.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;
using QrAssignment.Persistance.Repositories.Base;
using System.Linq.Expressions;

internal sealed class FeedBackRepository : GenericRepository<Feedback>, IFeedBackRepository
{
    private readonly AppDbContext _context;
    public FeedBackRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    // --- Okuma kaynakları ---
    private IQueryable<Feedback> ActiveFeedbacks => _context.Feedbacks.AsNoTracking();

    private IQueryable<Feedback> PassivedFeedbacks =>
        ActiveFeedbacks.IgnoreQueryFilters(["SoftDeleteFilter"]).Where(t => t.IsPassived);

    // --- Projeksiyonlar ---
    private static Expression<Func<Feedback, FeedBackListItemDto>> ListProjection =>
        t => new FeedBackListItemDto(t.Id, t.RevNum, t.ModifiedByUser != null ? t.ModifiedByUser.FullName :"", t.CreatedByUser != null ? t.CreatedByUser.FullName :"", t.ModifiedDate, t.CreatedDate, t.Comment, t.PageUrl, (int)t.Status, t.ScreenshotPath);

    private static Expression<Func<Feedback, FeedbackItemDto>> ItemProjection =>
        t => new FeedbackItemDto(t.Id, t.RevNum, t.ModifiedByUser != null ? t.ModifiedByUser.FullName : "", t.CreatedByUser != null ? t.CreatedByUser.FullName : "", t.ModifiedDate, t.CreatedDate, t.Comment, t.PageUrl, (int)t.Status, t.RowVersion, t.ScreenshotPath);


    private static Expression<Func<Feedback, FeedbackListItemExcelDto>> ExcelProjection =>
        t => new FeedbackListItemExcelDto
        {
            Code = t.RevNum.ToString(), 
        };

    // --- Liste / export ---
    public Task<Paginate<FeedBackListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken)
        => GetPaginatedListAsync(ActiveFeedbacks, request, ListProjection, cancellationToken);

    public Task<Paginate<FeedBackListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken)
        => GetPaginatedListAsync(PassivedFeedbacks, request, ListProjection, cancellationToken);

    public Task<List<FeedbackListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken cancellationToken)
        => GetFilteredListWithoutPaginationAsync(ActiveFeedbacks, request, ExcelProjection, cancellationToken);

    // --- Tekil (DTO) ---
    public Task<FeedbackItemDto?> GetDtoByIdAsync(Guid id, CancellationToken cancellationToken)
        => ActiveFeedbacks
            .Where(t => t.Id == id)
            .Select(ItemProjection)
            .SingleOrDefaultAsync(cancellationToken);

    public Task<FeedbackItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken cancellationToken)
        => PassivedFeedbacks
            .Where(t => t.Id == id)
            .Select(ItemProjection)
            .SingleOrDefaultAsync(cancellationToken);

    // --- Değer listesiyle (IN) sorgular ---
    public Task<List<Feedback>> GetByRevNumsAsync(List<long> revnums, CancellationToken cancellationToken)
        => GetByValuesAsync(t => t.RevNum, revnums, cancellationToken: cancellationToken);
     
}
