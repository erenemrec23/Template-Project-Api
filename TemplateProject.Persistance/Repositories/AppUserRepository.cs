using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Permission.Commands.Update;
using QrAssignment.Application.Features.Permission.Queries.GetByUserId;
using QrAssignment.Application.Features.Roles.Queries.LookUp.GetRoleLookUpWithPermission;
using QrAssignment.Application.Features.Users.DTOs;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Application.Features.Users.Queries.LookUp.DTOs;
using QrAssignment.Application.Features.Users.Queries.LookUp.GetPermissionLookUp;
using QrAssignment.Application.Repositories;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared.PagePermission;
using QrAssignment.Persistance.Context;
using System.Linq.Expressions; 

namespace QrAssignment.Persistance.Repositories;

internal sealed class AppUserRepository : GenericAppRepository<AppUser>, IAppUserRepository
{
    ITenantIdService _tenantIdService;
    public AppUserRepository(AppDbContext context, ITenantIdService tenantIdService) : base(context)
    {
        _tenantIdService = tenantIdService;
    }

    private static Expression<Func<AppUser, AppUserListItemDto>> ProjectionList =>
        u => new AppUserListItemDto(
            u.Id,
            u.FirstName,
            u.LastName,
            u.FullName,
            u.Email!,
            u.RevNum,
            u.ModifiedByUser != null ? u.ModifiedByUser.FullName : "",
            u.CreatedByUser != null ? u.CreatedByUser.FullName : "",
            u.ModifiedDate,
            u.CreatedDate);


    

    private static Expression<Func<AppUser, AppUserItemDto>> ProjectionItem =>
        u => new AppUserItemDto(u.Id, u.FirstName, u.LastName, u.Email!, u.RowVersion, u.TwoFactor.IsEnabled);

    private static Expression<Func<AppUser, AppUserListItemExcelDto>> ProjectionExcelItem =>
        u => new AppUserListItemExcelDto(u.FullName, u.Email!);

    // --- Ortak okuma yuzeyi ---
    public Task<Paginate<AppUserListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => PaginateAsync(ProjectionList, request, ct);

    public Task<Paginate<AppUserListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => PaginatePassivedAsync(ProjectionList, request, ct);

    public Task<List<AppUserListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => ListAsync(ProjectionExcelItem, request, ct);

    public Task<AppUserItemDto?> GetDtoByIdAsync(Guid id, CancellationToken ct = default)
        => SingleDtoByIdAsync(id, ProjectionItem, ct);

