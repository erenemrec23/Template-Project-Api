using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Permission.Queries.GetByUserId;
using QrAssignment.Application.Features.Roles.DTOs;
using QrAssignment.Application.Features.Roles.Queries.GetList;
using QrAssignment.Application.Features.Users.Queries.LookUp.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;
using System.Linq.Expressions;

namespace QrAssignment.Persistance.Repositories;
internal sealed class AppRoleRepository : GenericAppRepository<AppRole>, IAppRoleRepository
{
    private readonly ITenantIdService _tenantIdService;
    public AppRoleRepository(AppDbContext context,
         ITenantIdService tenantIdService) : base(context)
    {
        _tenantIdService = tenantIdService;
    }

    private static Expression<Func<AppRole, RoleListItemDto>> ProjectionList =>
        r => new RoleListItemDto(r.Id,
            r.Name!,
            r.RevNum,
            r.ModifiedByUser != null ? r.ModifiedByUser.FullName : "",
            r.CreatedByUser != null ? r.CreatedByUser.FullName : "",
            r.ModifiedDate,
            r.CreatedDate);

    private static Expression<Func<AppRole, RoleLookUpListItemDto>> ProjectionLookUpList =>
        r => new RoleLookUpListItemDto(
            r.Id,
            r.Name!);

    private static Expression<Func<AppRole, RoleItemDto>> ProjectionItem =>
        r => new RoleItemDto(r.Id, r.Name!, r.RowVersion);
    private static Expression<Func<AppRole, RoleListItemExcelDto>> ProjectionExcelItem =>
        r => new RoleListItemExcelDto(r.Name!);



    public Task<Paginate<RoleListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => PaginateAsync(ProjectionList, request, ct);

    public Task<Paginate<RoleListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => PaginatePassivedAsync(ProjectionList, request, ct);

    public Task<List<RoleListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => ListAsync(ProjectionExcelItem, request, ct);

    public Task<RoleItemDto?> GetDtoByIdAsync(Guid id, CancellationToken ct = default)
        => SingleDtoByIdAsync(id, ProjectionItem, ct);

    public Task<RoleItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken ct = default)
        => SinglePassivedDtoByIdAsync(id, ProjectionItem, ct);
    public Task<AppRole?> GetPassivedByIdAsync(Guid id, CancellationToken ct = default)
        => SinglePassivedByIdAsync(id, ct);

    public Task BulkDelete(List<Guid> ids, CancellationToken ct)
        => BulkDeleteByIdsAsync(ids, ct);
    public Task DeleteById(Guid id, CancellationToken ct)
        => DeleteByIdAsync(id, ct);


    public Task SetPassiveById(Guid id, CancellationToken ct)
        => SetPassiveByIdAsync(id, ct);
    public Task BulkSetPassiveByIds(List<Guid> ids, CancellationToken ct)
        => BulkSetPassiveByIdsAsync(ids, ct);

    public Task Delete(Guid id, CancellationToken ct)
        => DeleteByIdAsync(id, ct);

    public Task<List<AppRole>> GetByNamesAsync(List<string> names, CancellationToken ct)
    => GetByValuesAsync(r => r.Name!, names, ct);


    public async Task<List<Guid>> GetAssignedUserListDtoAsync(Guid roleId, CancellationToken ct = default)
    {

        var result = await _context.AppUserRole
            .Where(ur => ur.AppRoleId == roleId)
            .Join(_context.AppUsers,
                  ur => ur.AppUserId,
                  u => u.Id,
                  (ur, u) => u.Id)
            .ToListAsync(ct);

        return result;

    }
    public async Task SyncAssignedUsersAsync(Guid roleId, IEnumerable<Guid> userIds, CancellationToken ct)
    {
        // Olması gereken kullanıcılar (formdan gelen)
        var target = userIds?.ToHashSet() ?? new HashSet<Guid>();

        // DB'de şu an bu role atanmış satırlar
        var current = await _context.AppUserRole
            .Where(ur => ur.AppRoleId == roleId)
            .ToListAsync(ct);

        var currentIds = current
            .Where(ur => ur.AppUserId.HasValue)
            .Select(ur => ur.AppUserId!.Value)
            .ToHashSet();

        // 1) DB'de var ama formda YOK → çıkar
        var toRemove = current.Where(ur => ur.AppUserId.HasValue
                                        && !target.Contains(ur.AppUserId.Value));
        _context.AppUserRole.RemoveRange(toRemove);

        // 2) Formda var ama DB'de YOK → ekle
        var toAdd = target
            .Where(id => !currentIds.Contains(id))
            .Select(id => new AppUserRole { AppRoleId = roleId, AppUserId = id });
        await _context.AppUserRole.AddRangeAsync(toAdd, ct);

        // 3) İkisinde de olanlara dokunulmuyor → mevcut satır ve audit alanları korunur

        // SaveChanges YOK — UnitOfWorkBehavior commit edecek
    }

    public async Task<List<PermissionUserPageItemDto>> GetAssignedPermissionListDtoAsync(
    Guid roleId, CancellationToken cancellationToken)
    {
        var rows = await _context.Set<PagePermission>()
            .AsNoTracking()
            .Where(pp => pp.RoleId == roleId)
            .Select(pp => new { pp.Page.PageKey, pp.PermissionValue,   pp.MenuGroup.Key })
            .ToListAsync(cancellationToken);

        return rows.Select(r => new PermissionUserPageItemDto
        {
            PageName = r.PageKey,
            PermissionValue = (int)r.PermissionValue,
            GroupKey = r.Key
        }).ToList();
    }


    public Task BulkSetActiveAsync(List<Guid> ids, CancellationToken ct)
        => BulkSetActiveByIdsAsync(ids, ct);


    public Task SetActiveAsync(Guid id, CancellationToken ct)
        => SetActiveByIdAsync(id, ct);
     


    public Task<Paginate<RoleLookUpListItemDto>> GetDtoLookUpListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => PaginateAsync(ProjectionLookUpList, request, ct);
}
