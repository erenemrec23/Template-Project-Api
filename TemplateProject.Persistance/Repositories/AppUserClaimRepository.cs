using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Features.Permission.Queries.GetByUserId;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;

namespace QrAssignment.Persistance.Repositories;

public sealed class AppUserClaimRepository : IAppUserClaimRepository
{
    private readonly AppDbContext _context;

    public AppUserClaimRepository(AppDbContext context)
    {
        _context = context;
    }

    // Kullanıcının SADECE kendi satırları (PagePermission.UserId)
    public async Task<List<PermissionUserPageItemDto>> GetUserWithPermissionsAsync(
        Guid? userId, CancellationToken cancellationToken = default)
    {
        var rows = await _context.Set<PagePermission>()
    .AsNoTracking()
    .IgnoreQueryFilters(["TenantFilter"])
    .Where(pp => pp.UserId == userId)
    .Select(pp => new
    {
        PageKey = pp.Page != null ? pp.Page.PageKey : null,
        GroupKey = pp.MenuGroup != null ? pp.MenuGroup.Key : null,
        pp.PermissionValue
    })
    .ToListAsync(cancellationToken);

        return rows.Select(r => new PermissionUserPageItemDto
        {
            PageName = r.PageKey,
            GroupKey = r.GroupKey,
            PermissionValue = (int)r.PermissionValue
        }).ToList();
    }

    // Kullanıcının KENDİ + ROLLERİNDEN gelen satırları, sayfa bazında bitwise OR
    public async Task<List<PermissionUserPageItemDto>> GetEffectivePagePermissionsAsync(
     Guid? userId, CancellationToken ct = default)
    {
        if (userId is null) return new();

        var roleIds = await _context.AppUserRole
            .AsNoTracking()
            .IgnoreQueryFilters(["TenantFilter"])
            .Where(ur => ur.AppUserId == userId && ur.AppRoleId != null)
            .Select(ur => ur.AppRoleId!.Value)
            .ToListAsync(ct);

        // Kullanıcı + rollerine ait TÜM satırlar (sayfa VEYA grup hedefli)
        var grants = await _context.Set<PagePermission>()
            .AsNoTracking()
            .IgnoreQueryFilters(["TenantFilter"])
            .Where(pp => pp.UserId == userId
                      || (pp.RoleId != null && roleIds.Contains(pp.RoleId.Value)))
            .Select(pp => new
            {
                PageKey = pp.Page != null ? pp.Page.PageKey : null,   // sayfa hedefli
                pp.MenuGroupId,                                        // grup hedefli
                pp.PermissionValue
            })
            .ToListAsync(ct);

        if (grants.Count == 0) return new();

        // Grup hedefli grant'lar için: o grupların sayfalarını çek (grup → pageKey listesi)
        var groupIds = grants.Where(g => g.MenuGroupId != null)
                             .Select(g => g.MenuGroupId!.Value)
                             .Distinct().ToList();

        var groupToPages = new Dictionary<short, List<string>>();
        if (groupIds.Count > 0)
        {
            var rows = await _context.Set<Page>()
                .AsNoTracking() 
                .Where(p => p.MenuGroupId != null && groupIds.Contains(p.MenuGroupId.Value))
                .Select(p => new { GroupId = p.MenuGroupId!.Value, p.PageKey })
                .ToListAsync(ct);

            groupToPages = rows.GroupBy(x => x.GroupId)
                               .ToDictionary(g => g.Key, g => g.Select(x => x.PageKey).ToList());
        }

        // Sayfa bazında OR ile birleştir
        var effective = new Dictionary<string, int>();
        void Apply(string pageKey, int value)
            => effective[pageKey] = effective.TryGetValue(pageKey, out var cur) ? cur | value : value;

        foreach (var g in grants)
        {
            var val = (int)g.PermissionValue;
            if (g.PageKey != null)                                   // tek sayfa
                Apply(g.PageKey, val);
            else if (g.MenuGroupId != null                          // grup → tüm sayfalarına yay
                  && groupToPages.TryGetValue(g.MenuGroupId.Value, out var pages))
                foreach (var pk in pages) Apply(pk, val);
        }

        return effective.Where(kv => kv.Value > 0)
                        .Select(kv => new PermissionUserPageItemDto { PageName = kv.Key, PermissionValue = kv.Value })
                        .ToList();
    }
}