    public Task<AppUserItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken ct = default)
        => SinglePassivedDtoByIdAsync(id, ProjectionItem, ct);
    public Task<AppUser?> GetPassivedByIdAsync(Guid id, CancellationToken ct = default)
        => SinglePassivedByIdAsync(id, ct);

    public Task BulkDeleteAsync(List<Guid> ids, CancellationToken ct)
        => BulkDeleteByIdsAsync(ids, ct);


    public Task BulkSetActiveByIds(List<Guid> ids, CancellationToken ct)
        => BulkSetActiveByIdsAsync(ids, ct);


    public Task SetActiveAsync(Guid id, CancellationToken ct)
        => SetActiveByIdAsync(id, ct);
    public Task DeleteById(Guid id, CancellationToken ct)
        => DeleteByIdAsync(id, ct);

    // --- Excel Bulk Validation Helpers ---
    public async Task<List<string>> GetExistingUserNamesAsync(List<string> userNames, CancellationToken ct = default)
    {
        var users = await GetByValuesAsync(u => u.UserName!, userNames, ct);
        return users.Select(u => u.UserName!).ToList();
    }

    public async Task<List<string>> GetExistingEmailsAsync(List<string> emails, CancellationToken ct = default)
    {
        var users = await GetByValuesAsync(u => u.Email!, emails, ct);
        return users.Select(u => u.Email!).ToList();
    }

    // --- User'a ozel ---
    public Task<List<AppUserLookUpListItemDto>> GetLookUpList(CancellationToken ct)
        => _context.AppUsers
            .AsNoTracking()
            .Select(u => new AppUserLookUpListItemDto { Id = u.Id, FullName = u.FullName })
            .ToListAsync(ct);

    public Task<AppUser?> GetByIdWithRefreshTokenAsync(Guid id, CancellationToken ct = default)
        => _context.AppUsers
            .IgnoreQueryFilters(["TenantFilter"])
             .AsNoTracking()
            .Include(u => u.RefreshToken)
            .FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<AppUser?> GetByEmailWithRefreshTokenAsync(string email, CancellationToken ct = default)
        => _context.AppUsers
            .Include(u => u.RefreshToken)
            .Include(u => u.AppUserRoles)
            .ThenInclude(ur => ur.AppRole)
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<AppUser?> GetByEmailWithRefreshTokenAsync(Guid userid ,string email, CancellationToken ct = default)
        => _context.AppUsers.Where(w=> w.Id != userid)
            .Include(u => u.RefreshToken)
            .Include(u => u.AppUserRoles)
            .ThenInclude(ur => ur.AppRole)
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email == email, ct);


    public Task<AppUser?> GetByEmailForRememberPasswordAsync(string email, CancellationToken ct = default)
        => _context.AppUsers
            .AsNoTracking()
            .IgnoreQueryFilters(["TenantFilter"])
            .FirstOrDefaultAsync(u => u.Email == email, ct);

    // --- Role Sync & Permission Mappings ---
    public async Task<List<Guid>> GetAssignedRoleListDtoAsync(Guid userId, CancellationToken ct = default)
    {
        return await _context.AppUserRole
            .Where(ur => ur.AppUserId == userId && ur.AppRoleId.HasValue)
            .Select(ur => ur.AppRoleId!.Value)
            .ToListAsync(ct);
    }



    public async Task<Paginate<PermissionLookUpListItemDto>> GetUserLookUpWithPermissionAsync(
        GetUserLookUpWithPermissionQuery request, CancellationToken ct = default)
    {
        // 1. DÜZELTME: Sadece PageId değil, sayfanın bağlı olduğu Grubun (MenuGroupId) bilgisini de çekiyoruz.
        var pageInfo = await _context.Set<Page>()
            .Where(p => p.PageKey == request.PageKey)
            .Select(p => new { PageId = p.Id, MenuGroupId = p.MenuGroupId })
            .FirstOrDefaultAsync(ct);

        if (pageInfo == null) return new Paginate<PermissionLookUpListItemDto>();

        // 2. DÜZELTME: Kullanıcının SAYFADA (PageId) **VEYA** GRUPTA (MenuGroupId) yetkisi var mı diye bakıyoruz.
        var permittedIds = _context.Set<PagePermission>()
            .Where(pp => pp.UserId != null &&
                         (pp.PageId == pageInfo.PageId || (pageInfo.MenuGroupId != null && pp.MenuGroupId == pageInfo.MenuGroupId)) &&
                         pp.PermissionValue > 0)
            .Select(pp => pp.UserId!.Value)
            .Distinct(); // Bir kullanıcının hem grupta hem sayfada yetkisi varsa çift sayılmasını önler

        var query = _context.AppUsers.AsNoTracking().AsQueryable();

        // 1. İsim arama filtresi
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var term = request.Name.Trim();
            query = query.Where(u => EF.Functions.Like(u.FullName, $"%{term}%"));
        }

        // 2. Yetki durumu filtresi
        query = request.Filter switch
        {
            PermissionFilter.WithPermission => query.Where(u => permittedIds.Contains(u.Id)),
            PermissionFilter.WithoutPermission => query.Where(u => !permittedIds.Contains(u.Id)),
            _ => query
        };

        // 3. ADIM: DTO Projeksiyonu
        var projectedQuery = query.Select(u => new PermissionLookUpListItemDto
        {
            Id = u.Id,
            Name = u.FullName,
            HasPermission = permittedIds.Contains(u.Id)
        });

        // 4. ADIM: MANUEL SIRALAMA
        bool isAsc = string.Equals(request.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);

        if (string.Equals(request.SortBy, "Name", StringComparison.OrdinalIgnoreCase))
        {
            projectedQuery = isAsc
                ? projectedQuery.OrderBy(x => x.Name)
                : projectedQuery.OrderByDescending(x => x.Name);
        }
        else
        {
            projectedQuery = isAsc
                ? projectedQuery.OrderBy(x => x.HasPermission).ThenBy(x => x.Name)
                : projectedQuery.OrderByDescending(x => x.HasPermission).ThenBy(x => x.Name);
        }

        request.DynamicFilterAndSort = null;

        return await projectedQuery.ToPaginateAsync(request, x => x, ct);
    }

    public async Task<Paginate<PermissionLookUpListItemDto>> GetRoleLookUpWithPermissionAsync(
        GetRoleLookUpWithPermissionQuery request, CancellationToken ct = default)
    {
        // 1. DÜZELTME: Sayfa ve Grup ID'sini alıyoruz
        var pageInfo = await _context.Set<Page>()
            .Where(p => p.PageKey == request.PageKey)
            .Select(p => new { PageId = p.Id, MenuGroupId = p.MenuGroupId })
            .FirstOrDefaultAsync(ct);

        if (pageInfo == null) return new Paginate<PermissionLookUpListItemDto>();

        // 2. DÜZELTME: Rolün SAYFADA (PageId) **VEYA** GRUPTA (MenuGroupId) yetkisi var mı diye bakıyoruz.
        var permittedIds = _context.Set<PagePermission>()
            .Where(pp => pp.RoleId != null &&
                         (pp.PageId == pageInfo.PageId || (pageInfo.MenuGroupId != null && pp.MenuGroupId == pageInfo.MenuGroupId)) &&
                         pp.PermissionValue > 0)
            .Select(pp => pp.RoleId!.Value)
            .Distinct();

        var query = _context.AppRoles.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var term = request.Name.Trim();
            query = query.Where(r => EF.Functions.Like(r.Name, $"%{term}%"));
        }

        query = request.Filter switch
        {
            PermissionFilter.WithPermission => query.Where(r => permittedIds.Contains(r.Id)),
            PermissionFilter.WithoutPermission => query.Where(r => !permittedIds.Contains(r.Id)),
            _ => query
        };

        // 1. ADIM: DTO Projeksiyonu
        var projectedQuery = query.Select(r => new PermissionLookUpListItemDto
        {
            Id = r.Id,
            Name = r.Name,
            HasPermission = permittedIds.Contains(r.Id)
        });

        // 2. ADIM: MANUEL SIRALAMA
        bool isAsc = string.Equals(request.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);

        if (string.Equals(request.SortBy, "Name", StringComparison.OrdinalIgnoreCase))
        {
            projectedQuery = isAsc
                ? projectedQuery.OrderBy(x => x.Name)
                : projectedQuery.OrderByDescending(x => x.Name);
        }
        else
        {
            projectedQuery = isAsc
                ? projectedQuery.OrderBy(x => x.HasPermission).ThenBy(x => x.Name)
                : projectedQuery.OrderByDescending(x => x.HasPermission).ThenBy(x => x.Name);
        }

        request.DynamicFilterAndSort = null;

        return await projectedQuery.ToPaginateAsync(request, x => x, ct);
    }

    public async Task<List<Guid>> GetAssignedRoleIdsAsync(Guid userId, CancellationToken ct)
    {
        return await _context.AppUserRole
            .AsNoTracking()
            .Where(ur => ur.AppUserId == userId && ur.AppRoleId.HasValue)
            .Select(ur => ur.AppRoleId!.Value)
            .ToListAsync(ct);
    }


    public async Task<List<PermissionUserPageItemDto>> GetAssignedPermissionListDtoAsync(
    Guid userId, CancellationToken cancellationToken = default)
    {
        var roleIds = _context.AppUserRole
            .Where(ur => ur.AppUserId == userId && ur.AppRoleId != null)
            .Select(ur => ur.AppRoleId!.Value);

        var rows = await _context.Set<PagePermission>()
            .AsNoTracking()
            .Where(pp => pp.RoleId != null && roleIds.Contains(pp.RoleId.Value))
            .Select(pp => new { pp.Page.PageKey, pp.PermissionValue })
            .ToListAsync(cancellationToken);

        return rows
            .GroupBy(r => r.PageKey)
            .Select(g => new PermissionUserPageItemDto
            {
                PageName = g.Key,
                PermissionValue = g.Aggregate(0, (acc, r) => acc | (int)r.PermissionValue)
            })
            .ToList();
    }







    public async Task SyncAssignedRolesAsync(Guid userId, IEnumerable<Guid> roleIds, CancellationToken ct = default)
    {
        var target = roleIds?.ToHashSet() ?? new HashSet<Guid>();

        var current = await _context.AppUserRole
            .Where(ur => ur.AppUserId == userId)
            .ToListAsync(ct);

        var currentRoleIds = current
            .Where(ur => ur.AppRoleId.HasValue)
            .Select(ur => ur.AppRoleId!.Value)
            .ToHashSet();

        // 1) DB'de var ama formda YOK -> sil
        var toRemove = current.Where(ur => ur.AppRoleId.HasValue && !target.Contains(ur.AppRoleId.Value));
        _context.AppUserRole.RemoveRange(toRemove);

        // 2) Formda var ama DB'de YOK -> ekle
        var toAdd = target
            .Where(id => !currentRoleIds.Contains(id))
            .Select(id => new AppUserRole { AppUserId = userId, AppRoleId = id });

        await _context.AppUserRole.AddRangeAsync(toAdd, ct);
    }
    public async Task SyncUserPermissionsAsync(
   Guid userId, IEnumerable<PermissionUserUpdateDto> permissions, CancellationToken ct = default)
    {
        var incoming = (permissions ?? []).Where(p => p.PermissionValue > 0).ToList();
        var tenantId = _tenantIdService.GetTenantId();

        // Hedef tipine göre ayır — Scope alanına gerek yok, PageName/GroupKey belli ediyor
        var pageItems = incoming.Where(p => !string.IsNullOrEmpty(p.PageName)).ToList();
        var groupItems = incoming.Where(p => string.IsNullOrEmpty(p.PageName) && !string.IsNullOrEmpty(p.GroupKey)).ToList();

        await SyncPageRowsAsync(userId, pageItems, tenantId, ct);
        await SyncGroupRowsAsync(userId, groupItems, tenantId, ct);
        // SaveChanges YOK — UnitOfWorkBehavior tek transaction'da commit eder
    }

    // --- SAYFA satırları ---
    private async Task SyncPageRowsAsync(Guid userId, List<PermissionUserUpdateDto> items, Guid? tenantId, CancellationToken ct)
    {
        var keys = items.Select(p => p.PageName!).ToHashSet();
        var map = await _context.Set<Page>()
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(pg => keys.Contains(pg.PageKey))
            .Select(pg => new { pg.Id, pg.PageKey })
            .ToDictionaryAsync(x => x.PageKey, x => x.Id, ct);

        var current = await _context.Set<PagePermission>()
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(pp => pp.UserId == userId && pp.PageId != null)   // yalnızca SAYFA satırları
            .ToListAsync(ct);

        foreach (var p in items)
        {
            if (!map.TryGetValue(p.PageName!, out var pageId)) continue;
            var existing = current.FirstOrDefault(x => x.PageId == pageId);
            if (existing is null)
                _context.Set<PagePermission>().Add(
                    PagePermission.ForUser(userId, pageId, (PageAccessFlags)p.PermissionValue, tenantId));
            else
            {

                existing.PermissionValue = (PageAccessFlags)p.PermissionValue;
                existing.IsPassived = false;
            }
        }

        var ids = items.Where(p => map.ContainsKey(p.PageName!)).Select(p => map[p.PageName!]).ToHashSet();
        _context.Set<PagePermission>().RemoveRange(current.Where(x => !ids.Contains(x.PageId!.Value)));
    }

    // --- GRUP satırları ---
    private async Task SyncGroupRowsAsync(Guid userId, List<PermissionUserUpdateDto> items, Guid? tenantId, CancellationToken ct)
    {
        var keys = items.Select(p => p.GroupKey!).ToHashSet();
        var map = await _context.Set<MenuGroup>()
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(g => keys.Contains(g.Key))
            .Select(g => new { g.Id, g.Key })
            .ToDictionaryAsync(x => x.Key, x => x.Id, ct);

        var current = await _context.Set<PagePermission>()
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(pp => pp.UserId == userId && pp.MenuGroupId != null)   // yalnızca GRUP satırları
            .ToListAsync(ct);

        foreach (var p in items)
        {
            if (!map.TryGetValue(p.GroupKey!, out var groupId)) continue;
            var existing = current.FirstOrDefault(x => x.MenuGroupId == groupId);
            if (existing is null)
                _context.Set<PagePermission>().Add(
                    PagePermission.ForUserGroup(userId, groupId, (PageAccessFlags)p.PermissionValue, tenantId));
            else
            {
                existing.PermissionValue = (PageAccessFlags)p.PermissionValue;
                existing.IsPassived = false;
            }
        }

        var ids = items.Where(p => map.ContainsKey(p.GroupKey!)).Select(p => map[p.GroupKey!]).ToHashSet();
        _context.Set<PagePermission>().RemoveRange(current.Where(x => !ids.Contains(x.MenuGroupId!.Value)));
    }

}