using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.QrLocations.DTOs;
using QrAssignment.Application.Features.QrLocations.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity;
using QrAssignment.Persistance.Context;
using QrAssignment.Persistance.Repositories.Base;
using System.Linq.Expressions;

internal sealed class QrLocationRepository : GenericRepository<QrLocation>, IQrLocationRepository
{
    private readonly AppDbContext _context;
    public QrLocationRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    // --- Okuma kaynakları ---
    private IQueryable<QrLocation> ActiveFeedbacks => _context.QrLocations.AsNoTracking();

    private IQueryable<QrLocation> PassivedQrLocations =>
        ActiveFeedbacks.IgnoreQueryFilters(["SoftDeleteFilter"]).Where(t => t.IsPassived);

    // --- Projeksiyonlar ---
    private static Expression<Func<QrLocation, QrLocationListItemDto>> ListProjection =>
        t => new QrLocationListItemDto
        {
            Id = t.Id,
            Name = t.Name,
            StartDate = t.StartDate,
            EndDate = t.EndDate,
            LocationName = t.LocationName,
            RevNum = t.RevNum,
            CreatedUserFullName = t.CreatedByUser != null ? t.CreatedByUser.FullName : "",
            ModifiedUserFullName = t.ModifiedByUser != null ? t.ModifiedByUser.FullName : "",
            CreatedDateTime = t.CreatedDate,
            ModifiedDateTime = t.ModifiedDate
        };

    private static Expression<Func<QrLocation, QrLocationItemDto>> ItemProjection =>
        t => new QrLocationItemDto
        {
            Id = t.Id,
            Name = t.Name,
            StartDate = t.StartDate,
            EndDate = t.EndDate,
            LocationName = t.LocationName,
            RowVersion = t.RowVersion
        };

    private static Expression<Func<QrLocation, QrLocationListItemExcelDto>> ExcelProjection =>
        t => new QrLocationListItemExcelDto
        {
            Code = t.RevNum.ToString(),
            Name = t.Name,
            StartDate = t.StartDate,
            EndDate = t.EndDate,
            LocationName = t.LocationName
        };

    // --- Liste / export ---
    public Task<Paginate<QrLocationListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken)
        => GetPaginatedListAsync(ActiveFeedbacks, request, ListProjection, cancellationToken);

    public Task<Paginate<QrLocationListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken cancellationToken)
        => GetPaginatedListAsync(PassivedQrLocations, request, ListProjection, cancellationToken);

    public Task<List<QrLocationListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken cancellationToken)
        => GetFilteredListWithoutPaginationAsync(ActiveFeedbacks, request, ExcelProjection, cancellationToken);

    // --- Tekil (DTO) ---
    public Task<QrLocationItemDto?> GetDtoByIdAsync(Guid id, CancellationToken cancellationToken)
        => ActiveFeedbacks
            .Where(t => t.Id == id)
            .Select(ItemProjection)
            .SingleOrDefaultAsync(cancellationToken);

    public Task<QrLocationItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken cancellationToken)
        => PassivedQrLocations
            .Where(t => t.Id == id)
            .Select(ItemProjection)
            .SingleOrDefaultAsync(cancellationToken);

    // --- Değer listesiyle (IN) sorgular ---
    public Task<List<QrLocation>> GetByRevNumsAsync(List<long> revnums, CancellationToken cancellationToken)
        => GetByValuesAsync(t => t.RevNum, revnums, cancellationToken: cancellationToken);

    public Task<List<QrLocation>> GetByNamesAsync(List<string> names, CancellationToken cancellationToken)
        => GetByValuesAsync(t => t.Name, names, cancellationToken: cancellationToken);
}
