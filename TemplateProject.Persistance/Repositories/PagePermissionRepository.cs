using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;
using QrAssignment.Persistance.Repositories.Base;

internal sealed class PagePermissionRepository : GenericRepository<PagePermission>, IPagePermissionRepository
{

    private readonly AppDbContext _context;
    public PagePermissionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<List<PagePermission>> GetPagePermissionList(int pageId, CancellationToken ct = default)
    => _context.Set<PagePermission>().Where(p => p.PageId == pageId).ToListAsync(ct);


    public Task<Dictionary<string, int>> GetPageIdsByKeysAsync(IEnumerable<string> pageKeys, CancellationToken ct = default)
    {
        var keys = pageKeys.ToHashSet();
        return  _context.Set<Page>()
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(pg => keys.Contains(pg.PageKey))
            .Select(pg => new { pg.Id, pg.PageKey })
            .ToDictionaryAsync(x => x.PageKey, x => x.Id, ct);
    }

    public Task<Dictionary<string, short>> GetMenuGroupIdsByKeysAsync(IEnumerable<string> groupKeys, CancellationToken ct = default)
    {
        var keys = groupKeys.ToHashSet();
        return _context.Set<MenuGroup>()
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(g => keys.Contains(g.Key))
            .Select(g => new { g.Id, g.Key })
            .ToDictionaryAsync(x => x.Key, x => x.Id, ct);
    }

    public Task<List<PagePermission>> GetUserPagePermissionRowsAsync(IReadOnlyCollection<Guid> userIds, CancellationToken ct = default)
        => _context.Set<PagePermission>()
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(pp => pp.UserId != null && userIds.Contains(pp.UserId.Value) && pp.PageId != null)
            .ToListAsync(ct);

    public Task<List<PagePermission>> GetUserGroupPermissionRowsAsync(IReadOnlyCollection<Guid> userIds, CancellationToken ct = default)
        => _context.Set<PagePermission>()
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(pp => pp.UserId != null && userIds.Contains(pp.UserId.Value) && pp.MenuGroupId != null)
            .ToListAsync(ct);

    public Task<List<PagePermission>> GetRolePagePermissionRowsAsync(IReadOnlyCollection<Guid> roleIds, CancellationToken ct = default)
        => _context.Set<PagePermission>()
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(pp => pp.RoleId != null && roleIds.Contains(pp.RoleId.Value) && pp.PageId != null)
            .ToListAsync(ct);

    public Task<List<PagePermission>> GetRoleGroupPermissionRowsAsync(IReadOnlyCollection<Guid> roleIds, CancellationToken ct = default)
        => _context.Set<PagePermission>()
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(pp => pp.RoleId != null && roleIds.Contains(pp.RoleId.Value) && pp.MenuGroupId != null)
            .ToListAsync(ct);

    public void Add(PagePermission row) => _context.Set<PagePermission>().Add(row);

    public void RemoveRange(IEnumerable<PagePermission> rows) => _context.Set<PagePermission>().RemoveRange(rows);
}
